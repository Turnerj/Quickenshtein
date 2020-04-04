using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using Quickenshtein.Internal;
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
		public unsafe void ForwardsTrim()
		{
			fixed (char* testStringPtr = TestString)
			{
				var testStringLength = TestString.Length;
				DataHelper.GetIndexOfFirstNonMatchingCharacter(testStringPtr, testStringPtr, testStringLength, testStringLength);
			}
		}

		[Benchmark]
		public unsafe void BackwardsTrim()
		{
			fixed (char* testStringPtr = TestString)
			{
				var testStringLength = TestString.Length;
				DataHelper.TrimLengthOfMatchingCharacters(testStringPtr, testStringPtr, ref testStringLength, ref testStringLength);
			}
		}
	}
}