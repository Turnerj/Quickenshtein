#if NETCOREAPP
using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using System.Collections.Generic;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(CoreOnlyRuntimeConfig))]
	public class TrimInputBenchmark
	{
		[ParamsSource(nameof(GetComparisonStrings))]
		public string TestString;

		public static IEnumerable<string> GetComparisonStrings()
		{
			yield return string.Empty;
			yield return "abcdefghij";
			yield return Utilities.BuildString("abcdefghij", 400);
			yield return Utilities.BuildString("abcdefghij", 8000);
		}

		[Benchmark]
		public void TrimInput()
		{
			int startIndex = 0, sourceEnd = TestString.Length, targetEnd = sourceEnd;
			Levenshtein.TrimInput(TestString, TestString, ref startIndex, ref sourceEnd, ref targetEnd);
		}

		[Benchmark]
		public void TrimInput_Avx2()
		{
			int startIndex = 0, sourceEnd = TestString.Length, targetEnd = sourceEnd;
			Levenshtein.TrimInput_Avx2(TestString, TestString, ref startIndex, ref sourceEnd, ref targetEnd);
		}
	}
}
#endif