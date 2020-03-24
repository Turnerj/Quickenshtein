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
			Add(MemoryDiagnoser.Default);
			
			Add(Job.ShortRun
				.With(ClrRuntime.Net472)
				.AsBaseline());

			Add(Job.ShortRun
				.With(CoreRuntime.Core30));

			Add(SpeedupRatioColumn.SpeedupOfMean);

			//Requires version of BDN > 0.12.0
			//Add(WorthinessRatioColumn.WorthinessOfMean);
			//Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig()));
		}
	}
}
