using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Interfaces
{
    public interface IByteArraySerializable
    {
		byte[] ToBytes();
		void FromBytes(params byte[] bytes);
		string ToHex();
		void FromHex(string hex);
	}
}
