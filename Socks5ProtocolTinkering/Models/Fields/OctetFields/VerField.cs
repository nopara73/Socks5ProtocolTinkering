using Socks5ProtocolTinkering.Interfaces;
using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
	public class VerField : OctetSerializableBase
	{
		#region Statics

		public static VerField Socks5 => new VerField(5);

		#endregion

		#region PropertiesAndMembers

		public int Value => ByteValue;

		#endregion

		#region ConstructorsAndInitializers

		public VerField()
		{
			
		}

		public VerField(int value)
		{
			if (value > 255) throw new ArgumentOutOfRangeException(nameof(value));
			ByteValue = (byte)value;
		}

		#endregion		
	}
}
