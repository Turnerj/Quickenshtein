using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.Benchmarks
{
	/// <summary>
	/// This benchmark shows the impact of extremely long string lengths.
	/// The calculation is forced to compute the worst possible case due to no matching characters.
	/// The baseline calculator however can't perform calculations this large so it is omitted.
	/// </summary>
	[MemoryDiagnoser]
	[SimpleJob(RuntimeMoniker.NetCoreApp30)]
	[SimpleJob(RuntimeMoniker.Net472)]
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
