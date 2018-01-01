﻿using Socks5ProtocolTinkering.Models.Bases;
using Socks5ProtocolTinkering.Models.Fields.ByteArrayFields;
using Socks5ProtocolTinkering.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Messages
{
    public class VersionMethodRequest : ByteArraySerializableBase
    {
		#region PropertiesAndMembers

		public VerField Ver { get; set; }

		public NMethodsField NMethods { get; set; }

		public MethodsField Methods { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public VersionMethodRequest()
		{

		}

		public VersionMethodRequest(VerField ver, MethodsField methods)
		{
			Ver = ver ?? throw new ArgumentNullException(nameof(ver));
			Methods = methods ?? throw new ArgumentNullException(nameof(methods));

			// The NMETHODS field contains the number of method identifier octets that appear in the METHODS field.
			var nMethods = new NMethodsField();
			nMethods.FromMethodsField(methods);
			NMethods = nMethods;
		}

		#endregion

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException(nameof(bytes));
			}

			if (bytes.Length < 3 || bytes.Length > 257)
			{
				throw new ArgumentOutOfRangeException(nameof(bytes));
			}

			Ver = new VerField();
			Ver.FromByte(bytes[0]);

			NMethods = new NMethodsField();
			NMethods.FromByte(bytes[1]);

			if(NMethods.Value != bytes.Length - 2)
			{
				throw new ArgumentException(nameof(bytes));
			}

			Methods = new MethodsField();
			Methods.FromBytes(bytes.Skip(2).ToArray());
		}

		public override byte[] ToBytes() => ByteHelpers.Combine(new byte[] { Ver.ToByte(), NMethods.ToByte() }, Methods.ToBytes());

		#endregion
	}
}