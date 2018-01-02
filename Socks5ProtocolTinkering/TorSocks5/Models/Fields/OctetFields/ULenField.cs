using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Bases;
using Socks5ProtocolTinkering.TorSocks5.TorSocks5.Models.Fields.ByteArrayFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.TorSocks5.Models.Fields.OctetFields
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
			ByteValue = (byte)Guard.InRangeAndNotNull(nameof(value), value, 0, 255);
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
