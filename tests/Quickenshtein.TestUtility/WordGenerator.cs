using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.TestUtility
{
	class WordGenerator
	{
		private const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static readonly Random Random = new Random();

		public static string GenerateWord(int wordLength)
		{
			var builder = new StringBuilder(wordLength);
			for (var i = 0; i < wordLength; i++)
			{
				var characterIndex = Random.Next(0, CHARACTERS.Length);
				builder.Append(CHARACTERS[characterIndex]);
			}
			return builder.ToString();
		}
	}
}
