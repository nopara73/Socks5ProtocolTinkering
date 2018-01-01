using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
	public class ConnectionRequest : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public CmdField Cmd { get; set; }

		public RsvField Rsv { get; set; }

		public AtypField Atyp { get; set; }

		public DstAddrField DstAddr { get; set; }

		public DstPortField DstPort { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public ConnectionRequest()
		{

		}

		public ConnectionRequest(VerField ver, DstAddrField dstAddr, DstPortField dstPort)
		{
			Ver = ver ?? throw new ArgumentNullException(nameof(ver));
			DstAddr = dstAddr ?? throw new ArgumentNullException(nameof(dstAddr));
			DstPort = dstPort ?? throw new ArgumentNullException(nameof(dstPort));
			Cmd = CmdField.Connect;
			Rsv = RsvField.X00;
			Atyp = dstAddr.Atyp;
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (bytes.Length < 6)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			Ver = new VerField();
			Ver.FromByte(bytes[0]);

			Cmd = new CmdField();
			Cmd.FromByte(bytes[1]);

			Rsv = new RsvField();
			Rsv.FromBytes(new byte[] { bytes[2] });

			Atyp = new AtypField();
			Atyp.FromByte(bytes[3]);

			DstAddr = new DstAddrField();
			DstAddr.FromBytes(bytes.Skip(4).Take(bytes.Length - 4 - 2).ToArray());

			DstPort = new DstPortField();
			DstPort.FromBytes(bytes.Skip(bytes.Length - 2).ToArray());
		}

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte(), Cmd.ToByte() }, Rsv.ToBytes(), new byte[] { Atyp.ToByte() }, DstAddr.ToBytes(), DstPort.ToBytes());

		#endregion
	}
}
