using Socks5ProtocolTinkering.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
    public abstract class OctetField : IByteSerializable, IEquatable<OctetField>, IEquatable<byte>
	{
		#region PropertiesAndMembers

		protected int Value { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public OctetField()
		{

		}

		public OctetField(int value)
		{
			Value = value;
		}

		#endregion

		#region Serialization

		public byte ToByte() => (byte)Value;

		public void FromByte(byte b) => Value = b;

		public string ToHex() => ByteHelpers.ToHex(ToByte());

		public void FromHex(string hex)
		{
			if (hex == null) throw new ArgumentNullException(nameof(hex));
			byte[] bytes = ByteHelpers.FromHex(hex);
			if (bytes.Length != 1) throw new ArgumentException(nameof(hex));
			Value = bytes[0];
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		#endregion

		#region EqualityAndComparison

		public override bool Equals(object obj) => obj is OctetField && this == (OctetField)obj;
		public bool Equals(OctetField other) => this == other;
		public override int GetHashCode() => Value;
		public static bool operator ==(OctetField x, OctetField y) => x?.Value == y?.Value;
		public static bool operator !=(OctetField x, OctetField y) => !(x == y);

		public bool Equals(byte other) => Value == other;

		public static bool operator ==(byte x, OctetField y) => x == y?.Value;
		public static bool operator ==(OctetField x, byte y) => x?.Value == y;
		public static bool operator !=(byte x, OctetField y) => !(x == y);
		public static bool operator !=(OctetField x, byte y) => !(x == y);

		#endregion
	}
}
