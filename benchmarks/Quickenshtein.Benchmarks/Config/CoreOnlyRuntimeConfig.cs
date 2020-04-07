using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Quickenshtein.Benchmarks.Columns;

namespace Quickenshtein.Benchmarks.Config
{
	class CoreOnlyRuntimeConfig : ManualConfig
	{
		public CoreOnlyRuntimeConfig()
		{
			AddDiagnoser(MemoryDiagnoser.Default);

			AddJob(Job.Default
				.WithRuntime(CoreRuntime.Core30));

			AddColumn(SpeedupRatioColumn.SpeedupOfMean);
		}
	}
}
