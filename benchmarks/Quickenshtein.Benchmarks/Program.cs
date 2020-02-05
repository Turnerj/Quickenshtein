using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using static BenchmarkDotNet.Reports.SummaryTable;

namespace Quickenshtein.Benchmarks
{
	class Program
	{
		class RatioReport
		{
			public string Workload;
			public decimal Ratio;
		}

		static void Main(string[] args)
		{
			//var benchmark = new QuickenshteinBenchmark();
			//benchmark.Setup();
			//benchmark.Quickenshtein();
			//return;

			var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

			//Generate an average speed up report
			foreach (var summary in summaries)
			{
				if (summary.HasCriticalValidationErrors)
				{
					Console.WriteLine("Average Speed Up is unavailable");
					Console.WriteLine();
					continue;
				}

				SummaryTableColumn ratioColumn = null;
				for (var i = 0; i < summary.Table.ColumnCount; i++)
				{
					var column = summary.Table.Columns[i];
					if (column.Header == "Ratio")
					{
						ratioColumn = column;
						break;
					}
				}

				if (ratioColumn != null)
				{
					var ratioColumnContent = ratioColumn.Content;
					var workloadRatios = new List<RatioReport>();
					for (var i = 0; i < summary.Reports.Length; i++)
					{
						var report = summary.Reports[i];
						if (decimal.TryParse(ratioColumnContent[i], out var ratio))
						{
							workloadRatios.Add(new RatioReport
							{
								Ratio = ratio,
								Workload = report.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo
							});
						}
					}

					Console.WriteLine("Average Speed Up");
					foreach (var group in workloadRatios.GroupBy(r => r.Workload))
					{
						var averageRatio = group.Sum(r => r.Ratio) / group.Count();
						var averageSpeedUp = 1 / averageRatio;
						Console.WriteLine($"{group.Key}: {averageSpeedUp:0.000}");
					}

					Console.WriteLine();
				}
			}
		}
	}
}
