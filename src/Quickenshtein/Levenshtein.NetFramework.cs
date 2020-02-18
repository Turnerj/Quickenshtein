#if NETSTANDARD2_0
using System;
using System.Runtime.CompilerServices;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
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
				targetSpan.Slice(startIndex, targetLength)
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
	}
}
#endif