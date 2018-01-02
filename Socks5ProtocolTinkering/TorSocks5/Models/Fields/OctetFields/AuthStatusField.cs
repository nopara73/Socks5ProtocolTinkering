using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.TorSocks5.Models.Fields.OctetFields
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
			ByteValue = (byte)Guard.InRangeAndNotNull(nameof(value), value, 0, 255);
		}

		#endregion

		#region

		public bool IsSuccess() => Value == 0;

		#endregion
	}
}
