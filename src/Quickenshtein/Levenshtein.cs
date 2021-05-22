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
		public static int GetDistance(string source, string target)
		{
			return GetDistance(source, target, CalculationOptions.Default);
		}

		public static int GetDistance(string source, string target, CalculationOptions calculationOptions)
		{
			return GetDistance(source.AsSpan(), target.AsSpan(), calculationOptions);
		}

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

#if NETCOREAPP
			if (Sse41.IsSupported)
			{
				//Levenshtein Distance diagonal calculation inspired by Anna Henningsen's C implementation
				//https://github.com/addaleax/levenshtein-sse
				if (sourceLength > 16 && sourceLength < ushort.MaxValue && targetLength < ushort.MaxValue)
				{
					var diag1Array = ArrayPool<ushort>.Shared.Rent(sourceLength + 1);
					var diag2Array = ArrayPool<ushort>.Shared.Rent(sourceLength + 1);

					fixed (void* diag1Ptr = diag1Array)
					fixed (void* diag2Ptr = diag2Array)
					{
						var result = CalculateDiagonal_MinSse41<ushort>(diag1Ptr, diag2Ptr, sourcePtr, sourceLength, targetPtr, targetLength);
						ArrayPool<ushort>.Shared.Return(diag1Array);
						ArrayPool<ushort>.Shared.Return(diag2Array);
						return result;
					}
				}
				else
				{
					var diag1Array = ArrayPool<int>.Shared.Rent(sourceLength + 1);
					var diag2Array = ArrayPool<int>.Shared.Rent(sourceLength + 1);

					fixed (void* diag1Ptr = diag1Array)
					fixed (void* diag2Ptr = diag2Array)
					{
						var result = CalculateDiagonal_MinSse41<int>(diag1Ptr, diag2Ptr, sourcePtr, sourceLength, targetPtr, targetLength);
						ArrayPool<int>.Shared.Return(diag1Array);
						ArrayPool<int>.Shared.Return(diag2Array);
						return result;
					}
				}
			}
#endif

			var pooledArray = ArrayPool<int>.Shared.Rent(targetLength);

			fixed (int* previousRowPtr = pooledArray)
			{
				DataHelper.SequentialFill(previousRowPtr, targetLength);

				var rowIndex = 0;

				//Levenshtein Distance outer loop unrolling inspired by Gustaf Andersson's JS implementation
				//https://github.com/gustf/js-levenshtein/blob/55ca1bf22bd55aa81cb5836c63582da6e9fb5fb0/index.js#L71-L90
				CalculateRows_4Rows(previousRowPtr, sourcePtr, sourceLength, ref rowIndex, targetPtr, targetLength);

				//Calculate Single Rows
				for (; rowIndex < sourceLength; rowIndex++)
				{
					var lastSubstitutionCost = rowIndex;
					var lastInsertionCost = rowIndex + 1;

					var sourcePrevChar = sourcePtr[rowIndex];

					CalculateRow(previousRowPtr, targetPtr, targetLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
				}

				var result = previousRowPtr[targetLength - 1];
				ArrayPool<int>.Shared.Return(pooledArray);
				return result;
			}
		}
	}
}
