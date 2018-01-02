﻿using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.TorSocks5.TorSocks5.Models.Fields.ByteArrayFields
{
	public class PasswdField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public string Passwd => Encoding.UTF8.GetString(Bytes); // Tor accepts UTF8 encoded passwd

		#endregion

		#region ConstructorsAndInitializers

		public PasswdField()
		{

		}

		public PasswdField(string passwd)
		{
			Guard.NotNullOrEmpty(nameof(passwd), passwd);
			Bytes = Encoding.UTF8.GetBytes(passwd);
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes) => Bytes = Guard.NotNullOrEmpty(nameof(bytes), bytes);

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
