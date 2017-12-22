using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
    public class Message
    {
		private byte[] Bytes { get; }

		public Message(params byte[] bytes)
		{
			Bytes = bytes;
		}

		public byte[] ToBytes => Bytes;

		public string ToString(Encoding encoding) => encoding.GetString(Bytes);
    }
}
