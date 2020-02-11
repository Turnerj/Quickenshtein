using System;
using System.Buffers;
using System.Runtime.CompilerServices;
#if NETCOREAPP3_0
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
#endif

namespace Quickenshtein
{
	/// <summary>
	/// Quick Levenshtein Distance Calculator
	/// </summary>
	public static partial class Levenshtein
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryCalculateDistance_Fast(ReadOnlySpan<char> source, ReadOnlySpan<char> target, out int result)
		{
			if (source.Length == target.Length)
			{
				return TryCalculateDistance_Fast_EqualLength(source, target, out result);
			}

			return TryCalculateDistance_Fast_UnEqualLength(source, target, out result);
		}

		/// <summary>
		/// This takes advantage that two equal length strings, represented as a matrix, converge on the same value
		/// by doing character comparisons alone.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryCalculateDistance_Fast_EqualLength(ReadOnlySpan<char> source, ReadOnlySpan<char> target, out int result)
		{
			var distance = 0;
			var charactersRemaining = source.Length - 1;
			var characterIndex = 0;

			if (source[characterIndex] != target[characterIndex])
			{
				distance++;
			}
			characterIndex++;

			while (charactersRemaining > 0)
			{
				if (source[characterIndex] != target[characterIndex])
				{
					//Check validity of fast path
					if (source[characterIndex] == target[characterIndex - 1])
					{
						result = 0;
						return false;
					}

					distance++;
				}

				charactersRemaining--;
				characterIndex++;
			}

			result = distance;
			return true;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryCalculateDistance_Fast_UnEqualLength(ReadOnlySpan<char> source, ReadOnlySpan<char> target, out int result)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			//Check validity of fast path
			var characterIndex = 1;
			while (characterIndex < sourceLength)
			{
				if (
					source[characterIndex] != target[characterIndex] &&
					source[characterIndex] == target[characterIndex - 1]
				)
				{
					result = 0;
					return false;
				}
				characterIndex++;
			}

			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetLength);
			Span<int> previousRow = pooledArray;

			//ArrayPool values are sometimes bigger than allocated, let's trim our span to exactly what we use
			previousRow = previousRow.Slice(0, targetLength);

			FillRow(previousRow);

			fixed (char* targetPtr = target)
			fixed (int* previousRowPtr = previousRow)
			{
				var lastCost = 0;
				for (var rowIndex = 0; rowIndex < sourceLength; rowIndex++)
				{
					var sourcePrevChar = source[rowIndex];

					if (sourcePrevChar != targetPtr[rowIndex])
					{
						lastCost++;
					}

					var lastInsertionCost = lastCost;
					var lastSubstitutionCost = previousRowPtr[rowIndex];


					var columnIndex = rowIndex + 1;
					int lastDeletionCost;
					int localCost;

					var rowColumnsRemaining = targetLength - columnIndex;
					
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

			result = previousRow[targetLength - 1];
			arrayPool.Return(pooledArray);
			return true;
		}
	}
}
