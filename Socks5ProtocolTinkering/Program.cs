// https://www.ietf.org/rfc/rfc1928.txt
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
				await client.HandshakeAsync(false);
				Console.WriteLine(await client.ReverseResolveAsync(IPAddress.Parse("192.64.147.228")));
			}
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(false);
				Console.WriteLine(await client.ResolveAsync("google.com"));
			}
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(false);
				await client.ConnectToDestinationAsync("google.com", 80);
			}
			using (var client = new TorSocks5Client(new IPEndPoint(IPAddress.Loopback, 9050)))
			{
				await client.ConnectAsync();
				await client.HandshakeAsync(false);
				await client.ConnectToDestinationAsync("192.64.147.228", 80);
			}
			Console.WriteLine("Press a key to exit...");
			Console.ReadKey();
		}
    }
}
