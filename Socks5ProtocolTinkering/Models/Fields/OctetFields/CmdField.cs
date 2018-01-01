using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
	public class CmdField : OctetSerializableBase
	{
		#region Statics

		// https://gitweb.torproject.org/torspec.git/tree/socks-extensions.txt
		// The BIND command is not supported.
		// The (SOCKS5) "UDP ASSOCIATE" command is not supported.

		public static CmdField Connect
		{
			get
			{
				var cmd = new CmdField();
				cmd.FromHex("01");
				return cmd;
			}
		}

		#endregion

		#region ConstructorsAndInitializers

		public CmdField()
		{

		}

		#endregion
	}
}
