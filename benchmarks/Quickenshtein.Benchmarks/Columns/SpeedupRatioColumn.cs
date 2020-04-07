using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Quickenshtein.Benchmarks.Columns
{
	/// <summary>
	/// From: https://github.com/damageboy/VxSort/blob/research/Bench/Utils/SpeedupRatioColumn.cs
	/// </summary>
	public class SpeedupRatioColumn : BaselineCustomColumn
	{
		public enum RatioMetric
		{
			Mean,
			StdDev
		}

		public static readonly IColumn SpeedupOfMean = new SpeedupRatioColumn(RatioMetric.Mean);
		public static readonly IColumn SpeedupStdDev = new SpeedupRatioColumn(RatioMetric.StdDev);

		public RatioMetric Metric { get; }

		private SpeedupRatioColumn(RatioMetric metric)
		{
			Metric = metric;
		}

		public override string Id => nameof(SpeedupRatioColumn) + "." + Metric;

		public override string ColumnName
		{
			get
			{
				switch (Metric)
				{
					case RatioMetric.Mean:
						return "Speedup";
					case RatioMetric.StdDev:
						return "SpeedupSD";
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

			switch (Metric)
			{
				case RatioMetric.Mean:
					return IsNonBaselinesPrecise(summary, baseline, benchmarkCase) ? ratio.Mean.ToString("N3") : ratio.Mean.ToString("N2");
				case RatioMetric.StdDev:
					return ratio.StandardDeviation.ToString("N2");
				default:
					throw new NotSupportedException();
			}
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
