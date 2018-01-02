﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socks5ProtocolTinkering.Helpers
{
	public static class Guard
	{
		public static T NotNull<T>(string parameterName, T value)
		{
			if (parameterName == null)
			{
				throw new ArgumentNullException(nameof(parameterName), "Parameter cannot be null.");
			}

			if (parameterName == "")
			{
				throw new ArgumentException("Parameter cannot be empty.", nameof(parameterName));
			}

			if (parameterName.Trim() == "")
			{
				throw new ArgumentException("Parameter cannot be whitespace.", nameof(parameterName));
			}

			if (value == null)
			{
				throw new ArgumentNullException(parameterName, "Parameter cannot be null.");
			}

			return value;
		}

		public static IEnumerable<T> NotNullOrEmpty<T>(string parameterName, IEnumerable<T> value)
		{
			NotNull(parameterName, value);

			if (value.Count() == 0)
			{
				throw new ArgumentException("Parameter cannot be empty.", parameterName);
			}

			return value;
		}

		public static T[] NotNullOrEmpty<T>(string parameterName, T[] value)
		{
			NotNull(parameterName, value);

			if (value.Count() == 0)
			{
				throw new ArgumentException("Parameter cannot be empty.", parameterName);
			}

			return value;
		}

		public static string NotNullOrEmptyOrWhitespace(string parameterName, string value, bool trim = false)
		{
			NotNullOrEmpty(parameterName, value);

			string trimmedValue = value.Trim();
			if (trimmedValue == "")
			{
				throw new ArgumentException("Parameter cannot be whitespace.", parameterName);
			}

			if (trim)
			{
				return trimmedValue;
			}
			else
			{
				return value;
			}
		}

		public static int MinimumAndNotNull(string parameterName, int? value, int smallest)
		{
			NotNull(parameterName, value);

			if (value < smallest)
			{
				throw new ArgumentOutOfRangeException(parameterName, value, $"Parameter cannot be less than `{smallest}`.");
			}

			return (int)value;
		}

		public static int MaximumAndNotNull(string parameterName, int? value, int greatest)
		{
			NotNull(parameterName, value);

			if (value > greatest)
			{
				throw new ArgumentOutOfRangeException(parameterName, value, $"Parameter cannot be greater than `{greatest}`.");
			}

			return (int)value;
		}

		public static int InRangeAndNotNull(string parameterName, int? value, int smallest, int greatest)
		{
			MinimumAndNotNull(parameterName, value, smallest);
			MaximumAndNotNull(parameterName, value, greatest);

			return (int)value;
		}
	}
}
