using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.ByteArrayFields
{
	public class RsvField : OctetSerializableBase
	{		
		#region Statics

		public static RsvField X00
		{
			get
			{
				var rsv = new RsvField();
				rsv.FromHex("00");
				return rsv;
			}
		}
		
		#endregion

		#region ConstructorsAndInitializers

		public RsvField()
		{

		}

		#endregion
	}
}
