using System;
using System.Buffers;
using System.Diagnostics;
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
		private const byte VECTOR256_NUMBER_OF_CHARACTERS = 16;
		private const byte VECTOR256_COMPARISON_ALL_EQUAL = 255;

#if NETSTANDARD2_0
		public static int GetDistance(string source, string target)
		{
			//Shortcut any processing if either string is empty
			if (source == null || source.Length == 0)
			{
				return target?.Length ?? 0;
			}
			if (target == null || target.Length == 0)
			{
				return source?.Length ?? 0;
			}

			//Identify and trim any common prefix or suffix between the strings
			var startIndex = 0;
			var sourceEnd = source.Length;
			var targetEnd = target.Length;

			var charactersAvailableToTrim = Math.Min(targetEnd, sourceEnd);

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

			var sourceSpan = source.AsSpan();
			var targetSpan = target.AsSpan();

			return GetDistanceInternal(
				sourceSpan.Slice(startIndex, sourceLength),
				targetSpan.Slice(startIndex, targetLength)
			);
		}
#endif

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

			var charactersAvailableToTrim = Math.Min(targetEnd, sourceEnd);

#if NETCOREAPP3_0
			if (Avx2.IsSupported && charactersAvailableToTrim >= VECTOR256_NUMBER_OF_CHARACTERS)
			{
				fixed (char* sourcePtr = source)
				fixed (char* targetPtr = target)
				{
					var sourceUShortPtr = (ushort*)sourcePtr;
					var targetUShortPtr = (ushort*)targetPtr;

					while (charactersAvailableToTrim >= VECTOR256_NUMBER_OF_CHARACTERS)
					{
						var sectionEquality = Avx.MoveMask(
							Avx2.CompareEqual(
								Avx.LoadDquVector256(sourceUShortPtr + startIndex),
								Avx.LoadDquVector256(targetUShortPtr + startIndex)
							).AsSingle()
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
						var sectionEquality = Avx.MoveMask(
							Avx2.CompareEqual(
								Avx.LoadDquVector256(sourceUShortPtr + (sourceEnd - VECTOR256_NUMBER_OF_CHARACTERS) - 1),
								Avx.LoadDquVector256(targetUShortPtr + (targetEnd - VECTOR256_NUMBER_OF_CHARACTERS) - 1)
							).AsSingle()
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
#endif

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
			if (TryCalculateDistance_Fast(source, target, out var result))
			{
				Debug.WriteLine("Levenshtein-FastPath");
				return result;
			}
			else
			{
				Debug.WriteLine("Levenshtein-SlowPath");
				return CalculateDistance_Slow(source, target);
			}
		}
	}
}
