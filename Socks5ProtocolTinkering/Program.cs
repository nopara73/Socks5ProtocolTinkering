﻿// https://www.ietf.org/rfc/rfc1928.txt
// https://tools.ietf.org/html/rfc1929
// https://gitweb.torproject.org/torspec.git/plain/socks-extensions.txt

using Socks5ProtocolTinkering.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering
{
    class Program
    {
#pragma warning disable IDE1006 // Naming Styles
		static async Task Main(string[] args)
#pragma warning restore IDE1006 // Naming Styles
		{
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(true);
				await client.ConnectToDestinationAsync("172.217.6.142", 80);
			}
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(true);
				await client.ConnectToDestinationAsync("google.com", 80);
			}
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(true);
				await client.ConnectToDestinationAsync("google.com", 443);
			}
		}
    }
}
