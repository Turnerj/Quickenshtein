using System.Runtime.CompilerServices;
#if NETCOREAPP
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Quickenshtein.Internal
{
	internal static class SequentialFillHelper
	{
#if NETCOREAPP
		private const int VECTOR256_SEQUENCE_SIZE = 8;
		private static readonly Vector256<int> VECTOR256_START_FROM_ONE_SEQUENCE = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);

		private const int VECTOR128_SEQUENCE_SIZE = 4;
		private static readonly Vector128<int> VECTOR128_START_FROM_ONE_SEQUENCE = Vector128.Create(1, 2, 3, 4);
#endif

		/// <summary>
		/// Fills <paramref name="targetPtr"/> with a number sequence from 1 to the length specified.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill(int* targetPtr, int length)
		{
			var index = 0;

			while (length >= 8)
			{
				length -= 8;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
			}

			if (length > 4)
			{
				length -= 4;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
				targetPtr[index] = ++index;
			}

			while (length > 0)
			{
				length--;
				targetPtr[index] = ++index;
			}
		}

		/// <summary>
		/// Fills <paramref name="targetPtr"/> with a number sequence from <paramref name="startValue"/> to the length specified.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="startValue"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill(int* targetPtr, int startValue, int length)
		{
			var index = 0;
			var value = startValue;

			while (length >= 8)
			{
				length -= 8;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
			}

			if (length > 4)
			{
				length -= 4;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
				targetPtr[index++] = value++;
			}

			while (length > 0)
			{
				length--;
				targetPtr[index++] = value++;
			}
		}

#if NETCOREAPP
		/// <summary>
		/// Using AVX2, fills <paramref name="targetPtr"/> with a number sequence starting from 1 for the length specified.
		/// AVX2 instructions allow for a maximum fill rate of 8 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Avx2(int* targetPtr, int length)
		{
			Fill_Avx2(targetPtr, VECTOR256_START_FROM_ONE_SEQUENCE, length);
		}
		
		/// <summary>
		/// Using AVX2, fills <paramref name="targetPtr"/> with a number sequence starting from <paramref name="startValue"/> for the length specified.
		/// AVX2 instructions allow for a maximum fill rate of 8 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="startValue"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Avx2(int* targetPtr, int startValue, int length)
		{
			var startVector = Vector256.Create(
				startValue,
				startValue + 1,
				startValue + 2,
				startValue + 3,
				startValue + 4,
				startValue + 5,
				startValue + 6,
				startValue + 7
			);
			Fill_Avx2(targetPtr, startVector, length);
		}

		/// <summary>
		/// Using AVX2, fills <paramref name="targetPtr"/> with a number sequence using <paramref name="startVector"/> for the length specified.
		/// AVX2 instructions allow for a maximum fill rate of 8 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="startVector"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Avx2(int* targetPtr, Vector256<int> startVector, int length)
		{
			var index = 0;
			var lastVector256 = startVector;
			var shiftVector256 = Vector256.Create(VECTOR256_SEQUENCE_SIZE);

			while (length >= VECTOR256_SEQUENCE_SIZE)
			{
				length -= VECTOR256_SEQUENCE_SIZE;
				Avx.Store(targetPtr + index, lastVector256);
				lastVector256 = Avx2.Add(lastVector256, shiftVector256);
				index += VECTOR256_SEQUENCE_SIZE;
			}

			var value = lastVector256.GetElement(7) - VECTOR256_SEQUENCE_SIZE;

			if (length > 4)
			{
				length -= 4;
				targetPtr[index++] = ++value;
				targetPtr[index++] = ++value;
				targetPtr[index++] = ++value;
				targetPtr[index++] = ++value;
			}

			while (length > 0)
			{
				length--;
				targetPtr[index++] = ++value;
			}
		}

		/// <summary>
		/// Using SSE2, fills <paramref name="targetPtr"/> with a number sequence starting from 1 for the length specified.
		/// SSE2 instructions provide a maximum fill rate of 4 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Sse2(int* targetPtr, int length)
		{
			Fill_Sse2(targetPtr, VECTOR128_START_FROM_ONE_SEQUENCE, length);
		}

		/// <summary>
		///	Using SSE2, fills <paramref name="targetPtr"/> with a number sequence starting from <paramref name="startValue"/> for the length specified.
		/// SSE2 instructions provide a maximum fill rate of 4 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="startValue"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Sse2(int* targetPtr, int startValue, int length)
		{
			var startVector = Vector128.Create(
				startValue,
				startValue + 1,
				startValue + 2,
				startValue + 3
			);
			Fill_Sse2(targetPtr, startVector, length);
		}

		/// <summary>
		/// Using SSE2, fills <paramref name="targetPtr"/> with a number sequence using <paramref name="startVector"/> for the length specified.
		/// SSE2 instructions provide a maximum fill rate of 4 values at once.
		/// </summary>
		/// <param name="targetPtr"></param>
		/// <param name="startVector"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void Fill_Sse2(int* targetPtr, Vector128<int> startVector, int length)
		{
			var index = 0;
			var lastVector128 = startVector;
			var shiftVector128 = Vector128.Create(VECTOR128_SEQUENCE_SIZE);

			while (length >= VECTOR128_SEQUENCE_SIZE)
			{
				length -= VECTOR128_SEQUENCE_SIZE;
				Sse2.Store(targetPtr + index, lastVector128);
				lastVector128 = Sse2.Add(lastVector128, shiftVector128);
				index += VECTOR128_SEQUENCE_SIZE;
			}

			var value = lastVector128.GetElement(3) - VECTOR128_SEQUENCE_SIZE;

			while (length > 0)
			{
				length--;
				targetPtr[index++] = ++value;
			}
		}
#endif
	}
}
