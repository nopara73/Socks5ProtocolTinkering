using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
	public class RsvField : ByteArraySerializableBase
	{		
		#region Statics

		public static RsvField X00
		{
			get
			{
				var rsvField = new RsvField();
				rsvField.FromHex("00");
				return rsvField;
			}
		}

		public static RsvField X0000
		{
			get
			{
				var rsvField = new RsvField();
				rsvField.FromHex("0000");
				return rsvField;
			}
		}

		#endregion

		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public RsvField()
		{

		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
