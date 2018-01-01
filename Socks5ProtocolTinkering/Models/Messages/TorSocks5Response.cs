﻿using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
	public class TorSocks5Response : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public RepField Rep { get; set; }

		public RsvField Rsv { get; set; }

		public AtypField Atyp { get; set; }

		public AddrField BndAddr { get; set; }

		public PortField BndPort { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public TorSocks5Response()
		{

		}

		public TorSocks5Response(RepField rep, AddrField bndAddr, PortField bndPort)
		{
			Rep = rep ?? throw new ArgumentNullException(nameof(rep));
			BndAddr = bndAddr ?? throw new ArgumentNullException(nameof(bndAddr));
			BndPort = bndPort ?? throw new ArgumentNullException(nameof(bndPort));
			Ver = VerField.Socks5;
			Rsv = RsvField.X00;
			Atyp = bndAddr.Atyp;
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

			Rep = new RepField();
			Rep.FromByte(bytes[1]);

			Rsv = new RsvField();
			Rsv.FromByte(bytes[2]);

			Atyp = new AtypField();
			Atyp.FromByte(bytes[3]);

			BndAddr = new AddrField();
			BndAddr.FromBytes(bytes.Skip(4).Take(bytes.Length - 4 - 2).ToArray());

			BndPort = new PortField();
			BndPort.FromBytes(bytes.Skip(bytes.Length - 2).ToArray());
		}

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte(), Rep.ToByte(), Rsv.ToByte(), Atyp.ToByte() }, BndAddr.ToBytes(), BndPort.ToBytes());

		#endregion
	}
}