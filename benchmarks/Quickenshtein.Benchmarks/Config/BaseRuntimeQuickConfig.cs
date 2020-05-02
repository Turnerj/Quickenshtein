using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Quickenshtein.Benchmarks.Columns;

namespace Quickenshtein.Benchmarks.Config
{
	class BaseRuntimeQuickConfig : ManualConfig
	{
		public BaseRuntimeQuickConfig()
		{
			AddDiagnoser(MemoryDiagnoser.Default);

			AddJob(Job.ShortRun
				.WithRuntime(ClrRuntime.Net472)
				.AsBaseline());

			AddJob(Job.ShortRun
				.WithRuntime(CoreRuntime.Core30));

			AddColumn(SpeedupRatioColumn.SpeedupOfMean);
			AddColumn(WorthinessRatioColumn.WorthinessOfMean);
			AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig(maxDepth: 3)));
		}
	}
}
