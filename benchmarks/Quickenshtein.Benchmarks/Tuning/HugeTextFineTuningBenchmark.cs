using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Tuning
{
	[Config(typeof(CustomConfig))]
	public class HugeTextFineTuningBenchmark
	{
		private class CustomConfig : CustomIntrinsicConfig
		{
			public CustomConfig()
			{
				AddFramework();
				AddCore();

				AddHardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.CacheMisses);
			}
		}

		[Params(16000, 32000)]
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
