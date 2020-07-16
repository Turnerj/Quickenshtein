using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Comparisons.StringScenarios
{
	/// <summary>
	/// This benchmark shows the impact of various, though relatively short, string lengths.
	/// The calculation is forced to compute the worst possible case due to no matching characters.
	/// </summary>
	[Config(typeof(BaseRuntimeConfig))]
	public class TextLengthBenchmark
	{
		[Params(5, 20, 40)]
		public int NumberOfCharacters;

		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("abcdef", NumberOfCharacters);
			StringB = Utilities.BuildString("zyxwvu", NumberOfCharacters);
		}

		[Benchmark(Baseline = true)]
		public int Baseline()
		{
			return LevenshteinBaseline.GetDistance(StringA, StringB);
		}

		[Benchmark]
		public int Quickenshtein()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB);
		}

		[Benchmark]
		public int Fastenshtein()
		{
			return global::Fastenshtein.Levenshtein.Distance(StringA, StringB);
		}
	}
}
