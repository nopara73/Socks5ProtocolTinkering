using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
    public class NMethodsField : OctetSerializableBase
    {
		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public NMethodsField()
		{

		}

		public NMethodsField(int value)
		{
			if (value > 255) throw new ArgumentOutOfRangeException(nameof(value));
			ByteValue = (byte)value;
		}

		#endregion
	}
}
