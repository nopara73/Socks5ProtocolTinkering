using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
	public class TorSocks5Request : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public CmdField Cmd { get; set; }

		public RsvField Rsv { get; set; }

		public AtypField Atyp { get; set; }

		public AddrField DstAddr { get; set; }

		public PortField DstPort { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public TorSocks5Request()
		{

		}

		public TorSocks5Request(CmdField cmd, AddrField dstAddr, PortField dstPort)
		{
			Cmd = Guard.NotNull(nameof(cmd), cmd);
			DstAddr = Guard.NotNull(nameof(dstAddr), dstAddr);
			DstPort = Guard.NotNull(nameof(dstPort), dstPort);
			Ver = VerField.Socks5;
			Rsv = RsvField.X00;
			Atyp = dstAddr.Atyp;
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			Guard.NotNullOrEmpty(nameof(bytes), bytes);

			if (bytes.Length < 6)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			Ver = new VerField();
			Ver.FromByte(bytes[0]);

			Cmd = new CmdField();
			Cmd.FromByte(bytes[1]);

			Rsv = new RsvField();
			Rsv.FromByte(bytes[2]);

			Atyp = new AtypField();
			Atyp.FromByte(bytes[3]);

			DstAddr = new AddrField();
			DstAddr.FromBytes(bytes.Skip(4).Take(bytes.Length - 4 - 2).ToArray());

			DstPort = new PortField();
			DstPort.FromBytes(bytes.Skip(bytes.Length - 2).ToArray());
		}

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte(), Cmd.ToByte(), Rsv.ToByte(), Atyp.ToByte() }, DstAddr.ToBytes(), DstPort.ToBytes());

		#endregion
	}
}
