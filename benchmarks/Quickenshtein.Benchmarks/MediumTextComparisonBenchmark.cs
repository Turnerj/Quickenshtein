using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(RuntimeMoniker.NetCoreApp30)]
	[SimpleJob(RuntimeMoniker.Net472)]
	public class MediumTextComparisonBenchmark
	{
		[ParamsSource(nameof(GetComparisonStrings))]
		public string StringA;

		[ParamsSource(nameof(GetComparisonStrings))]
		public string StringB;

		public static IEnumerable<string> GetComparisonStrings()
		{
			yield return string.Empty;
			yield return Utilities.BuildString("aababbadebaaebebb", 400);
			yield return Utilities.BuildString("bbebeaabedabbabaa", 400);
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
