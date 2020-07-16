using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Tuning
{
	[Config(typeof(CustomConfig))]
	public class FrameworkTuningBenchmark
	{
		private class CustomConfig : CustomIntrinsicConfig
		{
			public CustomConfig()
			{
				AddFramework();

				AddHardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.CacheMisses);
			}
		}

		[Params(10, 400, 8000)]
		public int NumberOfCharacters;

		public string StringA;

		public string StringB;

		[GlobalSetup]
		public void Setup()
		{
			StringA = Utilities.BuildString("aabcdecbaabcadbab", NumberOfCharacters);
			StringB = Utilities.BuildString("babdacbaabcedcbaa", NumberOfCharacters);
		}

		[Benchmark]
		public int Quickenshtein()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB, CalculationOptions.Default);
		}

		[Benchmark]
		public int Quickenshtein_Threaded()
		{
			return global::Quickenshtein.Levenshtein.GetDistance(StringA, StringB, CalculationOptions.DefaultWithThreading);
		}
	}
}
