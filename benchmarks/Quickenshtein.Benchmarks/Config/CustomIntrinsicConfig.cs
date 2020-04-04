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
			Add(MemoryDiagnoser.Default);

			Orderer = new DefaultOrderer(SummaryOrderPolicy.SlowestToFastest);

			Add(SpeedupRatioColumn.SpeedupOfMean);

			//Requires version of BDN > 0.12.0
			//Add(WorthinessRatioColumn.WorthinessOfMean);
			//Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig()));
		}

		protected void AddFramework(bool asBaseline = false)
		{
			AddJob(Job.Default
				.With(ClrRuntime.Net472)
				.WithId("Framework"), asBaseline);
		}

		protected void AddCoreWithoutIntrinsics(bool asBaseline = false)
		{
			AddJob(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_HWINTRINSICS, "0")
				.WithId("Core (No Intrinsics)"), asBaseline);
		}

		protected void AddCoreWithoutSSE41(bool asBaseline = false)
		{
			AddJob(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_SSE41, "0")
				.WithId("Core (w/o SSE41)"), asBaseline);
		}

		protected void AddCoreWithoutAVX2(bool asBaseline = false)
		{
			AddJob(Job.Default
				.With(CoreRuntime.Core30)
				.WithEnvironmentVariable(ENV_ENABLE_AVX2, "0")
				.WithId("Core (w/o AVX2)"), asBaseline);
		}

		protected void AddCore(bool asBaseline = false)
		{
			AddJob(Job.Default
				.With(CoreRuntime.Core30)
				.WithId("Core (All Intrinsics)"), asBaseline);
		}

		private void AddJob(Job job, bool asBaseline)
		{
			if (asBaseline)
			{
				job.AsBaseline();
			}

			Add(job);
		}
	}
}
