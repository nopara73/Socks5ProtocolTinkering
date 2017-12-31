using Nito.AsyncEx;
using Socks5ProtocolTinkering.Models;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using Socks5ProtocolTinkering.Models.Messages;
using System;
using System.Collections.Generic;
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

		public IPEndPoint EndPoint { get; private set; }

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
			EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
			TcpClient = new TcpClient();
			AsyncLock = new AsyncLock();
		}

		public async Task ConnectAsync()
		{
			using (await AsyncLock.LockAsync())
			{
				await TcpClient.ConnectAsync(EndPoint.Address, EndPoint.Port).ConfigureAwait(false);
			}
		}

		public async Task HandshakeAsync(string username = null, string password = null)
		{
			using (await AsyncLock.LockAsync())
			{
				AssertConnected();

				var stream = TcpClient.GetStream();

				var ver = VerField.Socks5;
				MethodsField methods;
				if (username == null && password == null)
				{
					methods = new MethodsField(MethodField.NoAuthenticationRequired);
				}
				else
				{
					if (username == null)
					{
						username = ""; // does Tor works with empty username?
					}
					if(password == null)
					{
						password = ""; // does Tor works with empty password?
					}
					methods = new MethodsField(MethodField.NoAuthenticationRequired, MethodField.UsernamePassword);
				}

				var sendBuffer = new VersionMethodMessage(ver, methods).ToBytes();
				await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length).ConfigureAwait(false);
				await stream.FlushAsync().ConfigureAwait(false);

				var receiveBuffer = new byte[2];
				var receiveCount = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);
				if (receiveCount <= 0)
				{
					throw new InvalidOperationException("Not connected to Tor SOCKS port");
				}
				var methodSelection = new MethodSelectionMessage();
				methodSelection.FromBytes(receiveBuffer);
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
