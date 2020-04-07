using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Quickenshtein.Benchmarks.Columns
{
	/// <summary>
	/// From: https://github.com/damageboy/VxSort/blob/research/Bench/Utils/WorthinessRatioColumn.cs
	/// </summary>
	public class WorthinessRatioColumn : BaselineCustomColumn
	{
		public enum RatioMetric
		{
			Mean,
			StdDev
		}

		public static readonly IColumn WorthinessOfMean = new WorthinessRatioColumn(RatioMetric.Mean);

		public RatioMetric Metric { get; }

		private WorthinessRatioColumn(RatioMetric metric)
		{
			Metric = metric;
		}

		public override string Id => nameof(WorthinessRatioColumn) + "." + Metric;

		public override string ColumnName
		{
			get
			{
				switch (Metric)
				{
					case RatioMetric.Mean:
						return "Worthiness";
					case RatioMetric.StdDev:
						return "WorthinessSD";
					default:
						throw new NotSupportedException();
				}
			}
		}

		public override string GetValue(Summary summary, BenchmarkCase benchmarkCase, Statistics baseline,
			IReadOnlyDictionary<string, Metric> baselineMetric, Statistics current,
			IReadOnlyDictionary<string, Metric> currentMetric, bool isBaseline)
		{
			var ratio = GetRatioStatistics(current, baseline);
			if (ratio == null)
				return "NA";

			if (!currentMetric.ContainsKey("Native Code Size") || !baselineMetric.ContainsKey("Native Code Size"))
			{
				return "NA";
			}

			var codeSizeRatio = GetCodeSizeRatio(currentMetric, baselineMetric);

			switch (Metric)
			{
				case RatioMetric.Mean:
					return IsNonBaselinesPrecise(summary, baseline, benchmarkCase) ?
						(ratio.Mean / codeSizeRatio).ToString("N3") :
						(ratio.Mean / codeSizeRatio).ToString("N2");
				default:
					throw new NotSupportedException();
			}
		}

		double GetCodeSizeRatio(IReadOnlyDictionary<string, Metric> currentMetric, IReadOnlyDictionary<string, Metric> baselineMetric)
		{
			var currentCodeSize = currentMetric["Native Code Size"].Value;
			var baselineCodeSize = baselineMetric["Native Code Size"].Value;
			return currentCodeSize / baselineCodeSize;
		}

		private static bool IsNonBaselinesPrecise(Summary summary, Statistics baselineStat, BenchmarkCase benchmarkCase)
		{
			string logicalGroupKey = summary.GetLogicalGroupKey(benchmarkCase);
			var nonBaselines = summary.GetNonBaselines(logicalGroupKey);
			return nonBaselines.Any(x => GetRatioStatistics(summary[x].ResultStatistics, baselineStat)?.Mean < 0.01);
		}

		private static Statistics GetRatioStatistics(Statistics current, Statistics baseline)
		{
			if (current == null || current.N < 1)
				return null;
			if (baseline == null || baseline.N < 1)
				return null;
			try
			{
				return Statistics.Divide(baseline, current);
			}
			catch (DivideByZeroException)
			{
				return null;
			}
		}

		public override int PriorityInCategory => (int)Metric;
		public override bool IsNumeric => true;
		public override UnitType UnitType => UnitType.Dimensionless;

		public override string Legend
		{
			get
			{
				switch (Metric)
				{
					case RatioMetric.Mean:
						return "Mean of the ratio distribution ([Current]/[Baseline])";
					case RatioMetric.StdDev:
						return "Standard deviation of the ratio distribution ([Current]/[Baseline])";
					default:
						throw new ArgumentOutOfRangeException(nameof(Metric));
				}
			}
		}
	}
}
