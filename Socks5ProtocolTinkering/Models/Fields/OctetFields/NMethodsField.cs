using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
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
			if (value > 255)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			ByteValue = (byte)value;
		}

		#endregion

		#region Serialization
		
		public void FromMethodsField(MethodsField methods)
		{
			ByteValue = (byte)methods.ToBytes().Length;
		}

		#endregion
	}
}
