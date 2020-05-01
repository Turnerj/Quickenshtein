using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks
{
	[Config(typeof(CoreOnlyRuntimeConfig))]
	public class BasicBenchmark
	{
		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("aabcdecbaabcadbab", 8000);
			StringB = Utilities.BuildString("babdacbaabcedcbaa", 8000);
		}

		//[Benchmark(Baseline = true)]
		//public int Baseline()
		//{
		//	return LevenshteinBaseline.GetDistance(StringA, StringB);
		//}

		[Benchmark]
		public int Quickenshtein()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB);
		}

		//[Benchmark]
		//public int Fastenshtein()
		//{
		//	return global::Fastenshtein.Levenshtein.Distance(StringA, StringB);
		//}
	}
}
