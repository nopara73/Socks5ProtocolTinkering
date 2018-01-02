﻿using Socks5ProtocolTinkering.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering.TorSocks5
{
    public class TorSocks5Manager
    {
		#region PropertiesAndMembers

		public IPEndPoint TorSocks5EndPoint { get; private set; }

		#endregion

		#region ConstructorsAndInitializers

		public TorSocks5Manager(IPEndPoint endPoint)
		{
			TorSocks5EndPoint = Guard.NotNull(nameof(endPoint), endPoint);
		}

		#endregion

		#region Methods

		public async Task<TorSocks5Client> EstablishTcpConnectionAsync(IPEndPoint destination, bool isolateStream = true)
		{
			Guard.NotNull(nameof(destination), destination);

			var client = new TorSocks5Client(TorSocks5EndPoint);
			await client.ConnectAsync();
			await client.HandshakeAsync(isolateStream);
			await client.ConnectToDestinationAsync(destination);
			return client;
		}

		public async Task<TorSocks5Client> EstablishTcpConnectionAsync(string host, int port, bool isolateStream = true)
		{
			host = Guard.NotNullOrEmptyOrWhitespace(nameof(host), host, true);
			Guard.MinimumAndNotNull(nameof(port), port, 0);

			var client = new TorSocks5Client(TorSocks5EndPoint);
			await client.ConnectAsync();
			await client.HandshakeAsync(isolateStream);
			await client.ConnectToDestinationAsync(host, port);
			return client;
		}

		/// <summary>
		/// When Tor receives a "RESOLVE" SOCKS command, it initiates
		/// a remote lookup of the hostname provided as the target address in the SOCKS
		/// request.
		/// </summary>
		public async Task<IPAddress> ResolveAsync(string host, bool isolateStream = true)
		{
			host = Guard.NotNullOrEmptyOrWhitespace(nameof(host), host, true);
			
			using (var client = new TorSocks5Client(TorSocks5EndPoint))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(isolateStream);
				return await client.ResolveAsync(host);
			}
		}

		/// <summary>
		/// Tor attempts to find the canonical hostname for that IPv4 record
		/// </summary>
		public async Task<string> ReverseResolveAsync(IPAddress iPv4, bool isolateStream = true)
		{
			Guard.NotNull(nameof(iPv4), iPv4);
			Guard.Same($"{nameof(iPv4)}.{nameof(iPv4.AddressFamily)}", AddressFamily.InterNetwork, iPv4.AddressFamily);
			
			using (var client = new TorSocks5Client(TorSocks5EndPoint))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(isolateStream);
				return await client.ReverseResolveAsync(iPv4);
			}
		}

		#endregion
	}
}
