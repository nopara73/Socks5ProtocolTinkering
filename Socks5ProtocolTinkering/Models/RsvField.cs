using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
	public class RsvField : LongField
	{
		#region Statics

		public static RsvField X00
		{
			get
			{
				var rsvField = new RsvField();
				rsvField.FromHex("00");
				return rsvField;
			}
		}

		public static RsvField X0000
		{
			get
			{
				var rsvField = new RsvField();
				rsvField.FromHex("0000");
				return rsvField;
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
