using System;
using System.Text;

namespace Quickenshtein.TestCaseFinder
{
	class Program
	{
		private const int NUMBER_OF_CHECKS = 2000;
		private const int WORD_LENGTH = 20;

		private const string CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static readonly Random Random = new Random();

		static void Main(string[] args)
		{
			var numberOfFailures = 0;
			Console.WriteLine("=== Test Case Finder ===");
			Console.WriteLine($"Number of Checks: {NUMBER_OF_CHECKS}");
			Console.WriteLine($"World Length: {WORD_LENGTH}");
			Console.WriteLine("========================");

			for (var i = 0; i < NUMBER_OF_CHECKS; i++)
			{
				var source = GetRandomWord();
				var target = GetRandomWord();

				var baseline = Benchmarks.LevenshteinBaseline.GetDistance(source, target);
				var quickenshtein = Levenshtein.GetDistance(source, target);

				if (baseline != quickenshtein)
				{
					Console.WriteLine($"FAILED ({i + 1}): Expected {baseline}, Actual {quickenshtein}");
					Console.WriteLine($"Source: {source}");
					Console.WriteLine($"Target: {target}");
					numberOfFailures++;
				}
				else
				{
					Console.WriteLine($"PASSED ({i + 1})");
				}
			}

			Console.WriteLine("=== Test Case Finder Complete ===");
			Console.WriteLine($"Number of Failures: {numberOfFailures}");
		}

		static unsafe string GetRandomWord()
		{
			var builder = new StringBuilder(WORD_LENGTH);
			for (var i = 0; i < WORD_LENGTH; i++)
			{
				var characterIndex = Random.Next(0, CHARACTERS.Length);
				builder.Append(CHARACTERS[characterIndex]);
			}
			return builder.ToString();
		}
	}
}
