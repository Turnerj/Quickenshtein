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
		private static unsafe int CalculateDistance_Slow(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetLength);
			Span<int> previousRow = pooledArray;

			//ArrayPool values are sometimes bigger than allocated, let's trim our span to exactly what we use
			previousRow = previousRow.Slice(0, targetLength);

			FillRow(previousRow);

			fixed (char* targetPtr = target)
			fixed (int* previousRowPtr = previousRow)
			{
				for (var rowIndex = 0; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = source[rowIndex];

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

			var result = previousRow[targetLength - 1];
			arrayPool.Return(pooledArray);
			return result;
		}
	}
}
