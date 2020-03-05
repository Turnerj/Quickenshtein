using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein.Benchmarks
{
	[MemoryDiagnoser]
	[Config(typeof(OptionalHWIntrinsicsConfig))]
	public class IntrinsicComparisonBenchmark
	{
		private class OptionalHWIntrinsicsConfig : ManualConfig
		{
			private const string ENV_ENABLE_HWINTRINSICS = "COMPlus_EnableHWIntrinsic";
			private const string ENV_ENABLE_AVX2 = "COMPlus_EnableAVX2";
			private const string ENV_ENABLE_SSE41 = "COMPlus_EnableSSE41";

			public OptionalHWIntrinsicsConfig()
			{
				Add(Job.Default
					.With(CoreRuntime.Core30)
					.WithEnvironmentVariable(ENV_ENABLE_HWINTRINSICS, "0")
					.WithId("Intrinsics Disabled")
					.AsBaseline());

				Add(Job.Default
					.With(CoreRuntime.Core30)
					.WithEnvironmentVariable(ENV_ENABLE_SSE41, "0")
					.WithId("SSE2 Enabled"));

				Add(Job.Default
					.With(CoreRuntime.Core30)
					.WithEnvironmentVariable(ENV_ENABLE_AVX2, "0")
					.WithId("SSE41 Enabled"));

				Add(Job.Default
					.With(CoreRuntime.Core30)
					.WithId("AVX2 Enabled"));
			}
		}

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
