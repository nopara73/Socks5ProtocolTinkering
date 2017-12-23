using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
    public class Message : IEquatable<Message>, IEquatable<byte[]>
	{
		#region PropertiesAndMembers

		private byte[] Bytes { get; }

		#endregion

		#region Constructors

		public Message(params byte[] bytes)
		{
			Bytes = bytes;
		}

		#endregion

		#region Methods

		public byte[] ToBytes() => Bytes;

		public string ToString(Encoding encoding) => encoding.GetString(Bytes);

		#endregion

		#region Equality

		public override bool Equals(object obj) => obj is Message && this == (Message)obj;
		public bool Equals(Message other) => this == other;
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
		public static bool operator ==(Message x, Message y)
		{
			return ByteHelpers.CompareFastUnsafe(x?.Bytes, y?.Bytes);
		}
		public static bool operator !=(Message x, Message y) => !(x == y);

		public bool Equals(byte[] other) => ByteHelpers.CompareFastUnsafe(Bytes, other);
		public static bool operator ==(byte[] x, Message y) => ByteHelpers.CompareFastUnsafe(x, y?.Bytes);
		public static bool operator ==(Message x, byte[] y) => ByteHelpers.CompareFastUnsafe(x?.Bytes, y);
		public static bool operator !=(byte[] x, Message y) => !(x == y);
		public static bool operator !=(Message x, byte[] y) => !(x == y);

		#endregion
	}
}
