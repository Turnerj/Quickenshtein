#if NETCOREAPP3_0
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private const byte VECTOR256_NUMBER_OF_CHARACTERS = 16;
		private const sbyte VECTOR256_COMPARISON_ALL_EQUAL = -1;

		private const int VECTOR256_FILL_SIZE = 8;
		private static readonly Vector256<int> VECTOR256_SEQUENCE = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);

		private const int VECTOR128_FILL_SIZE = 4;
		private static readonly Vector128<int> VECTOR128_SEQUENCE = Vector128.Create(1, 2, 3, 4);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void TrimComparison_Avx2(ReadOnlySpan<char> source, ReadOnlySpan<char> target, ref int startIndex, ref int sourceEnd, ref int targetEnd)
		{
			var charactersAvailableToTrim = Math.Min(sourceEnd, targetEnd);
			if (charactersAvailableToTrim >= VECTOR256_NUMBER_OF_CHARACTERS)
			{
				fixed (char* sourcePtr = source)
				fixed (char* targetPtr = target)
				{
					var sourceUShortPtr = (ushort*)sourcePtr;
					var targetUShortPtr = (ushort*)targetPtr;

					while (charactersAvailableToTrim >= VECTOR256_NUMBER_OF_CHARACTERS)
					{
						var sectionEquality = Avx2.MoveMask(
							Avx2.CompareEqual(
								Avx.LoadDquVector256(sourceUShortPtr + startIndex),
								Avx.LoadDquVector256(targetUShortPtr + startIndex)
							).AsByte()
						);

						if (sectionEquality != VECTOR256_COMPARISON_ALL_EQUAL)
						{
							break;
						}

						startIndex += VECTOR256_NUMBER_OF_CHARACTERS;
						charactersAvailableToTrim -= VECTOR256_NUMBER_OF_CHARACTERS;
					}

					while (charactersAvailableToTrim >= VECTOR256_NUMBER_OF_CHARACTERS)
					{
						var sectionEquality = Avx2.MoveMask(
							Avx2.CompareEqual(
								Avx.LoadDquVector256(sourceUShortPtr + (sourceEnd - VECTOR256_NUMBER_OF_CHARACTERS + 1)),
								Avx.LoadDquVector256(targetUShortPtr + (targetEnd - VECTOR256_NUMBER_OF_CHARACTERS + 1))
							).AsByte()
						);

						if (sectionEquality != VECTOR256_COMPARISON_ALL_EQUAL)
						{
							break;
						}

						sourceEnd -= VECTOR256_NUMBER_OF_CHARACTERS;
						targetEnd -= VECTOR256_NUMBER_OF_CHARACTERS;
						charactersAvailableToTrim -= VECTOR256_NUMBER_OF_CHARACTERS;
					}
				}
			}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void FillRow_Avx2(Span<int> previousRow)
		{
			var columnIndex = 0;
			var columnsRemaining = previousRow.Length;

			fixed (int* previousRowPtr = previousRow)
			{
				var lastVector256 = VECTOR256_SEQUENCE;
				var shiftVector256 = Vector256.Create(VECTOR256_FILL_SIZE);

				while (columnsRemaining >= VECTOR256_FILL_SIZE)
				{
					columnsRemaining -= VECTOR256_FILL_SIZE;
					Avx.Store(previousRowPtr + columnIndex, lastVector256);
					lastVector256 = Avx2.Add(lastVector256, shiftVector256);
					columnIndex += VECTOR256_FILL_SIZE;
				}

				if (columnsRemaining > 4)
				{
					columnsRemaining -= 4;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
				}

				while (columnsRemaining > 0)
				{
					columnsRemaining--;
					previousRowPtr[columnIndex] = ++columnIndex;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void FillRow_Sse2(Span<int> previousRow)
		{
			var columnIndex = 0;
			var columnsRemaining = previousRow.Length;

			fixed (int* previousRowPtr = previousRow)
			{
				var lastVector128 = VECTOR128_SEQUENCE;
				var shiftVector128 = Vector128.Create(VECTOR128_FILL_SIZE);

				while (columnsRemaining >= VECTOR128_FILL_SIZE)
				{
					columnsRemaining -= VECTOR128_FILL_SIZE;
					Sse2.Store(previousRowPtr + columnIndex, lastVector128);
					lastVector128 = Sse2.Add(lastVector128, shiftVector128);
					columnIndex += VECTOR128_FILL_SIZE;
				}

				while (columnsRemaining > 0)
				{
					columnsRemaining--;
					previousRowPtr[columnIndex] = ++columnIndex;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateRow_Unroll8_Sse41(int* previousRowPtr, char* targetPtr, int targetLength, char sourcePrevChar, int lastInsertionCost, int lastSubstitutionCost)
		{
			var columnIndex = 0;
			var rowColumnsRemaining = targetLength;

			var allOnesVector = Vector128.Create(1);
			var lastInsertionCostVector = Vector128.Create(lastInsertionCost);
			var lastSubstitutionCostVector = Vector128.Create(lastSubstitutionCost);
			var lastDeletionCostVector = Vector128<int>.Zero;

			//Loop unrolling inspired by CoreLib SpanHelpers
			//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
			while (rowColumnsRemaining >= 8)
			{
				rowColumnsRemaining -= 8;
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
			}

			if (rowColumnsRemaining > 4)
			{
				rowColumnsRemaining -= 4;
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
			}

			while (rowColumnsRemaining > 0)
			{
				rowColumnsRemaining--;
				CalculateColumn_Sse41(previousRowPtr, targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector, ref columnIndex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_Sse41(
			int* previousRowPtr, 
			char* targetPtr, 
			char sourcePrevChar, 
			ref Vector128<int> lastSubstitutionCostVector, 
			ref Vector128<int> lastInsertionCostVector,
			ref Vector128<int> lastDeletionCostVector,
			ref Vector128<int> allOnesVector,
			ref int columnIndex
		)
		{
			var localCostVector = lastSubstitutionCostVector;
			lastDeletionCostVector = Vector128.Create(previousRowPtr[columnIndex]);
			if (sourcePrevChar != targetPtr[columnIndex])
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
			previousRowPtr[columnIndex++] = localCostVector.GetElement(0);
			lastSubstitutionCostVector = lastDeletionCostVector;
		}
	}
}
#endif