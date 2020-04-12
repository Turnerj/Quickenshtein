#if NETCOREAPP
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		/// <summary>
		/// Using SSE4.1, calculates the costs for an entire row of the virtual matrix.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		/// <param name="sourcePrevChar"></param>
		/// <param name="lastInsertionCost"></param>
		/// <param name="lastSubstitutionCost"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static unsafe void CalculateRow_Sse41(int* previousRowPtr, char* targetPtr, int targetLength, char sourcePrevChar, int lastInsertionCost, int lastSubstitutionCost)
		{
			var rowColumnsRemaining = targetLength;

			var allOnesVector = Vector128.Create(1);
			var lastInsertionCostVector = Vector128.Create(lastInsertionCost);
			var lastSubstitutionCostVector = Vector128.Create(lastSubstitutionCost);
			var lastDeletionCostVector = Vector128<int>.Zero;

			//Levenshtein Distance inner loop unrolling inspired by CoreLib SpanHelpers
			//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
			while (rowColumnsRemaining >= 8)
			{
				rowColumnsRemaining -= 8;
				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);

				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}

			if (rowColumnsRemaining > 4)
			{
				rowColumnsRemaining -= 4;
				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}

			lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
			while (rowColumnsRemaining > 0)
			{
				rowColumnsRemaining--;
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}
		}

		/// <summary>
		/// Using SSE4.1, calculates the cost for an individual cell in the virtual matrix.
		/// SSE4.1 instructions allow a virtually branchless minimum value computation when the source and target characters don't match.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="targetPtr"></param>
		/// <param name="sourcePrevChar"></param>
		/// <param name="lastSubstitutionCostVector"></param>
		/// <param name="lastInsertionCostVector"></param>
		/// <param name="lastDeletionCostVector"></param>
		/// <param name="allOnesVector"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_Sse41(
			ref int* previousRowPtr,
			ref char* targetPtr,
			char sourcePrevChar,
			ref Vector128<int> lastSubstitutionCostVector,
			ref Vector128<int> lastInsertionCostVector,
			ref Vector128<int> lastDeletionCostVector,
			ref Vector128<int> allOnesVector
		)
		{
			var localCostVector = lastSubstitutionCostVector;

			if (sourcePrevChar != targetPtr[0])
			{
				localCostVector = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							lastInsertionCostVector,
							localCostVector
						),
						lastDeletionCostVector
					),
					allOnesVector
				);
			}
			lastInsertionCostVector = localCostVector;
			previousRowPtr[0] = localCostVector.GetElement(0);
			lastSubstitutionCostVector = lastDeletionCostVector;

			previousRowPtr++;
			targetPtr++;
			lastDeletionCostVector = Sse2.ShiftRightLogical128BitLane(lastDeletionCostVector, 4);
		}

		/// <summary>
		/// Using SSE4.1, calculates the costs for the virtual matrix.
		/// This performs a 4x outer loop unrolling allowing fewer lookups of target character and deletion cost data across the rows.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="sourcePtr"></param>
		/// <param name="rowIndex"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		private static unsafe void CalculateRows_4Rows_Sse41(int* previousRowPtr, char* sourcePtr, int sourceLength, ref int rowIndex, char* targetPtr, int targetLength)
		{
			var acceptableRowCount = sourceLength - 3;

			Vector128<int> row1Costs, row2Costs, row3Costs, row4Costs, row5Costs;
			char sourceChar1, sourceChar2, sourceChar3, sourceChar4;
			var allOnesVector = Vector128.Create(1);

			var lastDeletionCostVector = Vector128<int>.Zero;

			for (; rowIndex < acceptableRowCount; rowIndex += 4)
			{
				var localTargetPtr = targetPtr;
				var localPreviousRowPtr = previousRowPtr;

				sourceChar1 = sourcePtr[rowIndex];
				sourceChar2 = sourcePtr[rowIndex + 1];
				sourceChar3 = sourcePtr[rowIndex + 2];
				sourceChar4 = sourcePtr[rowIndex + 3];
				row1Costs = Vector128.Create(rowIndex); //Sub
				row2Costs = Sse2.Add(row1Costs, allOnesVector); //Insert, Sub
				row3Costs = Sse2.Add(row2Costs, allOnesVector); //Insert, Sub
				row4Costs = Sse2.Add(row3Costs, allOnesVector); //Insert, Sub
				row5Costs = Sse2.Add(row4Costs, allOnesVector); //Insert

				var rowColumnsRemaining = targetLength;

				while (rowColumnsRemaining >= 8)
				{
					rowColumnsRemaining -= 8;
					lastDeletionCostVector = Sse2.LoadVector128(localPreviousRowPtr);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);

					lastDeletionCostVector = Sse2.LoadVector128(localPreviousRowPtr);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
				}

				if (rowColumnsRemaining >= 4)
				{
					rowColumnsRemaining -= 4;
					lastDeletionCostVector = Sse2.LoadVector128(localPreviousRowPtr);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
				}

				lastDeletionCostVector = Sse2.LoadVector128(localPreviousRowPtr);
				while (rowColumnsRemaining > 0)
				{
					rowColumnsRemaining--;
					CalculateColumn_4Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref lastDeletionCostVector, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4);
				}
			}
		}

		/// <summary>
		/// Using SSE4.1, calculates the cost for 4 vertically adjacent cells in the virtual matrix.
		/// Comparing 4 vertically adjacent cells prevents 3 target character lookups, 3 deletion cost lookups and 3 saves of the deletion cost.
		/// SSE4.1 instructions allow a virtually branchless minimum value computation when the source and target characters don't match.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="previousRowPtr"></param>
		/// <param name="row1Costs"></param>
		/// <param name="row2Costs"></param>
		/// <param name="row3Costs"></param>
		/// <param name="row4Costs"></param>
		/// <param name="row5Costs"></param>
		/// <param name="allOnesVector"></param>
		/// <param name="sourceChar1"></param>
		/// <param name="sourceChar2"></param>
		/// <param name="sourceChar3"></param>
		/// <param name="sourceChar4"></param>
		/// <param name="columnIndex"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_4Rows_Sse41(
			ref char* targetPtr,
			ref int* previousRowPtr,
			ref Vector128<int> lastDeletionCostVector,
			ref Vector128<int> row1Costs,
			ref Vector128<int> row2Costs,
			ref Vector128<int> row3Costs,
			ref Vector128<int> row4Costs,
			ref Vector128<int> row5Costs,
			ref Vector128<int> allOnesVector,
			char sourceChar1,
			char sourceChar2,
			char sourceChar3,
			char sourceChar4
		)
		{
			var targetChar = targetPtr[0];
			var lastDeletionCost = lastDeletionCostVector;

			var localCost = row1Costs;
			if (sourceChar1 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row2Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row1Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row2Costs;
			if (sourceChar2 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row3Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row2Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row3Costs;
			if (sourceChar3 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row4Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row3Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row4Costs;
			if (sourceChar4 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row5Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row4Costs = lastDeletionCost;
			row5Costs = localCost;
			previousRowPtr[0] = row5Costs.GetElement(0);

			previousRowPtr++;
			targetPtr++;
			lastDeletionCostVector = Sse2.ShiftRightLogical128BitLane(lastDeletionCostVector, 4);
		}


		/// <summary>
		/// Using SSE4.1, calculates the costs for the virtual matrix.
		/// This performs a 8x outer loop unrolling allowing fewer lookups of target character and deletion cost data across the rows.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="sourcePtr"></param>
		/// <param name="rowIndex"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		private static unsafe void CalculateRows_8Rows_Sse41(int* previousRowPtr, char* sourcePtr, int sourceLength, ref int rowIndex, char* targetPtr, int targetLength)
		{
			var acceptableRowCount = sourceLength - 7;

			Vector128<int> row1Costs, row2Costs, row3Costs, row4Costs, row5Costs, row6Costs, row7Costs, row8Costs, row9Costs;
			char sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8;
			var allOnesVector = Vector128.Create(1);

			for (; rowIndex < acceptableRowCount; rowIndex += 8)
			{
				var localTargetPtr = targetPtr;
				var localPreviousRowPtr = previousRowPtr;

				sourceChar1 = sourcePtr[rowIndex];
				sourceChar2 = sourcePtr[rowIndex + 1];
				sourceChar3 = sourcePtr[rowIndex + 2];
				sourceChar4 = sourcePtr[rowIndex + 3];
				sourceChar5 = sourcePtr[rowIndex + 4];
				sourceChar6 = sourcePtr[rowIndex + 5];
				sourceChar7 = sourcePtr[rowIndex + 6];
				sourceChar8 = sourcePtr[rowIndex + 7];
				row1Costs = Vector128.Create(rowIndex); //Sub
				row2Costs = Sse2.Add(row1Costs, allOnesVector); //Insert, Sub
				row3Costs = Sse2.Add(row2Costs, allOnesVector); //Insert, Sub
				row4Costs = Sse2.Add(row3Costs, allOnesVector); //Insert, Sub
				row5Costs = Sse2.Add(row4Costs, allOnesVector); //Insert, Sub
				row6Costs = Sse2.Add(row5Costs, allOnesVector); //Insert, Sub
				row7Costs = Sse2.Add(row6Costs, allOnesVector); //Insert, Sub
				row8Costs = Sse2.Add(row7Costs, allOnesVector); //Insert, Sub
				row9Costs = Sse2.Add(row8Costs, allOnesVector); //Insert

				var rowColumnsRemaining = targetLength;

				while (rowColumnsRemaining >= 8)
				{
					rowColumnsRemaining -= 8;
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
				}

				if (rowColumnsRemaining >= 4)
				{
					rowColumnsRemaining -= 4;
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
				}

				while (rowColumnsRemaining > 0)
				{
					rowColumnsRemaining--;
					CalculateColumn_8Rows_Sse41(ref localTargetPtr, ref localPreviousRowPtr, ref row1Costs, ref row2Costs, ref row3Costs, ref row4Costs, ref row5Costs, ref row6Costs, ref row7Costs, ref row8Costs, ref row9Costs, ref allOnesVector, sourceChar1, sourceChar2, sourceChar3, sourceChar4, sourceChar5, sourceChar6, sourceChar7, sourceChar8);
				}
			}
		}

		/// <summary>
		/// Using SSE4.1, calculates the cost for 8 vertically adjacent cells in the virtual matrix.
		/// Comparing 8 vertically adjacent cells prevents 7 target character lookups, 7 deletion cost lookups and 7 saves of the deletion cost.
		/// SSE4.1 instructions allow a virtually branchless minimum value computation when the source and target characters don't match.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="previousRowPtr"></param>
		/// <param name="row1Costs"></param>
		/// <param name="row2Costs"></param>
		/// <param name="row3Costs"></param>
		/// <param name="row4Costs"></param>
		/// <param name="row5Costs"></param>
		/// <param name="allOnesVector"></param>
		/// <param name="sourceChar1"></param>
		/// <param name="sourceChar2"></param>
		/// <param name="sourceChar3"></param>
		/// <param name="sourceChar4"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_8Rows_Sse41(
			ref char* targetPtr,
			ref int* previousRowPtr,
			ref Vector128<int> row1Costs,
			ref Vector128<int> row2Costs,
			ref Vector128<int> row3Costs,
			ref Vector128<int> row4Costs,
			ref Vector128<int> row5Costs,
			ref Vector128<int> row6Costs,
			ref Vector128<int> row7Costs,
			ref Vector128<int> row8Costs,
			ref Vector128<int> row9Costs,
			ref Vector128<int> allOnesVector,
			char sourceChar1,
			char sourceChar2,
			char sourceChar3,
			char sourceChar4,
			char sourceChar5,
			char sourceChar6,
			char sourceChar7,
			char sourceChar8
		)
		{
			var targetChar = targetPtr[0];
			var lastDeletionCost = Vector128.Create(previousRowPtr[0]);
			var localCost = row1Costs;
			if (sourceChar1 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row2Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row1Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row2Costs;
			if (sourceChar2 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row3Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row2Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row3Costs;
			if (sourceChar3 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row4Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row3Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row4Costs;
			if (sourceChar4 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row5Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row4Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row5Costs;
			if (sourceChar5 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row6Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row5Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row6Costs;
			if (sourceChar6 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row7Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row6Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row7Costs;
			if (sourceChar7 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row8Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row7Costs = lastDeletionCost;
			lastDeletionCost = localCost;
			localCost = row8Costs;
			if (sourceChar8 != targetChar)
			{
				localCost = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							row9Costs,
							localCost
						),
						lastDeletionCost
					),
					allOnesVector
				);
			}
			row8Costs = lastDeletionCost;
			row9Costs = localCost;
			previousRowPtr[0] = row9Costs.GetElement(0);

			previousRowPtr++;
			targetPtr++;
		}
	}
}
#endif