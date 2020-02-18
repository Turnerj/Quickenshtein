using System;
using System.Buffers;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_0
using System.Runtime.Intrinsics.X86;
#endif

namespace Quickenshtein
{
	/// <summary>
	/// Quick Levenshtein Distance Calculator
	/// </summary>
	public static partial class Levenshtein
	{
		public static unsafe int GetDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceEnd = source.Length;
			var targetEnd = target.Length;

			//Shortcut any processing if either string is empty
			if (sourceEnd == 0)
			{
				return targetEnd;
			}
			if (targetEnd == 0)
			{
				return sourceEnd;
			}

			//Identify and trim any common prefix or suffix between the strings
			var startIndex = 0;

#if NETCOREAPP3_0
			if (Avx2.IsSupported)
			{
				TrimComparison_Avx2(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
			}
			else
			{
				TrimComparison_Common(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
			}
#else
			TrimComparison_Common(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
#endif

			var sourceLength = sourceEnd - startIndex;
			var targetLength = targetEnd - startIndex;

			//Check the trimmed values are not empty
			if (sourceLength == 0)
			{
				return targetLength;
			}
			if (targetLength == 0)
			{
				return sourceLength;
			}

			//Switch around variables so outer loop has fewer iterations
			if (targetLength < sourceLength)
			{
				var tempSource = source;
				source = target;
				target = tempSource;

				(sourceLength, targetLength) = (targetLength, sourceLength);
			}

			return CalculateDistance(
				source.Slice(startIndex, sourceLength),
				target.Slice(startIndex, targetLength)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void TrimComparison_Common(ReadOnlySpan<char> source, ReadOnlySpan<char> target, ref int startIndex, ref int sourceEnd, ref int targetEnd)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int CalculateDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetLength);
			Span<int> previousRow = pooledArray;

			//ArrayPool values are sometimes bigger than allocated, let's trim our span to exactly what we use
			previousRow = previousRow.Slice(0, targetLength);

#if NETCOREAPP3_0
			if (Avx2.IsSupported)
			{
				FillRow_Avx2(previousRow);
			}
			else if (Sse2.IsSupported)
			{
				FillRow_Sse2(previousRow);
			}
			else
			{
				FillRow_Unroll8(previousRow);
			}
#else
			FillRow_Unroll8(previousRow);
#endif

			fixed (char* targetPtr = target)
			fixed (int* previousRowPtr = previousRow)
			{
				for (var rowIndex = 0; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = source[rowIndex];

#if NETCOREAPP3_0
					if (Sse41.IsSupported)
					{
						CalculateRow_Unroll8_Sse41(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
					else
					{
						CalculateRow_Unroll8(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
#else
					CalculateRow_Unroll8(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
#endif
				}
			}

			var result = previousRow[targetLength - 1];
			arrayPool.Return(pooledArray);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void FillRow_Unroll8(Span<int> previousRow)
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateRow_Unroll8(int* previousRowPtr, char* targetPtr, int targetLength, char sourcePrevChar, int lastInsertionCost, int lastSubstitutionCost)
		{
			var columnIndex = 0;
			int lastDeletionCost;
			int localCost;

			var rowColumnsRemaining = targetLength;

			//Loop unrolling inspired by CoreLib SpanHelpers
			//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
			while (rowColumnsRemaining >= 8)
			{
				rowColumnsRemaining -= 8;

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
	}
}
