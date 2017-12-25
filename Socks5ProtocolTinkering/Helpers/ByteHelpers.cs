using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ByteHelpers
    {
		// Fastest byte array comparison in C#
		// https://stackoverflow.com/a/8808245/2061103
		// Copyright (c) 2008-2013 Hafthor Stefansson
		// Distributed under the MIT/X11 software license
		// Ref: http://www.opensource.org/licenses/mit-license.php.
		public static unsafe bool CompareFastUnsafe(byte[] a1, byte[] a2)
		{
			if (a1 == a2) return true;
			if (a1 == null || a2 == null || a1.Length != a2.Length)
				return false;
			fixed (byte* p1 = a1, p2 = a2)
			{
				byte* x1 = p1, x2 = p2;
				int l = a1.Length;
				for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
					if (*((long*)x1) != *((long*)x2)) return false;
				if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
				if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
				if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
				return true;
			}
		}

		// Fastest byte array to hex implementation in C#
		// https://stackoverflow.com/a/5919521/2061103
		// https://stackoverflow.com/a/10048895/2061103
		public static string ToHex(params byte[] bytes)
		{
			var result = new StringBuilder(bytes.Length * 2);
			var hexAlphabet = "0123456789ABCDEF";

			foreach (byte b in bytes)
			{
				result.Append(hexAlphabet[b >> 4]);
				result.Append(hexAlphabet[b & 0xF]);
			}

			return result.ToString();
		}

		// Fastest hex to byte array implementation in C#
		// https://stackoverflow.com/a/5919521/2061103
		// https://stackoverflow.com/a/10048895/2061103
		public static byte[] GetBytes(string hex)
		{
			var bytes = new byte[hex.Length / 2];
			var hexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
	   0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
	   0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

			for (int x = 0, i = 0; i < hex.Length; i += 2, x += 1)
			{
				bytes[x] = (byte)(hexValue[char.ToUpper(hex[i + 0]) - '0'] << 4 |
								  hexValue[char.ToUpper(hex[i + 1]) - '0']);
			}

			return bytes;
		}
	}
}
