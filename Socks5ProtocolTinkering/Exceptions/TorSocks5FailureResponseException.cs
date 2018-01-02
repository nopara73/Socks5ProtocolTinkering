using Socks5ProtocolTinkering.TorSocks5.Models.Fields.OctetFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Exceptions
{
	public class TorSocks5FailureResponseException : Exception
	{
		public TorSocks5FailureResponseException(RepField rep) : base($"Tor SOCKS5 proxy responded with `{rep.ToString()}`.")
		{

		}
	}
}
