#if NETCOREAPP
using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(CoreOnlyRuntimeConfig))]
	public class FillRowBenchmark
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
		public unsafe void FillRow()
		{
			fixed (int* data = Data)
			{
				Levenshtein.FillRow(data, RowSize);
			}
		}

		[Benchmark]
		public unsafe void FillRow_Sse2()
		{
			fixed (int* data = Data)
			{
				Levenshtein.FillRow_Sse2(data, RowSize);
			}
		}

		[Benchmark]
		public unsafe void FillRow_Avx2()
		{
			fixed (int* data = Data)
			{
				Levenshtein.FillRow_Avx2(data, RowSize);
			}
		}
	}
}
#endif