using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
	public class ULenField : OctetSerializableBase
	{
		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public ULenField()
		{

		}

		public ULenField(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			ByteValue = (byte)value;
		}

		#endregion

		#region Serialization

		public void FromUNameField(UNameField uName)
		{
			ByteValue = (byte)uName.ToBytes().Length;
		}

		#endregion
	}
}
