using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
	public class PLenField : OctetSerializableBase
	{
		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public PLenField()
		{

		}

		public PLenField(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentOutOfRangeException(nameof(value));
			}

			ByteValue = (byte)value;
		}

		#endregion

		#region Serialization

		public void FromPasswdField(PasswdField passwd)
		{
			ByteValue = (byte)passwd.ToBytes().Length;
		}

		#endregion
	}
}
