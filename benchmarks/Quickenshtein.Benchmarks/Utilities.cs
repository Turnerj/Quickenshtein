using System;
using System.Text;

namespace Quickenshtein.Benchmarks
{
	public static class Utilities
	{
		public static string BuildString(string baseString, int numberOfCharacters)
		{
			var builder = new StringBuilder(numberOfCharacters);
			var charBlocksRemaining = (int)Math.Floor((double)numberOfCharacters / baseString.Length);

			while (charBlocksRemaining >= 8)
			{
				charBlocksRemaining -= 8;
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
			}

			if (charBlocksRemaining > 4)
			{
				charBlocksRemaining -= 4;
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
				builder.Append(baseString);
			}

			while (charBlocksRemaining > 0)
			{
				charBlocksRemaining--;
				builder.Append(baseString);
			}

			var remainder = (int)((double)numberOfCharacters / baseString.Length % 1 * baseString.Length);
			builder.Append(baseString.Substring(0, remainder));

			return builder.ToString();
		}
	}
}
