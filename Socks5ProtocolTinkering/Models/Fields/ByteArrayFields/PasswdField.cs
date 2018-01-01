using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
	public class PasswdField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public string Passwd => Encoding.UTF8.GetString(Bytes); // does Tor accept utf8 or only acii?

		#endregion

		#region ConstructorsAndInitializers

		public PasswdField()
		{

		}

		public PasswdField(string passwd)
		{
			if (string.IsNullOrEmpty(passwd)) throw new ArgumentException(nameof(passwd));
			Bytes = Encoding.UTF8.GetBytes(passwd);
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
