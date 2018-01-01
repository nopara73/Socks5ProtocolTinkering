using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
    public class AuthVerField : OctetSerializableBase
	{
		#region Statics

		public static AuthVerField Version1 => new AuthVerField(1);

		#endregion

		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public AuthVerField()
		{

		}

		public AuthVerField(int value)
		{
			if (value > 255)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			ByteValue = (byte)value;
		}

		#endregion
	}
}
