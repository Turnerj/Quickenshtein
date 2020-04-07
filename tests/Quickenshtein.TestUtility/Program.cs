using System;
using System.Text;

namespace Quickenshtein.TestUtility
{
	class Program
	{
		private static IUtility[] Utilities = new IUtility[]
		{
			new TestCaseFinder(),
			new ProfilingTest()
		};

		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("Please select the utility you want to run (eg. '1'):");

				for (var i = 0; i < Utilities.Length; i++)
				{
					Console.WriteLine($"{i + 1}. {Utilities[i].GetType().Name}");
				}

				Console.Write("Your pick: ");

				var rawSelection = Console.ReadLine();
				if (int.TryParse(rawSelection, out var value) && value > 0 && value <= Utilities.Length)
				{
					var utility = Utilities[value - 1];
					Console.Clear();
					Console.WriteLine($"Running {utility.GetType().Name}...");
					utility.Run();
					break;
				}
				else
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid Selection");
					Console.ResetColor();
				}
			}
		}
	}
}
