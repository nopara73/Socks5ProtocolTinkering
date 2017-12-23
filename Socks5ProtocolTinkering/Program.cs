// https://www.ietf.org/rfc/rfc1928.txt
// https://tools.ietf.org/html/rfc1929
// https://gitweb.torproject.org/torspec.git/plain/socks-extensions.txt

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Socks5ProtocolTinkering
{
    class Program
    {
#pragma warning disable IDE1006 // Naming Styles
		static async Task Main(string[] args)
#pragma warning restore IDE1006 // Naming Styles
		{
			for (int i = 0; i < 100; i++)
			{
				var b = (byte)i;
				Console.WriteLine((int)b);
			}

			Console.ReadLine();
        }
    }
}
