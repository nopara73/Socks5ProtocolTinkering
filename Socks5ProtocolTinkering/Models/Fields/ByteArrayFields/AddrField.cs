﻿using Socks5ProtocolTinkering.Helpers;
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
	public class AddrField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public AtypField Atyp { get; set; }

		public string DomainOrIpv4
		{
			get
			{
				if(Atyp == AtypField.DomainName)
				{
					return Encoding.ASCII.GetString(Bytes.Skip(1).ToArray()); // UTF8 result in general SOCKS server failure
				}
				else if (Atyp == AtypField.IpV4)
				{
					var values = new string[4];
					for (int i = 0; i < 4; i++)
					{
						values[i] = Bytes[i].ToString(); // it's ok ASCII here, these are always numbers
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

		public AddrField()
		{

		}

		/// <param name="dstAddr">domain or ipv4</param>
		public AddrField(string dstAddr)
		{
			dstAddr = Guard.NotNullOrEmptyOrWhitespace(nameof(dstAddr), dstAddr, true);

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
				var domainBytes = Encoding.ASCII.GetBytes(dstAddr); // Tor only knows ASCII, UTF8 results in general SOCKS server failure
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
			Bytes = Guard.NotNullOrEmpty(nameof(bytes), bytes);

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

		public override string ToString() => DomainOrIpv4;

		#endregion
	}
}
