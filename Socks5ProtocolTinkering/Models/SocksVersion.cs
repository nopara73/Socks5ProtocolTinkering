using Socks5ProtocolTinkering.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
	public class SocksVersion : IByteSerializable, IEquatable<SocksVersion>, IEquatable<byte>
	{
		#region Statics

		public static SocksVersion Socks5 => new SocksVersion(5);

		#endregion

		#region PropertiesAndMembers

		private int Value { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public SocksVersion()
		{
			
		}

		private SocksVersion(int value)
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
			byte[] bytes = ByteHelpers.GetBytes(hex);
			if (bytes.Length != 1) throw new ArgumentException(nameof(hex));
			Value = bytes[0];
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		#endregion

		#region EqualityAndComparison

		public override bool Equals(object obj) => obj is SocksVersion && this == (SocksVersion)obj;
		public bool Equals(SocksVersion other) => this == other;
		public override int GetHashCode() => Value;
		public static bool operator ==(SocksVersion x, SocksVersion y) => x?.Value == y?.Value;
		public static bool operator !=(SocksVersion x, SocksVersion y) => !(x == y);

		public bool Equals(byte other) => Value == other;

		public static bool operator ==(byte x, SocksVersion y) => x == y?.Value;
		public static bool operator ==(SocksVersion x, byte y) => x?.Value == y;
		public static bool operator !=(byte x, SocksVersion y) => !(x == y);
		public static bool operator !=(SocksVersion x, byte y) => !(x == y);

		#endregion
	}
}
