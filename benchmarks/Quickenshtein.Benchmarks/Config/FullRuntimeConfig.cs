using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Quickenshtein.Benchmarks.Columns;

namespace Quickenshtein.Benchmarks.Config
{
	class FullRuntimeConfig : ManualConfig
	{
		private const string ENV_ENABLE_HWINTRINSICS = "COMPlus_EnableHWIntrinsic";
		private const string ENV_ENABLE_AVX2 = "COMPlus_EnableAVX2";
		private const string ENV_ENABLE_SSE41 = "COMPlus_EnableSSE41";

		public FullRuntimeConfig() : base()
		{
			Add(MemoryDiagnoser.Default);

			Orderer = new DefaultOrderer(SummaryOrderPolicy.SlowestToFastest);

			Add(Job.Default
				.With(ClrRuntime.Net472)
				.WithId("Framework")
				.AsBaseline());

			Add(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_HWINTRINSICS, "0")
				.WithId("Core (No Intrinsics)"));

			Add(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_SSE41, "0")
				.WithId("Core (<= SSE2)"));

			Add(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_AVX2, "0")
				.WithId("Core (<= SSE41)"));

			Add(Job.Default
				.With(CoreRuntime.Core30)
				.WithId("Core (All Intrinsics)"));

			Add(SpeedupRatioColumn.SpeedupOfMean);

			//Requires version of BDN > 0.12.0
			//Add(WorthinessRatioColumn.WorthinessOfMean);
			//Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig()));
		}
	}
}
