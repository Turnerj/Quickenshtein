using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Quickenshtein.Benchmarks.Columns;

namespace Quickenshtein.Benchmarks.Config
{
	abstract class CustomIntrinsicConfig : ManualConfig
	{
		private const string ENV_ENABLE_HWINTRINSICS = "COMPlus_EnableHWIntrinsic";
		private const string ENV_ENABLE_AVX2 = "COMPlus_EnableAVX2";
		private const string ENV_ENABLE_SSE41 = "COMPlus_EnableSSE41";

		public CustomIntrinsicConfig() : base()
		{
			AddDiagnoser(MemoryDiagnoser.Default);

			Orderer = new DefaultOrderer(SummaryOrderPolicy.SlowestToFastest);

			AddColumn(SpeedupRatioColumn.SpeedupOfMean);
			AddColumn(WorthinessRatioColumn.WorthinessOfMean);
			AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(maxDepth: 3)));
		}

		protected void AddFramework(bool asBaseline = false)
		{
			AddJob(Job.Default
				.WithRuntime(ClrRuntime.Net472)
				.WithId("Framework"), asBaseline);
		}

		protected void AddCoreWithoutIntrinsics(bool asBaseline = false)
		{
			AddJob(Job.Default
				.WithRuntime(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_HWINTRINSICS, "0")
				.WithId("Core (No Intrinsics)"), asBaseline);
		}

		protected void AddCoreWithoutSSE41(bool asBaseline = false)
		{
			AddJob(Job.Default
				.WithRuntime(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_SSE41, "0")
				.WithId("Core (w/o SSE41)"), asBaseline);
		}

		protected void AddCoreWithoutAVX2(bool asBaseline = false)
		{
			AddJob(Job.Default
				.WithRuntime(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_AVX2, "0")
				.WithId("Core (w/o AVX2)"), asBaseline);
		}

		protected void AddCore(bool asBaseline = false)
		{
			AddJob(Job.Default
				.WithRuntime(CoreRuntime.Core30)
				.WithId("Core (All Intrinsics)"), asBaseline);
		}

		private void AddJob(Job job, bool asBaseline)
		{
			if (asBaseline)
			{
				job.AsBaseline();
			}

			AddJob(job);
		}
	}
}
