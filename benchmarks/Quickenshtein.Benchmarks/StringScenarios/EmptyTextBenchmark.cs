using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.StringScenarios
{
	/// <summary>
	/// This benchmark shows how well the calculator can optimise an empty string in either the source or target.
	/// </summary>
	[Config(typeof(BaseRuntimeConfig))]
	public class EmptyTextBenchmark
	{
		[ParamsAllValues]
		public bool IsSourceEmpty;

		public string Source;

		public string Target;

		[GlobalSetup]
		public void Setup()
		{
			if (IsSourceEmpty)
			{
				Source = string.Empty;
				Target = Utilities.BuildString("abcdefghij", 400);
			}
			else
			{
				Target = string.Empty;
				Source = Utilities.BuildString("abcdefghij", 400);
			}
		}

		[Benchmark]
		public int Quickenshtein()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(Source, Target);
		}

		[Benchmark]
		public int Fastenshtein()
		{
			return global::Fastenshtein.Levenshtein.Distance(Source, Target);
		}
	}
}
