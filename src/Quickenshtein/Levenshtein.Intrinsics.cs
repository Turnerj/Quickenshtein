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

		private static unsafe void CalculateDiagonal_4_Sse41(int* diag1, int* diag2, char* sourcePtr, char* targetPtr, int targetLength, ref int rowIndex, int columnIndex)
		{
			if (rowIndex >= 4 && targetLength - columnIndex >= 4)
			{
				var a_ = Sse41.ConvertToVector128Int32(Sse3.LoadDquVector128((ushort*)sourcePtr + rowIndex - 4));
				var b_ = Sse41.ConvertToVector128Int32(Sse3.LoadDquVector128((ushort*)targetPtr + columnIndex - 1));
				b_ = Sse2.Shuffle(b_, 0x1b); // simple reverse
				var substitutionCost32 = Sse2.CompareEqual(a_, b_);

				Vector128<int> diag1_1, diag1_2, diag2_1, diag2_2;

				diag1_1 = Sse3.LoadDquVector128(diag1 + rowIndex - 3);
				diag1_2 = Sse3.LoadDquVector128(diag1 + rowIndex - 7);
				diag2_1 = Sse3.LoadDquVector128(diag2 + rowIndex - 3);
				diag2_2 = Sse3.LoadDquVector128(diag2 + rowIndex - 7);

				var diag2_i_m1 = Ssse3.AlignRight(diag2_1, diag2_2, 12);
				var diag_i_m1 = Ssse3.AlignRight(diag1_1, diag1_2, 12);

				var result3 = Sse2.Add(diag_i_m1, substitutionCost32);
				var min = Sse41.Min(Sse41.Min(diag2_i_m1, diag2_1), result3);
				min = Sse2.Add(min, Vector128.Create(1));

				Sse2.Store(diag1 + rowIndex - 3, min);
				rowIndex -= 4;
			}
			else
			{
				var min = Math.Min(diag2[rowIndex], diag2[rowIndex - 1]);
				if (min < diag1[rowIndex - 1])
				{
					diag1[rowIndex] = min + 1;
				}
				else
				{
					diag1[rowIndex] = diag1[rowIndex - 1] + (sourcePtr[rowIndex - 1] != targetPtr[columnIndex - 1] ? 1 : 0);
				}
				rowIndex--;
			}
		}
	}
}
#endif