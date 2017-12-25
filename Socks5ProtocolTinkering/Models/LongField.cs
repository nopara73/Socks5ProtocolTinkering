using Socks5ProtocolTinkering.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
	public abstract class LongField : IByteArraySerializable, IEquatable<LongField>, IEquatable<byte[]>
	{
		#region PropertiesAndMembers

		protected byte[] Bytes { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public LongField()
		{

		}

		public LongField(params byte[] bytes)
		{
			Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
		}

		#endregion

		#region Serialization

		public byte[] ToBytes() => Bytes;

		public void FromBytes(byte[] bytes) => Bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));

		public string ToHex() => ByteHelpers.ToHex(Bytes);

		public void FromHex(string hex)
		{
			if (hex == null) throw new ArgumentNullException(nameof(hex));
			Bytes = ByteHelpers.FromHex(hex);
		}

		public string ToString(Encoding encoding) => encoding.GetString(Bytes);

		public override string ToString()
		{
			return ToString(Encoding.UTF8);
		}

		#endregion

		#region EqualityAndComparison

		public override bool Equals(object obj) => obj is OctetField && this == (LongField)obj;
		public bool Equals(LongField other) => this == other;
		public override int GetHashCode()
		{
			// https://github.com/bcgit/bc-csharp/blob/b19e68a517e56ef08cd2e50df4dcb8a96ddbe507/crypto/src/util/Arrays.cs#L206
			if (Bytes == null)
			{
				return 0;
			}

			int i = Bytes.Length;
			int hash = i + 1;

			while (--i >= 0)
			{
				hash *= 257;
				hash ^= Bytes[i];
			}

			return hash;
		}
		public static bool operator ==(LongField x, LongField y) => ByteHelpers.CompareFastUnsafe(x?.Bytes, y?.Bytes);
		public static bool operator !=(LongField x, LongField y) => !(x == y);

		public bool Equals(byte[] other) => ByteHelpers.CompareFastUnsafe(Bytes, other);

		public static bool operator ==(byte[] x, LongField y) => ByteHelpers.CompareFastUnsafe(x, y?.Bytes);
		public static bool operator ==(LongField x, byte[] y) => ByteHelpers.CompareFastUnsafe(x?.Bytes, y);
		public static bool operator !=(byte[] x, LongField y) => !(x == y);
		public static bool operator !=(LongField x, byte[] y) => !(x == y);

		#endregion
	}
}
