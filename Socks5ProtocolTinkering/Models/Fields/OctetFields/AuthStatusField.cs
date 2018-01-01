using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
    public class AuthStatusField : OctetSerializableBase
	{
		#region Statics

		public static AuthStatusField Success => new AuthStatusField(0);

		#endregion

		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public AuthStatusField()
		{

		}

		public AuthStatusField(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			ByteValue = (byte)value;
		}

		#endregion

		#region

		public bool IsSuccess() => Value == 0;

		#endregion
	}
}
