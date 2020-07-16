using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Comparisons
{
	/// <summary>
	/// This is the overall comparison benchmark between different implementations for different string sizes.
	/// </summary>
	[Config(typeof(BaseRuntimeQuickConfig))]
	public class QuickComparisonBenchmark
	{
		[Params(10, 400, 8000)]
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
		public int Quickenshtein_Threaded()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB, CalculationOptions.DefaultWithThreading);
		}

		[Benchmark]
		public int Fastenshtein()
		{
			return global::Fastenshtein.Levenshtein.Distance(StringA, StringB);
		}
	}
}
