﻿using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.TorSocks5.TorSocks5.Models.Fields.ByteArrayFields
{
    public class PortField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public int DstPort => BitConverter.ToInt16(Bytes, 0);

		#endregion

		#region ConstructorsAndInitializers

		public PortField()
		{

		}

		public PortField(int dstPort)
		{
			Guard.MinimumAndNotNull(nameof(dstPort), dstPort, 0);

		    var bytes = BitConverter.GetBytes(dstPort);
			if(bytes[2] != 0 || bytes[3] != 0)
			{
				throw new FormatException($"`{nameof(dstPort)}` cannot be encoded in two octets. Value: `{dstPort}`.");
			}
			// https://www.ietf.org/rfc/rfc1928.txt
			// DST.PORT desired destination port in network octet order
			Bytes = bytes.Take(2).Reverse().ToArray();
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes) => Bytes = Guard.NotNullOrEmpty(nameof(bytes), bytes);

		public override byte[] ToBytes() => Bytes;

		public override string ToString() => DstPort.ToString();

		#endregion
	}
}
