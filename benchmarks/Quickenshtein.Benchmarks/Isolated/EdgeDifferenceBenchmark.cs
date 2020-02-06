using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.Benchmarks.Isolated
{
	/// <summary>
	/// This benchmark eliminates bonuses for trimming equal characters at the start and end.
	/// The strings the same length and, besides the edges, are only filled with matching characters.
	/// This then forces the fastest path internally (least operations) for calculating distance.
    /// </summary>
	[MemoryDiagnoser]
	[SimpleJob(RuntimeMoniker.NetCoreApp30)]
	[SimpleJob(RuntimeMoniker.Net472)]
	public class EdgeDifferenceBenchmark
	{
		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = $"a{Utilities.BuildString("b", 256 + 7)}c";
			StringB = $"y{Utilities.BuildString("b", 256 + 7)}z";
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
