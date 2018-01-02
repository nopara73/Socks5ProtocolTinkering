// https://www.ietf.org/rfc/rfc1928.txt
// https://tools.ietf.org/html/rfc1929
// https://gitweb.torproject.org/torspec.git/plain/socks-extensions.txt

using Socks5ProtocolTinkering.Models;
using System;
using System.Collections.Generic;
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
			var manager = new TorSocks5Manager(new IPEndPoint(IPAddress.Loopback, 9050));

			var connectionTasks = new HashSet<Task<TorSocks5Client>>
			{
				manager.EstablishTcpConnectionAsync("google.com", 80),
				manager.EstablishTcpConnectionAsync("penis.com", 80),
				manager.EstablishTcpConnectionAsync("bitcoin.com", 80, false),
				manager.EstablishTcpConnectionAsync("bitcoin.org", 80),
				manager.EstablishTcpConnectionAsync("foo.com", 80, false),
				manager.EstablishTcpConnectionAsync("pets.com", 80),
				manager.EstablishTcpConnectionAsync("pets.com", 80),
				manager.EstablishTcpConnectionAsync(new IPEndPoint(IPAddress.Parse("192.64.147.228"), 80)),
				manager.EstablishTcpConnectionAsync("google.com", 443)
			};
			Console.WriteLine(await manager.ReverseResolveAsync(IPAddress.Parse("192.64.147.228")));
			Console.WriteLine(await manager.ResolveAsync("google.com", false));

			foreach(var connTask in connectionTasks)
			{
				TorSocks5Client client = await connTask;
				Console.WriteLine($"Connected to: {client.Destination}");
				client.Dispose();
			}

			Console.WriteLine("Press a key to exit...");
			Console.ReadKey();
		}
    }
}
