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
		class SpeedupReport
		{
			public string Workload;
			public decimal Speedup;
		}

		static void Main(string[] args)
		{
			var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

			//Generate an average speedup report
			foreach (var summary in summaries)
			{
				if (summary.HasCriticalValidationErrors)
				{
					Console.WriteLine("Average Speedup is unavailable");
					Console.WriteLine();
					continue;
				}

				SummaryTableColumn speedUpColumn = null;
				for (var i = 0; i < summary.Table.ColumnCount; i++)
				{
					var column = summary.Table.Columns[i];
					if (column.Header == "Speedup")
					{
						speedUpColumn = column;
						break;
					}
				}

				if (speedUpColumn != null)
				{
					var speedupColumnContent = speedUpColumn.Content;
					var workloadRatios = new List<SpeedupReport>();
					for (var i = 0; i < summary.Reports.Length; i++)
					{
						var report = summary.Reports[i];
						if (decimal.TryParse(speedupColumnContent[i], out var speedup))
						{
							workloadRatios.Add(new SpeedupReport
							{
								Speedup = speedup,
								Workload = report.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo
							});
						}
					}

					Console.WriteLine("Average Speedup");
					foreach (var group in workloadRatios.GroupBy(r => r.Workload))
					{
						var averageSpeedup = group.Average(r => r.Speedup);
						Console.Write($"{group.Key}: ".PadRight(16));
						Console.WriteLine(averageSpeedup.ToString("0.00").PadLeft(7));
					}

					Console.WriteLine();
				}
			}
		}
	}
}
