using System;
using System.Runtime.CompilerServices;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		/// <summary>
		/// Calculates the trim offsets at the start and end of the source and target spans where characters are equal.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="startIndex"></param>
		/// <param name="sourceEnd"></param>
		/// <param name="targetEnd"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void TrimInput(ReadOnlySpan<char> source, ReadOnlySpan<char> target, ref int startIndex, ref int sourceEnd, ref int targetEnd)
		{
			startIndex = 0;

			var charactersAvailableToTrim = Math.Min(sourceEnd, targetEnd);

			while (charactersAvailableToTrim > 0 && source[startIndex] == target[startIndex])
			{
				charactersAvailableToTrim--;
				startIndex++;
			}

			while (charactersAvailableToTrim > 0 && source[sourceEnd - 1] == target[targetEnd - 1])
			{
				charactersAvailableToTrim--;
				sourceEnd--;
				targetEnd--;
			}
		}

		/// <summary>
		/// Fills <paramref name="previousRow"/> with a number sequence from 1 to the length of the row.
		/// </summary>
		/// <param name="previousRow"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void FillRow(Span<int> previousRow)
		{
			var columnIndex = 0;
			var columnsRemaining = previousRow.Length;

			while (columnsRemaining >= 8)
			{
				columnsRemaining -= 8;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
			}

			if (columnsRemaining > 4)
			{
				columnsRemaining -= 4;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
				previousRow[columnIndex] = ++columnIndex;
			}

			while (columnsRemaining > 0)
			{
				columnsRemaining--;
				previousRow[columnIndex] = ++columnIndex;
			}
		}

		/// <summary>
		/// Calculates the costs for an entire row of the virtual matrix.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		/// <param name="sourcePrevChar"></param>
		/// <param name="lastInsertionCost"></param>
		/// <param name="lastSubstitutionCost"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateRow(int* previousRowPtr, char* targetPtr, int targetLength, char sourcePrevChar, int lastInsertionCost, int lastSubstitutionCost)
		{
			var columnIndex = 0;
			int lastDeletionCost;
			int localCost;

			var rowColumnsRemaining = targetLength;

			//Levenshtein Distance inner loop unrolling inspired by CoreLib SpanHelpers
			//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
			while (rowColumnsRemaining >= 8)
			{
				rowColumnsRemaining -= 8;

				//Note: Code is inlined manually due to performance degradation on .NET Framework

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;
			}

			if (rowColumnsRemaining > 4)
			{
				rowColumnsRemaining -= 4;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;
			}

			while (rowColumnsRemaining > 0)
			{
				rowColumnsRemaining--;

				localCost = lastSubstitutionCost;
				lastDeletionCost = previousRowPtr[columnIndex];
				if (sourcePrevChar != targetPtr[columnIndex])
				{
					localCost = Math.Min(lastInsertionCost, localCost);
					localCost = Math.Min(lastDeletionCost, localCost);
					localCost++;
				}
				lastInsertionCost = localCost;
				previousRowPtr[columnIndex++] = localCost;
				lastSubstitutionCost = lastDeletionCost;
			}
		}

		/// <summary>
		/// Calculates the costs for the virtual matrix.
		/// This performs a 4x outer loop unrolling allowing fewer lookups of target character and deletion cost data across the rows.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="source"></param>
		/// <param name="rowIndex"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		private static unsafe void CalculateRows_4Rows(int* previousRowPtr, ReadOnlySpan<char> source, ref int rowIndex, char* targetPtr, int targetLength)
		{
			var acceptableRowCount = source.Length - 3;

			int row1Costs, row2Costs, row3Costs, row4Costs, row5Costs;
			char sourceChar1, sourceChar2, sourceChar3, sourceChar4;

			for (; rowIndex < acceptableRowCount;)
			{
				sourceChar1 = source[row1Costs = rowIndex]; //Sub
				sourceChar2 = source[row2Costs = rowIndex + 1]; //Insert, Sub
				sourceChar3 = source[row3Costs = rowIndex + 2]; //Insert, Sub
				sourceChar4 = source[row4Costs = rowIndex + 3]; //Insert, Sub
				row5Costs = rowIndex += 4; //Insert
				
				var columnIndex = 0;
				var rowColumnsRemaining = targetLength;

				while (rowColumnsRemaining >= 8)
				{
					rowColumnsRemaining -= 8;
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
				}

				if (rowColumnsRemaining >= 4)
				{
					rowColumnsRemaining -= 4;
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
				}

				while (rowColumnsRemaining > 0)
				{
					rowColumnsRemaining--;
					CalculateColumn_4Rows(targetPtr, previousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, sourceChar1, sourceChar2, sourceChar3, sourceChar4, ref columnIndex);
				}
			}
		}

		/// <summary>
		/// Calculates the cost for 4 vertically adjacent cells in the virtual matrix.
		/// Comparing 4 vertically adjacent cells prevents 3 target character lookups, 3 deletion cost lookups and 3 saves of the deletion cost.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="previousRowPtr"></param>
		/// <param name="row1Costs"></param>
		/// <param name="row2Costs"></param>
		/// <param name="row3Costs"></param>
		/// <param name="row4Costs"></param>
		/// <param name="row5Costs"></param>
		/// <param name="sourceChar1"></param>
		/// <param name="sourceChar2"></param>
		/// <param name="sourceChar3"></param>
		/// <param name="sourceChar4"></param>
		/// <param name="columnIndex"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_4Rows(char* targetPtr, int* previousRowPtr, ref int row1Costs, ref int row2Costs, ref int row3Costs, ref int row4Costs, ref int row5Costs, char sourceChar1, char sourceChar2, char sourceChar3, char sourceChar4, ref int columnIndex)
		{
			var targetChar = targetPtr[columnIndex];
			var lastDeletionCost = previousRowPtr[columnIndex];
			var localCost = row1Costs;
			if (sourceChar1 != targetChar)
			{
				localCost = Math.Min(row2Costs, localCost);
				localCost = Math.Min(lastDeletionCost, localCost);
				localCost++;
			}
			row1Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row2Costs;
			if (sourceChar2 != targetChar)
			{
				localCost = Math.Min(row3Costs, localCost);
				localCost = Math.Min(lastDeletionCost, localCost);
				localCost++;
			}
			row2Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row3Costs;
			if (sourceChar3 != targetChar)
			{
				localCost = Math.Min(row4Costs, localCost);
				localCost = Math.Min(lastDeletionCost, localCost);
				localCost++;
			}
			row3Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row4Costs;
			if (sourceChar4 != targetChar)
			{
				localCost = Math.Min(row5Costs, localCost);
				localCost = Math.Min(lastDeletionCost, localCost);
				localCost++;
			}
			row4Costs = lastDeletionCost;
			previousRowPtr[columnIndex++] = row5Costs = localCost;
		}
	}
}