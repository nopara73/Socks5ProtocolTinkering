using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
    public class UsernamePasswordResponse : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public AuthVerField Ver { get; set; }

		public AuthStatusField Status { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public UsernamePasswordResponse()
		{

		}

		public UsernamePasswordResponse(AuthVerField ver, AuthStatusField status)
		{
			Ver = ver ?? throw new ArgumentNullException(nameof(ver));
			Status = status ?? throw new ArgumentNullException(nameof(status));
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

			Ver = new AuthVerField();
			Ver.FromByte(bytes[0]);

			Status = new AuthStatusField();
			Status.FromByte(bytes[1]);
		}

		public override byte[] ToBytes() => new byte[] { Ver.ToByte(), Status.ToByte() };

		#endregion
	}
}
