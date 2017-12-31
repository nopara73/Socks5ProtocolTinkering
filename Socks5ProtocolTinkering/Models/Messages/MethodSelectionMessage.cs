using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
    public class MethodSelectionMessage : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public MethodField Method { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public MethodSelectionMessage()
		{

		}

		public MethodSelectionMessage(VerField verField, MethodField methodField)
		{
			Ver = verField ?? throw new ArgumentNullException(nameof(verField));
			Method = methodField ?? throw new ArgumentNullException(nameof(methodField));
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

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte() }, new byte[] { Method.ToByte() });

		#endregion
	}
}
