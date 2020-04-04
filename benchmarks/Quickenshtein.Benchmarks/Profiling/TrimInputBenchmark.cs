using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using System;
using System.Collections.Generic;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(CustomConfig))]
	public class TrimInputBenchmark
	{
		class CustomConfig : CustomIntrinsicConfig
		{
			public CustomConfig()
			{
				AddFramework(true);
				AddCoreWithoutIntrinsics();
				AddCoreWithoutAVX2();
				AddCore();
			}
		}

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
			Levenshtein.TrimInput(TestString.AsSpan(), TestString.AsSpan(), ref startIndex, ref sourceEnd, ref targetEnd);
		}
	}
}