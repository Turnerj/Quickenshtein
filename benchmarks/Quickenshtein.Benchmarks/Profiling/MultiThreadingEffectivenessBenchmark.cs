using BenchmarkDotNet.Attributes;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(BaseRuntimeQuickConfig))]
	public class MultiThreadingEffectivenessBenchmark
	{
		[Params(1000, 2000, 4000, 8000, 16000)]
		public int NumberOfCharacters;

		public string SourceString;
		public string TargetString;

		[GlobalSetup]
		public void Setup()
		{
			SourceString = Utilities.BuildString("aaaaaaaaaa", NumberOfCharacters);
			TargetString = Utilities.BuildString("bbbbbbbbbb", NumberOfCharacters);
		}

		[Benchmark]
		public unsafe int Baseline()
		{
			return Levenshtein.GetDistance(SourceString, TargetString, CalculationOptions.Default);
		}

		[Benchmark]
		public unsafe int TwoThreads()
		{
			return Levenshtein.GetDistance(SourceString, TargetString, new CalculationOptions
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = NumberOfCharacters / 2
			});
		}

		[Benchmark]
		public unsafe int FourThreads()
		{
			return Levenshtein.GetDistance(SourceString, TargetString, new CalculationOptions
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = NumberOfCharacters / 4
			});
		}

		[Benchmark]
		public unsafe int EightThreads()
		{
			return Levenshtein.GetDistance(SourceString, TargetString, new CalculationOptions
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = NumberOfCharacters / 8
			});
		}
	}
}