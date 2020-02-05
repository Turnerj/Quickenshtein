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
		private const int INTRINSIC_CALCULATION_THRESHOLD = 32;

#if NETSTANDARD2_0
		public static int GetDistance(string source, string target)
		{
			return GetDistance(source.AsSpan(), target.AsSpan());
		}
#endif

		public static int GetDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
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

			while (startIndex < sourceEnd && startIndex < targetEnd && source[startIndex] == target[startIndex])
			{
				startIndex++;
			}
			while (startIndex < sourceEnd && startIndex < targetEnd && source[sourceEnd - 1] == target[targetEnd - 1])
			{
				sourceEnd--;
				targetEnd--;
			}

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

				var tempSourceLength = sourceLength;
				sourceLength = targetLength;
				targetLength = tempSourceLength;
			}

			return GetDistanceInternal(
				source.Slice(startIndex, sourceLength),
				target.Slice(startIndex, targetLength)
			);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int GetDistanceInternal(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var targetLength = target.Length;

			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetLength);
			Span<int> previousRow = pooledArray;

			//ArrayPool values are sometimes bigger than allocated, let's trim our span to exactly what we use
			previousRow = previousRow.Slice(0, targetLength);

			FillRow(previousRow);

#if NETCOREAPP3_0

			Calculate_Standard(previousRow, source, target);
			//if (targetLength > INTRINSIC_CALCULATION_THRESHOLD && (Avx2.IsSupported || Sse2.IsSupported))
			//{
			//	Calculate_Core(previousRow, source, target);
			//}
			//else
			//{
			//	Calculate_Standard(previousRow, source, target);
			//}
#else
			Calculate_Standard(previousRow, source, target);
#endif

			var result = previousRow[targetLength - 1];
			arrayPool.Return(pooledArray);
			return result;
		}

#if NETCOREAPP3_0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void Calculate_Core(Span<int> previousRow, ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			fixed (char* targetPtr = target)
			fixed (int* previousRowPtr = previousRow)
			{
				var ushortTargetPtr = (ushort*)targetPtr;

				for (var rowIndex = 0; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = source[rowIndex];

					var columnIndex = 0;

					if (Avx2.IsSupported)
					{
						CalculateDistance_Avx2(sourcePrevChar, ushortTargetPtr, targetLength, ref lastInsertionCost, ref lastSubstitutionCost, previousRowPtr, ref columnIndex);
					}
					else if (Sse2.IsSupported)
					{
						CalculateDistance_Sse3(sourcePrevChar, ushortTargetPtr, targetLength, ref lastInsertionCost, ref lastSubstitutionCost, previousRowPtr, ref columnIndex);
					}

					int lastDeletionCost;
					int localCost;

					while (columnIndex < targetLength)
					{
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
#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void Calculate_Standard(Span<int> previousRow, ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

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

					//Loop unrolling inspired by CoreLib SpanHelpers
					//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
					var rowColumnsRemaining = targetLength;
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
	}
}
