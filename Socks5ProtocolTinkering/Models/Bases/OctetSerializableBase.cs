﻿using Socks5ProtocolTinkering.Helpers;
using Socks5ProtocolTinkering.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Bases
{
    public abstract class OctetSerializableBase : IByteSerializable, IEquatable<OctetSerializableBase>, IEquatable<byte>
	{
		#region PropertiesAndMembers

		protected byte ByteValue { get; set; }

		#endregion

		#region ConstructorsAndInitializers

		public OctetSerializableBase()
		{

		}

		#endregion

		#region Serialization

		public byte ToByte() => ByteValue;

		public void FromByte(byte b) => ByteValue = b;

		public string ToHex() => ByteHelpers.ToHex(ToByte());

		public void FromHex(string hex)
		{
			hex = Guard.NotNullOrEmptyOrWhitespace(nameof(hex), hex, true);

			byte[] bytes = ByteHelpers.FromHex(hex);
			if (bytes.Length != 1)
			{
				throw new FormatException($"`{nameof(hex)}` must be exactly one byte. Actual: `{bytes.Length}` bytes. Value: `{hex}`.");
			}

			ByteValue = bytes[0];
		}

		public override string ToString()
		{
			return ByteValue.ToString();
		}

		#endregion

		#region EqualityAndComparison

		public override bool Equals(object obj) => obj is OctetSerializableBase && this == (OctetSerializableBase)obj;
		public bool Equals(OctetSerializableBase other) => this == other;
		public override int GetHashCode() => ByteValue;
		public static bool operator ==(OctetSerializableBase x, OctetSerializableBase y) => x?.ByteValue == y?.ByteValue;
		public static bool operator !=(OctetSerializableBase x, OctetSerializableBase y) => !(x == y);

		public bool Equals(byte other) => ByteValue == other;

		public static bool operator ==(byte x, OctetSerializableBase y) => x == y?.ByteValue;
		public static bool operator ==(OctetSerializableBase x, byte y) => x?.ByteValue == y;
		public static bool operator !=(byte x, OctetSerializableBase y) => !(x == y);
		public static bool operator !=(OctetSerializableBase x, byte y) => !(x == y);

		#endregion
	}
}
