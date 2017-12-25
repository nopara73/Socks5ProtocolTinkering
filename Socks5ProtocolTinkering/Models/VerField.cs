using Socks5ProtocolTinkering.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models
{
	public class VerField : OctetField
	{
		#region Statics

		public static VerField Socks5 => new VerField(5);

		#endregion

		#region ConstructorsAndInitializers

		public VerField()
		{
			
		}

		public VerField(int value) : base(value)
		{

		}

		#endregion		
	}
}
