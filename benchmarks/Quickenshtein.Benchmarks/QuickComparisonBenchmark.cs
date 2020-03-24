using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks
{
	[Config(typeof(BaseRuntimeQuickConfig))]
	public class QuickComparisonBenchmark
	{
		[Params(0, 10, 400, 8000)]
		public int NumberOfCharacters;

		public string StringA;
		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("aabcdecbaabcadbab", NumberOfCharacters);
			StringB = Utilities.BuildString("babdacbaabcedcbaa", NumberOfCharacters);
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
