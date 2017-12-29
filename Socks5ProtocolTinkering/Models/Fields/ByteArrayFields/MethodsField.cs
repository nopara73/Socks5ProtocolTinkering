using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
    public class MethodsField : ByteArraySerializableBase
	{
		#region Statics

		public static MethodsField NoAuthenticationRequired
		{
			get
			{
				var methodsField = new MethodsField();
				methodsField.FromHex("00");
				return methodsField;
			}
		}

		public static MethodsField GssApi
		{
			get
			{
				var methodsField = new MethodsField();
				methodsField.FromHex("01");
				return methodsField;
			}
		}

		public static MethodsField UsernamePassword
		{
			get
			{
				var methodsField = new MethodsField();
				methodsField.FromHex("02");
				return methodsField;
			}
		}

		/// <summary>
		/// X'03' to X'7F'
		/// </summary>
		public static MethodsField FromIanaAssigned(string hex)
		{
			ByteHelpers.AssertRange(hex, "03", "7F");

			var methodsField = new MethodsField();
			methodsField.FromHex(hex);

			return methodsField;
		}

		/// <summary>
		/// X'80' to X'FE'
		/// </summary>
		public static MethodsField FromReservedPrivateMethod(string hex)
		{
			ByteHelpers.AssertRange(hex, "80", "FE");

			var methodsField = new MethodsField();
			methodsField.FromHex(hex);

			return methodsField;
		}

		public static MethodsField NoAcceptableMethods
		{
			get
			{
				var methodsField = new MethodsField();
				methodsField.FromHex("FF");
				return methodsField;
			}
		}

		#endregion

		#region PropertiesAndMembers

		private byte[] Bytes { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public MethodsField()
		{

		}

		#endregion

		#region Serialization

		#region Serialization

		public override void FromBytes(byte[] bytes)
		{
			if(bytes == null) throw new ArgumentNullException(nameof(bytes));
			var hex = ByteHelpers.ToHex(bytes);
			ByteHelpers.AssertRange(hex, "00", "FF"); // Works for Socks 5
			Bytes = bytes;
		}

		public override byte[] ToBytes() => Bytes;

		#endregion

		#endregion
	}
}
