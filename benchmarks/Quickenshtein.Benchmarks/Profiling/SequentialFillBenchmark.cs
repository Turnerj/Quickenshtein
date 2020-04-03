#if NETCOREAPP
using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using Quickenshtein.Internal;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(CoreOnlyRuntimeConfig))]
	public class SequentialFillBenchmark
	{
		[Params(10, 400, 8000)]
		public int RowSize;

		public int[] Data;

		[GlobalSetup]
		public void Setup()
		{
			Data = new int[RowSize];
		}

		[Benchmark(Baseline = true)]
		public void Baseline()
		{
			for (int i = 0, l = Data.Length; i < l;)
			{
				Data[i] = ++i;
			}
		}

		[Benchmark]
		public unsafe void Fill()
		{
			fixed (int* data = Data)
			{
				SequentialFillHelper.Fill(data, RowSize);
			}
		}

		[Benchmark]
		public unsafe void Fill_Sse2()
		{
			fixed (int* data = Data)
			{
				SequentialFillHelper.Fill_Sse2(data, RowSize);
			}
		}

		[Benchmark]
		public unsafe void Fill_Avx2()
		{
			fixed (int* data = Data)
			{
				SequentialFillHelper.Fill_Avx2(data, RowSize);
			}
		}
	}
}
#endif