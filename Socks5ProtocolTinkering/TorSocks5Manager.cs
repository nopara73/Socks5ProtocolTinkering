﻿using Socks5ProtocolTinkering.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering
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

		public async Task<string> ReverseResolveAsync(IPAddress ipv4, bool isolateStream = true)
		{
			Guard.NotNull(nameof(ipv4), ipv4);

			using (var client = new TorSocks5Client(TorSocks5EndPoint))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(isolateStream);
				return await client.ReverseResolveAsync(ipv4);
			}
		}

		#endregion
	}
}
