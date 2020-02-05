using System;
using System.Text;

namespace Quickenshtein.Tests
{
	public static class Utilities
	{
		public static string BuildString(string baseString, int numberOfCharacters)
		{
			var builder = new StringBuilder(numberOfCharacters);
			var charBlocks = (int)Math.Floor((double)numberOfCharacters / baseString.Length);
			for (int i = 0, l = charBlocks; i < l; i++)
			{
				builder.Append(baseString);
			}

			var remainder = (int)((double)numberOfCharacters / baseString.Length % 1 * baseString.Length);
			builder.Append(baseString.Substring(0, remainder));

			return builder.ToString();
		}
	}
}
