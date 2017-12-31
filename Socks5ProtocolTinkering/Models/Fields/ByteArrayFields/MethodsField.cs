using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
    public class MethodsField : ByteArraySerializableBase
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		public IEnumerable<MethodField> Methods
		{
			get
			{
				foreach(var b in Bytes)
				{
					var method = new MethodField();
					method.FromByte(b);
					yield return method;
				}
			}
		}

		#endregion

		#region ConstructorsAndInitializers

		public MethodsField()
		{
			
		}

		public MethodsField(params MethodField[] methods)
		{
			if (methods == null || methods.Length == 0) throw new ArgumentException(nameof(methods));
			int count = methods.Length;
			Bytes = new byte[count];
			for (int i = 0; i < count; i++)
			{
				Bytes[i] = methods[i].ToByte();
			}
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			if(bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			foreach(var b in bytes)
			{
				if(b != MethodField.NoAuthenticationRequired && b != MethodField.UsernamePassword)
				{
					throw new ArgumentOutOfRangeException(nameof(bytes));
				}
			}

			Bytes = bytes;
		}

		public override byte[] ToBytes() => Bytes;

		#endregion
	}
}
