using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
	public class DstAddrField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public AtypField Atyp { get; set; }

		public string DstAddr
		{
			get
			{
				if(Atyp == AtypField.DomainName)
				{
					return Encoding.UTF8.GetString(Bytes.Skip(1).ToArray()); // TODO: UTF8 is fine here?
				}
				else if (Atyp == AtypField.IpV4)
				{
					var values = new string[4];
					for (int i = 0; i < 4; i++)
					{
						values[i] = Encoding.ASCII.GetString(new byte[] { Bytes[i] }); // it's ok ASCII here, these are always numbers
					}
					return string.Join(".", values);
				}
				else
				{
					throw new NotSupportedException(); // cannot happen
				}
			}
		}

		#endregion

		#region ConstructorsAndInitializers

		public DstAddrField()
		{

		}

		/// <param name="dstAddr">domain or ipv4</param>
		public DstAddrField(string dstAddr)
		{
			if (string.IsNullOrWhiteSpace(dstAddr)) throw new ArgumentException(nameof(dstAddr));

			var atyp = new AtypField();
			atyp.FromDstAddr(dstAddr);

			Atyp = atyp;

			byte[] bytes;
			if(atyp == AtypField.DomainName)
			{
				// https://www.ietf.org/rfc/rfc1928.txt
				// the address field contains a fully-qualified domain name.  The first
				// octet of the address field contains the number of octets of name that
				// follow, there is no terminating NUL octet.
				var domainBytes = Encoding.UTF8.GetBytes(dstAddr);
				var numberOfOctets = domainBytes.Length;
				if (numberOfOctets > 255)
				{
					throw new ArgumentOutOfRangeException(nameof(dstAddr));
				}

				bytes = ByteHelpers.Combine(new byte[] { (byte)numberOfOctets }, domainBytes);
			}
			else if(atyp == AtypField.IpV4)
			{
				// the address is a version-4 IP address, with a length of 4 octets
				var parts = dstAddr.Split(".", StringSplitOptions.RemoveEmptyEntries);
				if(parts.Length != 4 || parts.Any(x => string.IsNullOrWhiteSpace(x)))
				{
					throw new ArgumentException(nameof(dstAddr));
				}

				bytes = new byte[4];
				for(int i = 0; i < 4; i++)
				{
					if(int.TryParse(parts[i], out int partInt))
					{
						if(partInt < 0 || partInt > 255)
						{
							throw new ArgumentException(nameof(dstAddr));
						}
						bytes[i] = (byte)partInt;
					}
					else
					{
						throw new ArgumentException(nameof(dstAddr));
					}
				}
			}
			else
			{
				throw new ArgumentException(dstAddr);
			}

			Bytes = bytes;
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
			if (bytes.Length == 0)
			{
				throw new ArgumentException(nameof(bytes));
			}

			AtypField atyp;
			if (bytes.First() == bytes.Length - 1 && bytes.Length != 4)
			{
				atyp = AtypField.DomainName;
			}
			else if(bytes.Length == 4)
			{
				atyp = AtypField.IpV4;
			}
			else
			{
				throw new ArgumentException(nameof(bytes));
			}

			Atyp = atyp;
		}

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
