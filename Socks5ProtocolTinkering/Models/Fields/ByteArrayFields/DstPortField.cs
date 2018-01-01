using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
    public class DstPortField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public int DstPort => BitConverter.ToInt16(Bytes, 0);

		#endregion

		#region ConstructorsAndInitializers

		public DstPortField()
		{

		}

		public DstPortField(int dstPort)
		{
		   var bytes = BitConverter.GetBytes(dstPort);
			if(bytes[2] != 0 || bytes[3] != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(dstPort));
			}
			// https://www.ietf.org/rfc/rfc1928.txt
			// DST.PORT desired destination port in network octet order
			Bytes = bytes.Take(2).Reverse().ToArray();
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
