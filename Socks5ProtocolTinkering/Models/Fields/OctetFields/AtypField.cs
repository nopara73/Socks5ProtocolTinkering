using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
    public class AtypField : OctetSerializableBase
	{
		#region Statics

		// https://gitweb.torproject.org/torspec.git/tree/socks-extensions.txt
		// IPv6 is not supported in CONNECT commands.

		public static AtypField IpV4
		{
			get
			{
				var atyp = new AtypField();
				atyp.FromHex("01");
				return atyp;
			}
		}

		public static AtypField DomainName
		{
			get
			{
				var atyp = new AtypField();
				atyp.FromHex("03");
				return atyp;
			}
		}

		#endregion

		#region ConstructorsAndInitializers

		public AtypField()
		{

		}

		#endregion

		#region Serialization

		public void FromDstAddr(string dstAddr)
		{
			if(string.IsNullOrWhiteSpace(dstAddr))
			{
				throw new ArgumentException(nameof(dstAddr));
			}

			if (IPAddress.TryParse(dstAddr, out IPAddress address))
			{
				if (address.AddressFamily != AddressFamily.InterNetwork)
				{
					throw new ArgumentException(nameof(dstAddr));
				}
				ByteValue = IpV4.ToByte();
			}
			else
			{
				ByteValue = DomainName.ToByte();
			}
		}

		#endregion
	}
}
