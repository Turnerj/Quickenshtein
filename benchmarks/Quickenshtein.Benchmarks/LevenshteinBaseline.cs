using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quickenshtein.Benchmarks
{
	public static class LevenshteinBaseline
	{
		public static int GetDistance(string source, string target)
		{
			var costMatrix = Enumerable
			  .Range(0, source.Length + 1)
			  .Select(line => new int[target.Length + 1])
			  .ToArray();

			for (var rowIndex = 1; rowIndex <= source.Length; rowIndex++)
			{
				costMatrix[rowIndex][0] = rowIndex;
			}

			for (var columnIndex = 1; columnIndex <= target.Length; columnIndex++)
			{
				costMatrix[0][columnIndex] = columnIndex;
			}

			for (var rowIndex = 1; rowIndex <= source.Length; rowIndex++)
			{
				for (var columnIndex = 1; columnIndex <= target.Length; columnIndex++)
				{
					var insertion = costMatrix[rowIndex][columnIndex - 1] + 1;
					var deletion = costMatrix[rowIndex - 1][columnIndex] + 1;
					var substitution = costMatrix[rowIndex - 1][columnIndex - 1] + (source[rowIndex - 1] == target[columnIndex - 1] ? 0 : 1);

					costMatrix[rowIndex][columnIndex] = Math.Min(Math.Min(insertion, deletion), substitution);
				}
			}

			return costMatrix[source.Length][target.Length];
		}
	}
}
