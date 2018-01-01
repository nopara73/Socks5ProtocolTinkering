using Socks5ProtocolTinkering.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socks5ProtocolTinkering.Models.Fields.OctetFields
{
	public class RepField : OctetSerializableBase
	{
		#region Statics

		// https://www.ietf.org/rfc/rfc1928.txt
		// REP    Reply field:
		// o X'00' succeeded
		// o X'01' general SOCKS server failure
		// o X'02' connection not allowed by ruleset
		// o X'03' Network unreachable
		// o X'04' Host unreachable
		// o X'05' Connection refused
		// o X'06' TTL expired
		// o X'07' Command not supported
		// o X'08' Address type not supported
		// o X'09' to X'FF' unassigned

		public static RepField Succeeded
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("00");
				return cmd;
			}
		}

		public static RepField GeneralSocksServerFailure
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("01");
				return cmd;
			}
		}

		public static RepField CconnectionNotAllowedByRuleset
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("02");
				return cmd;
			}
		}

		public static RepField NetworkUnreachable
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("03");
				return cmd;
			}
		}

		public static RepField HostUnreachable
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("04");
				return cmd;
			}
		}

		public static RepField ConnectionRefused
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("05");
				return cmd;
			}
		}

		public static RepField TtlExpired
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("06");
				return cmd;
			}
		}

		public static RepField CommandNoSupported
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("07");
				return cmd;
			}
		}

		public static RepField AddressTypeNotSupported
		{
			get
			{
				var cmd = new RepField();
				cmd.FromHex("08");
				return cmd;
			}
		}

		#endregion

		#region ConstructorsAndInitializers

		public RepField()
		{

		}

		#endregion
	}
}
