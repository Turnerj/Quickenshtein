using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.StringScenarios
{
	/// <summary>
	/// This benchmark shows how well the calculator can optimise matching characters at the start and end.
	/// </summary>
	[Config(typeof(BaseRuntimeConfig))]
	public class EdgeMatchBenchmark
	{
		[Params(40, 8192)]
		public int NumberOfCharacters;

		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			var buildStringLength = NumberOfCharacters / 2;
			var buildString = Utilities.BuildString("aaaaaaaaaa", buildStringLength - 1);
			StringA = buildString + "bb" + buildString;
			StringB = buildString + "cc" + buildString;
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
