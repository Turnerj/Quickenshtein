using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Comparisons.StringScenarios
{
	/// <summary>
	/// This benchmark shows the impact of extremely long string lengths.
	/// The calculation is forced to compute the worst possible case due to no matching characters.
	/// The baseline calculator however can't perform calculations this large so it is omitted.
	/// </summary>
	[Config(typeof(BaseRuntimeConfig))]
	public class HugeTextBenchmark
	{
		[Params(8192, 32768)]
		public int NumberOfCharacters;

		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("abcdef", NumberOfCharacters);
			StringB = Utilities.BuildString("zyxwvu", NumberOfCharacters);
		}

		[Benchmark]
		public int Quickenshtein_NoThreading()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB);
		}

		[Benchmark]
		public int Quickenshtein_WithThreading()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB, new CalculationOptions 
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = 16
			});
		}

		[Benchmark]
		public int Fastenshtein()
		{
			return global::Fastenshtein.Levenshtein.Distance(StringA, StringB);
		}
	}
}
