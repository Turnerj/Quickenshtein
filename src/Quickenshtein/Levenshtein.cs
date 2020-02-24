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
				TrimInput_Avx2(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
			}
			else
			{
				TrimInput(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
			}
#else
			TrimInput(source, target, ref startIndex, ref sourceEnd, ref targetEnd);
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
				FillRow(previousRow);
			}
#else
			FillRow(previousRow);
#endif

			fixed (char* targetPtr = target)
			fixed (int* previousRowPtr = previousRow)
			{
				var rowIndex = 0;

				//Levenshtein Distance outer loop unrolling inspired by Gustaf Andersson's JS implementation
				//https://github.com/gustf/js-levenshtein/blob/55ca1bf22bd55aa81cb5836c63582da6e9fb5fb0/index.js#L71-L90
#if NETCOREAPP3_0
				if (Sse41.IsSupported)
				{
					if (sourceLength > 7)
					{
						CalculateRows_8Rows_Sse41(previousRowPtr, source, ref rowIndex, targetPtr, targetLength);
					}
					else
					{
						CalculateRows_4Rows_Sse41(previousRowPtr, source, ref rowIndex, targetPtr, targetLength);
					}
				}
				else
				{
					CalculateRows(previousRowPtr, source, ref rowIndex, targetPtr, targetLength);
				}
#else
				CalculateRows(previousRowPtr, source, ref rowIndex, targetPtr, targetLength);
#endif

				//Calculate Single Rows
				for (; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = source[rowIndex];

#if NETCOREAPP3_0
					if (Sse41.IsSupported)
					{
						CalculateRow_Sse41(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
					else
					{
						CalculateRow(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
#else
					CalculateRow(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
#endif
				}
			}

			var result = previousRow[targetLength - 1];
			arrayPool.Return(pooledArray);
			return result;
		}
	}
}
