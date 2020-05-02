#if NETCOREAPP
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private static readonly Vector256<int> ROTL1_256 = Vector256.Create(7, 0, 1, 2, 3, 4, 5, 6);

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDiagonal_MinSse41(int* diag1Ptr, int* diag2Ptr, char* sourcePtr, char* targetPtr, int targetLength, ref int rowIndex, int columnIndex)
		{
			if (false && Avx2.IsSupported && rowIndex >= Vector256<int>.Count && targetLength - columnIndex >= Vector256<int>.Count)
			{
				var sourceVector = Avx2.ConvertToVector256Int32((ushort*)sourcePtr + rowIndex - 8);
				var targetVector = Avx2.ConvertToVector256Int32((ushort*)targetPtr + columnIndex - 1);
				targetVector = Avx2.Shuffle(targetVector, 0x1b);
				targetVector = Avx2.Permute2x128(targetVector, targetVector, 1);
				var substitutionCost32 = Avx2.CompareEqual(sourceVector, targetVector);

				Vector256<int> diag1_1, diag1_2, diag2_1, diag2_2;

				diag1_1 = Avx.LoadDquVector256(diag1Ptr + rowIndex - 7);
				diag1_2 = Avx.LoadDquVector256(diag1Ptr + rowIndex - 15);
				diag2_1 = Avx.LoadDquVector256(diag2Ptr + rowIndex - 7);
				diag2_2 = Avx.LoadDquVector256(diag2Ptr + rowIndex - 15);

				var blended = Avx2.Blend(diag2_1, diag2_2, 0x80);
				var diag2_i_m1 = Avx2.PermuteVar8x32(blended, ROTL1_256);
				blended = Avx2.Blend(diag1_1, diag1_2, 0x80);
				var diag1_i_m1 = Avx2.PermuteVar8x32(blended, ROTL1_256);

				var result3 = Avx2.Add(diag1_i_m1, substitutionCost32);
				var min = Avx2.Min(Avx2.Min(diag2_i_m1, diag2_1), result3);
				min = Avx2.Add(min, Vector256.Create(1));

				Avx.Store(diag1Ptr + rowIndex - 7, min);
				rowIndex -= Vector256<int>.Count;
			}
			else if (rowIndex >= Vector128<int>.Count && targetLength - columnIndex >= Vector128<int>.Count)
			{
				var sourceVector = Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - 4);
				var targetVector = Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
				targetVector = Sse2.Shuffle(targetVector, 0x1b);
				var substitutionCost32 = Sse2.CompareEqual(sourceVector, targetVector);

				var diag1_i_m1 = Sse3.LoadDquVector128(diag1Ptr + rowIndex - 4);

				var diag2_1 = Sse3.LoadDquVector128(diag2Ptr + rowIndex - 3);
				var diag2_i_m1 = Sse3.LoadDquVector128(diag2Ptr + rowIndex - 4);

				var result3 = Sse2.Add(diag1_i_m1, substitutionCost32);
				var min = Sse41.Min(Sse41.Min(diag2_i_m1, diag2_1), result3);
				min = Sse2.Add(min, Vector128.Create(1));

				Sse2.Store(diag1Ptr + rowIndex - 3, min);
				rowIndex -= Vector128<int>.Count;
			}
			else
			{
				var localCost = Math.Min(diag2Ptr[rowIndex], diag2Ptr[rowIndex - 1]);
				if (localCost < diag1Ptr[rowIndex - 1])
				{
					diag1Ptr[rowIndex] = localCost + 1;
				}
				else
				{
					diag1Ptr[rowIndex] = diag1Ptr[rowIndex - 1] + (sourcePtr[rowIndex - 1] != targetPtr[columnIndex - 1] ? 1 : 0);
				}
				rowIndex--;
			}
		}
	}
}
#endif