using System;
using System.Text;

namespace Quickenshtein.Tests
{
	public static class Utilities
	{
		public static string BuildString(string baseString, int numberOfCharacters)
		{
			var builder = new StringBuilder(numberOfCharacters);
			var charBlocksRemaining = (int)Math.Floor((double)numberOfCharacters / baseString.Length);

			while (charBlocksRemaining > 0)
			{
				charBlocksRemaining--;
				builder.Append(baseString);
			}

			var remainder = numberOfCharacters % baseString.Length;
			if (remainder > 0)
			{
				builder.Append(baseString.Substring(0, remainder));
			}

			return builder.ToString();
		}
	}
}
