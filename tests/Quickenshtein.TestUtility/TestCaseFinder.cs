using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.TestUtility
{
	public class TestCaseFinder : IUtility
	{
		private const int NUMBER_OF_CHECKS = 2000;
		private const int WORD_LENGTH = 20;

		private static readonly CalculationOptions[] OptionsToTest = new[]
		{
			CalculationOptions.Default,
			new CalculationOptions
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = 16
			}
		};

		public void Run()
		{
			var numberOfFailures = 0;
			Console.WriteLine("=== Test Case Finder ===");
			Console.WriteLine($"Number of Options to Test: {OptionsToTest.Length}");
			Console.WriteLine($"Number of Checks per Test: {NUMBER_OF_CHECKS}");
			Console.WriteLine($"World Length: {WORD_LENGTH}");
			Console.WriteLine("========================");

			for (var i = 0; i < OptionsToTest.Length; i++)
			{
				Console.WriteLine();
				Console.WriteLine("========================");
				Console.WriteLine($"======= Option {i + 1} =======");
				numberOfFailures += RunWithOptions(OptionsToTest[i]);
			}

			Console.WriteLine("=== Test Case Finder Complete ===");
			Console.WriteLine($"Total Number of Failures: {numberOfFailures}");
		}
		static int RunWithOptions(CalculationOptions calculationOptions)
		{
			Console.WriteLine("========================");
			Console.WriteLine($"EnableThreadingAfterXCharacters: {calculationOptions.EnableThreadingAfterXCharacters}");
			Console.WriteLine($"MinimumCharactersPerThread: {calculationOptions.MinimumCharactersPerThread}");
			Console.WriteLine("========================");

			var numberOfFailures = 0;

			for (var i = 0; i < NUMBER_OF_CHECKS; i++)
			{
				var source = WordGenerator.GenerateWord(WORD_LENGTH);
				var target = WordGenerator.GenerateWord(WORD_LENGTH);

				var baseline = Benchmarks.LevenshteinBaseline.GetDistance(source, target);
				var quickenshtein = Levenshtein.GetDistance(source, target);

				if (baseline != quickenshtein)
				{
					Console.WriteLine($"FAILED ({i + 1}): Expected {baseline}, Actual {quickenshtein}");
					Console.WriteLine($"Source: {source}");
					Console.WriteLine($"Target: {target}");
					numberOfFailures++;
				}
			}

			Console.WriteLine("========================");
			Console.WriteLine($"Number of Failures: {numberOfFailures}");
			Console.WriteLine("========================");
			Console.WriteLine();

			return numberOfFailures;
		}
	}
}
