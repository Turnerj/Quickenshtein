using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Quickenshtein.Benchmarks.Config;

namespace Quickenshtein.Benchmarks.Profiling
{
	[Config(typeof(CustomConfig))]
	public class FrameworkCalculateRowBenchmark
	{
		class CustomConfig : CustomIntrinsicConfig
		{
			public CustomConfig() : base()
			{
				AddFramework();

				AddHardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.CacheMisses);
			}
		}

		[Params(10, 400, 8000)]
		public int NumberOfCharacters;

		[ParamsAllValues]
		public bool TargetCharMatchesSourceChar;

		public int[] PreviousRow;

		public int LastInsertionCost = 1;
		public int LastSubstitutionCost = 0;
		public char SourcePrevChar = 'a';

		public string TargetString;

		[GlobalSetup]
		public void Setup()
		{
			PreviousRow = new int[NumberOfCharacters];

			if (TargetCharMatchesSourceChar)
			{
				TargetString = Utilities.BuildString("aaaaaaaaaa", NumberOfCharacters);
			}
			else
			{
				TargetString = Utilities.BuildString("bbbbbbbbbb", NumberOfCharacters);
			}
		}

		[Benchmark(Baseline = true)]
		public unsafe void Baseline()
		{
			int lastDeletionCost;
			var lastInsertionCost = 1;
			var lastSubstitutionCost = 0;
			for (var columnIndex = 0; columnIndex < NumberOfCharacters; columnIndex++)
			{
				lastDeletionCost = PreviousRow[columnIndex];
				var insertOrDelete = Math.Min(lastInsertionCost, lastDeletionCost) + 1;
				var edit = lastSubstitutionCost + (SourcePrevChar == TargetString[columnIndex] ? 0 : 1);

				lastInsertionCost = Math.Min(insertOrDelete, edit);
				lastSubstitutionCost = lastDeletionCost;
				PreviousRow[columnIndex] = lastInsertionCost;
			}
		}

		[Benchmark]
		public unsafe void CalculateRow()
		{
			fixed (int* previousRowPtr = PreviousRow)
			fixed (char* targetPtr = TargetString)
			{
				Levenshtein.CalculateRow(previousRowPtr, targetPtr, NumberOfCharacters, SourcePrevChar, LastInsertionCost, LastSubstitutionCost);
			}
		}
	}
}