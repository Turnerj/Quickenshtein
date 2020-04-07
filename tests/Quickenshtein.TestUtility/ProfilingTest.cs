using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.TestUtility
{
	class ProfilingTest : IUtility
	{
		private const int ITERATIONS = 2000;
		private const int WORD_LENGTH = 400;

		public void Run()
		{
			var source = WordGenerator.GenerateWord(WORD_LENGTH);
			var target = WordGenerator.GenerateWord(WORD_LENGTH);

			Console.WriteLine($"Source Word: {source}");
			Console.WriteLine($"Target Word: {target}");

			for (var i = 0; i < ITERATIONS; i++)
			{
				Levenshtein.GetDistance(source, target);
			}

			Console.WriteLine("Done!");
		}
	}
}
