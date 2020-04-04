#if NETSTANDARD2_0
using System;
using System.Runtime.CompilerServices;
using Quickenshtein.Internal;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		public static int GetDistance(string source, string target)
		{
			return GetDistance(source, target, CalculationOptions.Default);
		}

		public static int GetDistance(string source, string target, CalculationOptions calculationOptions)
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

			//Identify and trim any common prefix or suffix between the strings
			TrimInput_NetFramework(source, target, out var startIndex, out var sourceEnd, out var targetEnd);

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

			return CalculateDistance(
				sourceSpan.Slice(startIndex, sourceLength),
				targetSpan.Slice(startIndex, targetLength),
				calculationOptions
			);
		}

		/// <summary>
		/// Specifically used for <see cref="GetDistance(string, string)"/> - helps defer `AsSpan` overhead on .NET Framework
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="startIndex"></param>
		/// <param name="sourceEnd"></param>
		/// <param name="targetEnd"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void TrimInput_NetFramework(string source, string target, out int startIndex, out int sourceEnd, out int targetEnd)
		{
			sourceEnd = source.Length;
			targetEnd = target.Length;

			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				startIndex = DataHelper.GetIndexOfFirstNonMatchingCharacter(sourcePtr, targetPtr, sourceEnd, targetEnd);
				DataHelper.GetIndexesOfLastNonMatchingCharacters(sourcePtr, targetPtr, startIndex, sourceEnd, targetEnd, out sourceEnd, out targetEnd);
			}
		}
	}
}
#endif