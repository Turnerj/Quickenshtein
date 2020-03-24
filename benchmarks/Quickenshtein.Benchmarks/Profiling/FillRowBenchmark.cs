#if NETCOREAPP
using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;
using System.Collections.Generic;

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
		public void FillRow()
		{
			Levenshtein.FillRow(Data);
		}

		[Benchmark]
		public void FillRow_Sse2()
		{
			Levenshtein.FillRow_Sse2(Data);
		}

		[Benchmark]
		public void FillRow_Avx2()
		{
			Levenshtein.FillRow_Avx2(Data);
		}
	}
}
#endif