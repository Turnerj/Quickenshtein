using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
#if NETCOREAPP3_0
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private const int VECTOR256_FILL_SIZE = 8;
		private const int VECTOR128_FILL_SIZE = 4;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void FillRow(Span<int> previousRow)
		{
			var columnIndex = 0;
			var columnsRemaining = previousRow.Length;

			fixed (int* previousRowPtr = previousRow)
			{
#if NETCOREAPP3_0
				if (Avx2.IsSupported)
				{
					var lastVector256 = Vector256.Create(1, 2, 3, 4, 5, 6, 7, 8);
					var shiftVector256 = Vector256.Create(VECTOR256_FILL_SIZE);

					while (columnsRemaining >= VECTOR256_FILL_SIZE)
					{
						columnsRemaining -= VECTOR256_FILL_SIZE;
						Avx.Store(previousRowPtr + columnIndex, lastVector256);
						lastVector256 = Avx2.Add(lastVector256, shiftVector256);
						columnIndex += VECTOR256_FILL_SIZE;
					}
				}
				else if (Sse2.IsSupported)
				{
					var lastVector128 = Vector128.Create(1, 2, 3, 4);
					var shiftVector128 = Vector128.Create(VECTOR128_FILL_SIZE);

					while (columnsRemaining >= VECTOR128_FILL_SIZE)
					{
						columnsRemaining -= VECTOR128_FILL_SIZE;
						Sse2.Store(previousRowPtr + columnIndex, lastVector128);
						lastVector128 = Sse2.Add(lastVector128, shiftVector128);
						columnIndex += VECTOR128_FILL_SIZE;
					}
				}
				else
#endif
				{
					while (columnsRemaining >= 8)
					{
						columnsRemaining -= 8;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
						previousRowPtr[columnIndex] = ++columnIndex;
					}
				}

				if (columnsRemaining > 4)
				{
					columnsRemaining -= 4;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
					previousRowPtr[columnIndex] = ++columnIndex;
				}

				while (columnsRemaining > 0)
				{
					columnsRemaining--;
					previousRowPtr[columnIndex] = ++columnIndex;
				}
			}
		}
	}
}
