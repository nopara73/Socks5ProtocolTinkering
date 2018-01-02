using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
	public class UsernamePasswordRequest : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		public AuthVerField Ver { get; set; }

		public ULenField ULen { get; set; }

		public UNameField UName { get; set; }

		public PLenField PLen { get; set; }

		public PasswdField Passwd { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public UsernamePasswordRequest()
		{

		}

		public UsernamePasswordRequest(UNameField uName, PasswdField passwd)
		{
			Ver = AuthVerField.Version1;
			UName = Guard.NotNull(nameof(uName), uName);
			Passwd = Guard.NotNull(nameof(passwd), passwd);

			var pLen = new PLenField();
			var uLen = new ULenField();
			pLen.FromPasswdField(passwd);
			uLen.FromUNameField(uName);
			PLen = pLen;
			ULen = uLen;
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			Guard.NotNullOrEmpty(nameof(bytes), bytes);

			if (bytes.Length < 6 || bytes.Length > 513)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			Ver = new AuthVerField();
			Ver.FromByte(bytes[0]);

			ULen = new ULenField();
			ULen.FromByte(bytes[1]);

			UName = new UNameField();
			UName.FromBytes(bytes.Skip(2).Take(ULen.Value).ToArray());

			PLen = new PLenField();
			PLen.FromByte(bytes[1 + ULen.Value]);
			if (PLen.Value != bytes.Length - 3 + ULen.Value)
			{
				throw new ArgumentException(nameof(bytes));
			}
			Passwd.FromBytes(bytes.Skip(3 + ULen.Value).ToArray());
		}

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte(), ULen.ToByte() }, UName.ToBytes(), new byte[] { PLen.ToByte() }, Passwd.ToBytes());

		#endregion
	}
}
