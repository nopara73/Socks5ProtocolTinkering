using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
    public class VersionMethodMessage : Message
    {
		public VersionMethodMessage(params byte[] bytes) : base(bytes)
		{
			
		}
    }
}
