using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Quickenshtein.Benchmarks.Columns;

namespace Quickenshtein.Benchmarks.Config
{
	class FullRuntimeConfig : CustomIntrinsicConfig
	{
		public FullRuntimeConfig() : base()
		{
			AddFramework(true);
			AddCoreWithoutIntrinsics();
			AddCoreWithoutSSE41();
			AddCoreWithoutAVX2();
			AddCore();
		}
	}
}
