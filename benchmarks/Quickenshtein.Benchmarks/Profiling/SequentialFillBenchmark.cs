using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using Quickenshtein.Internal;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(FullRuntimeConfig))]
	public class SequentialFillBenchmark
	{
		[Params(10, 300, 8102)]
		public int RowSize;

		public int[] Data;

		[GlobalSetup]
		public void Setup()
		{
			Data = new int[RowSize];
		}

		[Benchmark]
		public unsafe void Fill()
		{
			fixed (int* data = Data)
			{
				DataHelper.SequentialFill(data, 1, RowSize);
			}
		}
	}
}