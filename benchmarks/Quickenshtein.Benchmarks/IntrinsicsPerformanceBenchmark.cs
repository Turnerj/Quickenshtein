using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks
{
	[Config(typeof(FullRuntimeConfig))]
	public class IntrinsicsPerformanceBenchmark
	{
		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("aabcdecbaabcadbab", 8000);
			StringB = Utilities.BuildString("babdacbaabcedcbaa", 8000);
		}

		[Benchmark]
		public int Quickenshtein()
		{
			return Levenshtein.GetDistance(StringA, StringB);
		}
	}
}
