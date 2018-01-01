﻿using Nito.AsyncEx;
using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Models;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using Socks5ProtocolTinkering.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering
{
	public class TorSocks5Client : IDisposable
	{
		#region PropertiesAndMembers

		public TcpClient TcpClient { get; private set; }

		public IPEndPoint TorSocks5EndPoint { get; private set; }
		
		public string Destination { get; private set; }

		public bool IsConnected
		{
			get
			{
				try
				{
					if (TcpClient == null || TcpClient.GetStream() == null)
					{
						return false;
					}

					return TcpClient.Connected;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return false;
				}
			}
		}

		private AsyncLock AsyncLock { get; }

		#endregion

		#region ConstructorsAndInitializers

		public TorSocks5Client(IPEndPoint endPoint)
		{
			TorSocks5EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
			TcpClient = new TcpClient();
			AsyncLock = new AsyncLock();
		}

		public async Task ConnectAsync()
		{
			using (await AsyncLock.LockAsync())
			{
				await TcpClient.ConnectAsync(TorSocks5EndPoint.Address, TorSocks5EndPoint.Port).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// IsolateSOCKSAuth must be on (on by default)
		/// https://www.torproject.org/docs/tor-manual.html.en
		/// https://gitweb.torproject.org/torspec.git/tree/socks-extensions.txt#n35
		/// </summary>
		public async Task HandshakeAsync(bool isolateStream = true)
		{
			using (await AsyncLock.LockAsync())
			{
				AssertConnected();

				var stream = TcpClient.GetStream();
				
				MethodsField methods;
				if (!isolateStream)
				{
					methods = new MethodsField(MethodField.NoAuthenticationRequired);
				}
				else
				{
					methods = new MethodsField(MethodField.UsernamePassword);
				}

				var sendBuffer = new VersionMethodRequest(methods).ToBytes();
				await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
				await stream.FlushAsync().ConfigureAwait(false);

				var receiveBuffer = new byte[2];
				var receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
				if (receiveCount <= 0)
				{
					throw new InvalidOperationException("Not connected to Tor SOCKS port");
				}
				var methodSelection = new MethodSelectionResponse();
				methodSelection.FromBytes(receiveBuffer.Take(receiveCount).ToArray());
				if(methodSelection.Ver != VerField.Socks5)
				{
					throw new InvalidOperationException($"SOCKS{methodSelection.Ver.Value} is not supported. Only SOCKS5 is supported");
				}
				if(methodSelection.Method == MethodField.NoAcceptableMethods)
				{
					// https://www.ietf.org/rfc/rfc1928.txt
					// If the selected METHOD is X'FF', none of the methods listed by the
					// client are acceptable, and the client MUST close the connection.
					DisposeTcpClient();
					throw new InvalidOperationException("The SOCKS5 proxy does not support any of the client's authentication methods.");
				}
				if(methodSelection.Method == MethodField.UsernamePassword)
				{
					// https://tools.ietf.org/html/rfc1929#section-2
					// Once the SOCKS V5 server has started, and the client has selected the
					// Username / Password Authentication protocol, the Username / Password
					// subnegotiation begins.  This begins with the client producing a
					// Username / Password request:
					var username = RandomString.Generate(21);
					var password = RandomString.Generate(21);
					var uName = new UNameField(username);
					var passwd = new PasswdField(password);
					var usernamePasswordRequest = new UsernamePasswordRequest(uName, passwd);
					sendBuffer = usernamePasswordRequest.ToBytes();
					await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
					await stream.FlushAsync().ConfigureAwait(false);

					Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
					receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
					if (receiveCount <= 0)
					{
						throw new InvalidOperationException("Not connected to Tor SOCKS port");
					}

					var userNamePasswordResponse = new UsernamePasswordResponse();
					userNamePasswordResponse.FromBytes(receiveBuffer.Take(receiveCount).ToArray());
					if(userNamePasswordResponse.Ver != usernamePasswordRequest.Ver)
					{
						throw new InvalidOperationException("Wrong auth version");
					}
					
					if (!userNamePasswordResponse.Status.IsSuccess()) // In Tor authentication is different, this will never happen;
					{
						// https://tools.ietf.org/html/rfc1929#section-2
						// A STATUS field of X'00' indicates success. If the server returns a
						// `failure' (STATUS value other than X'00') status, it MUST close the
						// connection.
						DisposeTcpClient();
						throw new InvalidOperationException("Wrong username and/or password");
					}
				}
			}
		}

		public async Task ConnectToDestinationAsync(IPEndPoint destination)
		{
			if (destination == null) throw new ArgumentNullException(nameof(destination));
			await ConnectToDestinationAsync(destination.Address.ToString(), destination.Port).ConfigureAwait(false);
		}
		
		/// <param name="host">ipv4 or domain</param>
		public async Task ConnectToDestinationAsync(string host, int port)
		{
			if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException(nameof(host));
			if (port < 0) throw new ArgumentOutOfRangeException(nameof(port));
			host = host.Trim();

			using (await AsyncLock.LockAsync())
			{
				AssertConnected();
				var stream = TcpClient.GetStream();

				var cmd = CmdField.Connect;

				var dstAddr = new AddrField(host);

				var dstPort = new PortField(port);

				var connectionRequest = new TorSocks5Request(cmd, dstAddr, dstPort);
				var sendBuffer = connectionRequest.ToBytes();
				await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
				await stream.FlushAsync().ConfigureAwait(false);

				var receiveBuffer = new byte[TcpClient.ReceiveBufferSize];
				var receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
				if (receiveCount <= 0)
				{
					throw new InvalidOperationException("Not connected to Tor SOCKS port");
				}

				var connectionResponse = new TorSocks5Response();
				connectionResponse.FromBytes(receiveBuffer.Take(receiveCount).ToArray());

				if(connectionResponse.Rep != RepField.Succeeded)
				{
					// https://www.ietf.org/rfc/rfc1928.txt
					// When a reply(REP value other than X'00') indicates a failure, the
					// SOCKS server MUST terminate the TCP connection shortly after sending
					// the reply.This must be no more than 10 seconds after detecting the
					// condition that caused a failure.
					DisposeTcpClient();
					throw new InvalidOperationException(connectionResponse.Rep.ToHex());
				}

				// Don't check the Bnd. Address and Bnd. Port. because Tor doesn't seem to return any, ever. It returns zeros instead.
				// Generally also don't check anything but the success response, according to Socks5 RFC

				// If the reply code(REP value of X'00') indicates a success, and the
				// request was either a BIND or a CONNECT, the client may now start
				// passing data.  If the selected authentication method supports
				// encapsulation for the purposes of integrity, authentication and / or
				// confidentiality, the data are encapsulated using the method-dependent
				// encapsulation.Similarly, when data arrives at the SOCKS server for
				// the client, the server MUST encapsulate the data as appropriate for 
				// the authentication method in use.
			}
		}
		
		public void AssertConnected()
		{
			if (!IsConnected)
			{
				throw new Exception($"{nameof(TorSocks5Client)} is not connected");
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// When Tor receives a "RESOLVE" SOCKS command, it initiates
		/// a remote lookup of the hostname provided as the target address in the SOCKS
		/// request.
		/// </summary>
		public async Task<IPAddress> ResolveAsync(string host)
		{
			// https://gitweb.torproject.org/torspec.git/tree/socks-extensions.txt#n44

			if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException(nameof(host));
			host = host.Trim();

			using (await AsyncLock.LockAsync())
			{
				AssertConnected();
				var stream = TcpClient.GetStream();

				var cmd = CmdField.Resolve;

				var dstAddr = new AddrField(host);

				var dstPort = new PortField(0);

				var resolveRequest = new TorSocks5Request(cmd, dstAddr, dstPort);
				var sendBuffer = resolveRequest.ToBytes();
				await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
				await stream.FlushAsync().ConfigureAwait(false);

				var receiveBuffer = new byte[TcpClient.ReceiveBufferSize];
				var receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
				if (receiveCount <= 0)
				{
					throw new InvalidOperationException("Not connected to Tor SOCKS port");
				}

				var resolveResponse = new TorSocks5Response();
				resolveResponse.FromBytes(receiveBuffer.Take(receiveCount).ToArray());

				if (resolveResponse.Rep != RepField.Succeeded)
				{
					throw new InvalidOperationException(resolveResponse.Rep.ToHex());
				}
				return IPAddress.Parse(resolveResponse.BndAddr.DomainOrIpv4);
			}
		}

		/// <summary>
		/// Tor attempts to find the canonical hostname for that IPv4 record
		/// </summary>
		public async Task<string> ReverseResolveAsync(IPAddress ipv4)
		{
			// https://gitweb.torproject.org/torspec.git/tree/socks-extensions.txt#n55

			if (ipv4 == null) throw new ArgumentNullException(nameof(ipv4));
			if(ipv4.AddressFamily != AddressFamily.InterNetwork)
			{
				throw new ArgumentException(nameof(ipv4));
			}

			using (await AsyncLock.LockAsync())
			{
				AssertConnected();
				var stream = TcpClient.GetStream();

				var cmd = CmdField.ResolvePtr;

				var dstAddr = new AddrField(ipv4.ToString());

				var dstPort = new PortField(0);

				var resolveRequest = new TorSocks5Request(cmd, dstAddr, dstPort);
				var sendBuffer = resolveRequest.ToBytes();
				await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
				await stream.FlushAsync().ConfigureAwait(false);

				var receiveBuffer = new byte[TcpClient.ReceiveBufferSize];
				var receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
				if (receiveCount <= 0)
				{
					throw new InvalidOperationException("Not connected to Tor SOCKS port");
				}

				var resolveResponse = new TorSocks5Response();
				resolveResponse.FromBytes(receiveBuffer.Take(receiveCount).ToArray());

				if (resolveResponse.Rep != RepField.Succeeded)
				{
					throw new InvalidOperationException(resolveResponse.Rep.ToHex());
				}
				return resolveResponse.BndAddr.DomainOrIpv4;
			}
		}

		#endregion

		#region IDisposable Support

		private bool _disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					DisposeTcpClient();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				_disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~TorSocks5Client() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		public void DisposeTcpClient()
		{
			try
			{
				TcpClient?.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				TcpClient = null; // need to be called, .net bug
			}
		}

		#endregion
	}
}
