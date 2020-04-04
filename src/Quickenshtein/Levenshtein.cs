using System;
using System.Buffers;
using System.Runtime.CompilerServices;
#if NETCOREAPP
using System.Runtime.Intrinsics.X86;
#endif
using Quickenshtein.Internal;

namespace Quickenshtein
{
	/// <summary>
	/// Quick Levenshtein Distance Calculator
	/// </summary>
	public static partial class Levenshtein
	{
#if NETSTANDARD
		public static int GetDistance(string source, string target)
		{
			return GetDistance(source, target, CalculationOptions.Default);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe int GetDistance(string source, string target, CalculationOptions calculationOptions)
		{
			//Shortcut any processing if either string is empty
			if (source == null || source.Length == 0)
			{
				return target?.Length ?? 0;
			}
			if (target == null || target.Length == 0)
			{
				return source.Length;
			}

			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				return CalculateDistance(sourcePtr, targetPtr, source.Length, target.Length, calculationOptions);
			}
		}
#endif

		public static unsafe int GetDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			return GetDistance(source, target, CalculationOptions.Default);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe int GetDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target, CalculationOptions calculationOptions)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			//Shortcut any processing if either string is empty
			if (sourceLength == 0)
			{
				return targetLength;
			}
			if (targetLength == 0)
			{
				return sourceLength;
			}

			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				return CalculateDistance(sourcePtr, targetPtr, sourceLength, targetLength, calculationOptions);
			}
		}

		private static unsafe int CalculateDistance(char* sourcePtr, char* targetPtr, int sourceLength, int targetLength, CalculationOptions calculationOptions)
		{
			//Identify and trim any common prefix or suffix between the strings
			var offset = DataHelper.GetIndexOfFirstNonMatchingCharacter(sourcePtr, targetPtr, sourceLength, targetLength);
			sourcePtr += offset;
			targetPtr += offset;
			sourceLength -= offset;
			targetLength -= offset;
			DataHelper.TrimLengthOfMatchingCharacters(sourcePtr, targetPtr, ref sourceLength, ref targetLength);

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
				var tempSourcePtr = sourcePtr;
				sourcePtr = targetPtr;
				targetPtr = tempSourcePtr;

				(sourceLength, targetLength) = (targetLength, sourceLength);
			}

			if (targetLength >= calculationOptions.EnableThreadingAfterXCharacters)
			{
				return CalculateDistance_MultiThreaded(sourcePtr, targetPtr, sourceLength, targetLength, calculationOptions);
			}

			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetLength);

			fixed (int* previousRowPtr = pooledArray)
			{
				DataHelper.SequentialFill(previousRowPtr, targetLength);

				var rowIndex = 0;

				//Levenshtein Distance outer loop unrolling inspired by Gustaf Andersson's JS implementation
				//https://github.com/gustf/js-levenshtein/blob/55ca1bf22bd55aa81cb5836c63582da6e9fb5fb0/index.js#L71-L90
#if NETCOREAPP
				if (Sse41.IsSupported)
				{
					if (sourceLength > 7)
					{
						CalculateRows_8Rows_Sse41(previousRowPtr, sourcePtr, sourceLength, ref rowIndex, targetPtr, targetLength);
					}
					else
					{
						CalculateRows_4Rows_Sse41(previousRowPtr, sourcePtr, sourceLength, ref rowIndex, targetPtr, targetLength);
					}
				}
				else
				{
					CalculateRows_4Rows(previousRowPtr, sourcePtr, sourceLength, ref rowIndex, targetPtr, targetLength);
				}
#else
				CalculateRows_4Rows(previousRowPtr, sourcePtr, sourceLength, ref rowIndex, targetPtr, targetLength);
#endif

				//Calculate Single Rows
				for (; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = sourcePtr[rowIndex];

#if NETCOREAPP
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

				var result = previousRowPtr[targetLength - 1];
				arrayPool.Return(pooledArray);
				return result;
			}
		}
	}
}
