using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
    public class MethodSelectionResponse : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public MethodField Method { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public MethodSelectionResponse()
		{

		}

		public MethodSelectionResponse(VerField ver, MethodField method)
		{
			Ver = ver ?? throw new ArgumentNullException(nameof(ver));
			Method = method ?? throw new ArgumentNullException(nameof(method));
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (bytes.Length != 2)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			Ver = new VerField();
			Ver.FromByte(bytes[0]);

			Method = new MethodField();
			Method.FromByte(bytes[1]);
		}

		public override byte[] ToBytes() => new byte[] { Ver.ToByte(), Method.ToByte() };

		#endregion
	}
}
