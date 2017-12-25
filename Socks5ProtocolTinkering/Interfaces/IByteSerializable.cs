using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Interfaces
{
    public interface IByteSerializable
    {
		byte ToByte();
		void FromByte(byte b);
		string ToHex();
		void FromHex(string hex);
	}
}
