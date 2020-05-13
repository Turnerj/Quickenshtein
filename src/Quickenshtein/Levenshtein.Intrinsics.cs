#if NETCOREAPP
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private static readonly Vector256<int> ROTL1_256 = Vector256.Create(7, 0, 1, 2, 3, 4, 5, 6);
		private static readonly Vector128<byte> REVERSE_USHORT_AS_BYTE_128 = Vector128.Create((byte)14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);
		private static readonly Vector256<byte> REVERSE_USHORT_AS_BYTE_256 = Vector256.Create((byte)30, 31, 28, 29, 26, 27, 24, 25, 22, 23, 20, 21, 18, 19, 16, 17, 14, 15, 12, 13, 10, 11, 8, 9, 6, 7, 4, 5, 2, 3, 0, 1);

		/// <summary>
		/// Using SSE4.1, calculates the costs for an entire row of the virtual matrix.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="targetPtr"></param>
		/// <param name="targetLength"></param>
		/// <param name="sourcePrevChar"></param>
		/// <param name="lastInsertionCost"></param>
		/// <param name="lastSubstitutionCost"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static unsafe void CalculateRow_Sse41(int* previousRowPtr, char* targetPtr, int targetLength, char sourcePrevChar, int lastInsertionCost, int lastSubstitutionCost)
		{
			var rowColumnsRemaining = targetLength;

			var allOnesVector = Vector128.Create(1);
			var lastInsertionCostVector = Vector128.Create(lastInsertionCost);
			var lastSubstitutionCostVector = Vector128.Create(lastSubstitutionCost);
			var lastDeletionCostVector = Vector128<int>.Zero;

			//Levenshtein Distance inner loop unrolling inspired by CoreLib SpanHelpers
			//https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Private.CoreLib/src/System/SpanHelpers.T.cs#L62-L118
			while (rowColumnsRemaining >= 8)
			{
				rowColumnsRemaining -= 8;
				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);

				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}

			if (rowColumnsRemaining > 4)
			{
				rowColumnsRemaining -= 4;
				lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}

			lastDeletionCostVector = Sse2.LoadVector128(previousRowPtr);
			while (rowColumnsRemaining > 0)
			{
				rowColumnsRemaining--;
				CalculateColumn_Sse41(ref previousRowPtr, ref targetPtr, sourcePrevChar, ref lastSubstitutionCostVector, ref lastInsertionCostVector, ref lastDeletionCostVector, ref allOnesVector);
			}
		}

		/// <summary>
		/// Using SSE4.1, calculates the cost for an individual cell in the virtual matrix.
		/// SSE4.1 instructions allow a virtually branchless minimum value computation when the source and target characters don't match.
		/// </summary>
		/// <param name="previousRowPtr"></param>
		/// <param name="targetPtr"></param>
		/// <param name="sourcePrevChar"></param>
		/// <param name="lastSubstitutionCostVector"></param>
		/// <param name="lastInsertionCostVector"></param>
		/// <param name="lastDeletionCostVector"></param>
		/// <param name="allOnesVector"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateColumn_Sse41(
			ref int* previousRowPtr,
			ref char* targetPtr,
			char sourcePrevChar,
			ref Vector128<int> lastSubstitutionCostVector,
			ref Vector128<int> lastInsertionCostVector,
			ref Vector128<int> lastDeletionCostVector,
			ref Vector128<int> allOnesVector
		)
		{
			var localCostVector = lastSubstitutionCostVector;

			if (sourcePrevChar != targetPtr[0])
			{
				localCostVector = Sse2.Add(
					Sse41.Min(
						Sse41.Min(
							lastInsertionCostVector,
							localCostVector
						),
						lastDeletionCostVector
					),
					allOnesVector
				);
			}
			lastInsertionCostVector = localCostVector;
			previousRowPtr[0] = localCostVector.GetElement(0);
			lastSubstitutionCostVector = lastDeletionCostVector;

			previousRowPtr++;
			targetPtr++;
			lastDeletionCostVector = Sse2.ShiftRightLogical128BitLane(lastDeletionCostVector, 4);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe T CalculateDiagonal_MinSse41<T>(void* diag1Ptr, void* diag2Ptr, char* sourcePtr, int sourceLength, char* targetPtr, int targetLength) where T : struct
		{
			new Span<T>(diag1Ptr, sourceLength + 1).Clear();
			new Span<T>(diag2Ptr, sourceLength + 1).Clear();

			int rowIndex, columnIndex, endRow;

			var counter = 1;
			while (true)
			{
				var startRow = counter > targetLength ? counter - targetLength : 1;

				if (counter > sourceLength)
				{
					endRow = sourceLength;
				}
				else
				{
					Unsafe.Write(Unsafe.Add<T>(diag1Ptr, counter), Unsafe.As<int, T>(ref counter));
					endRow = counter - 1;
				}

				for (rowIndex = endRow; rowIndex >= startRow;)
				{
					columnIndex = counter - rowIndex;
					CalculateDiagonalSection_MinSse41<T>(diag1Ptr, diag2Ptr, sourcePtr, targetPtr, targetLength, ref rowIndex, columnIndex);
				}

				if (counter == sourceLength + targetLength)
				{
					return Unsafe.Read<T>(Unsafe.Add<T>(diag1Ptr, startRow));
				}

				Unsafe.Write(diag1Ptr, Unsafe.As<int, T>(ref counter));

				var tempPtr = diag1Ptr;
				diag1Ptr = diag2Ptr;
				diag2Ptr = tempPtr;

				counter++;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDiagonalSection_MinSse41<T>(void* refDiag1Ptr, void* refDiag2Ptr, char* sourcePtr, char* targetPtr, int targetLength, ref int rowIndex, int columnIndex) where T : struct
		{
			if (Avx2.IsSupported && rowIndex >= Vector256<T>.Count && targetLength - columnIndex >= Vector256<T>.Count)
			{
				CalculateDiagonalSection_Avx2<T>(refDiag1Ptr, refDiag2Ptr, sourcePtr, targetPtr, ref rowIndex, columnIndex);
				rowIndex -= Vector256<T>.Count;
			}
			else if (rowIndex >= Vector128<T>.Count && targetLength - columnIndex >= Vector128<T>.Count)
			{
				CalculateDiagonalSection_Sse41<T>(refDiag1Ptr, refDiag2Ptr, sourcePtr, targetPtr, ref rowIndex, columnIndex);
				rowIndex -= Vector128<T>.Count;
			}
			else
			{
				CalculateDiagonalSection_Single<T>(refDiag1Ptr, refDiag2Ptr, sourcePtr, targetPtr, ref rowIndex, columnIndex);
				rowIndex--;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDiagonalSection_Single<T>(void* refDiag1Ptr, void* refDiag2Ptr, char* sourcePtr, char* targetPtr, ref int rowIndex, int columnIndex) where T : struct
		{
			if (typeof(T) == typeof(int))
			{
				var diag1Ptr = (int*)refDiag1Ptr;
				var diag2Ptr = (int*)refDiag2Ptr;

				var localCost = Math.Min(diag2Ptr[rowIndex], diag2Ptr[rowIndex - 1]);
				if (localCost < diag1Ptr[rowIndex - 1])
				{
					diag1Ptr[rowIndex] = localCost + 1;
				}
				else
				{
					diag1Ptr[rowIndex] = diag1Ptr[rowIndex - 1] + (sourcePtr[rowIndex - 1] != targetPtr[columnIndex - 1] ? 1 : 0);
				}
			}
			else if (typeof(T) == typeof(ushort))
			{
				var diag1Ptr = (ushort*)refDiag1Ptr;
				var diag2Ptr = (ushort*)refDiag2Ptr;

				var localCost = Math.Min(diag2Ptr[rowIndex], diag2Ptr[rowIndex - 1]);
				if (localCost < diag1Ptr[rowIndex - 1])
				{
					diag1Ptr[rowIndex] = (ushort)(localCost + 1);
				}
				else
				{
					diag1Ptr[rowIndex] = (ushort)(diag1Ptr[rowIndex - 1] + (sourcePtr[rowIndex - 1] != targetPtr[columnIndex - 1] ? 1 : 0));
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDiagonalSection_Sse41<T>(void* refDiag1Ptr, void* refDiag2Ptr, char* sourcePtr, char* targetPtr, ref int rowIndex, int columnIndex) where T : struct
		{
			if (typeof(T) == typeof(int))
			{
				var diag1Ptr = (int*)refDiag1Ptr;
				var diag2Ptr = (int*)refDiag2Ptr;

				var sourceVector = Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - Vector128<T>.Count);
				var targetVector = Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
				targetVector = Sse2.Shuffle(targetVector, 0x1b);
				var substitutionCostAdjustment = Sse2.CompareEqual(sourceVector, targetVector);

				var substitutionCost = Sse2.Add(
					Sse3.LoadDquVector128(diag1Ptr + rowIndex - Vector128<T>.Count),
					substitutionCostAdjustment
				);

				var deleteCost = Sse3.LoadDquVector128(diag2Ptr + rowIndex - (Vector128<T>.Count - 1));
				var insertCost = Sse3.LoadDquVector128(diag2Ptr + rowIndex - Vector128<T>.Count);

				var localCost = Sse41.Min(Sse41.Min(insertCost, deleteCost), substitutionCost);
				localCost = Sse2.Add(localCost, Vector128.Create(1));

				Sse2.Store(diag1Ptr + rowIndex - (Vector128<T>.Count - 1), localCost);
			}
			else if (typeof(T) == typeof(ushort))
			{
				var diag1Ptr = (ushort*)refDiag1Ptr;
				var diag2Ptr = (ushort*)refDiag2Ptr;

				var sourceVector = Sse3.LoadDquVector128((ushort*)sourcePtr + rowIndex - Vector128<T>.Count);
				var targetVector = Sse3.LoadDquVector128((ushort*)targetPtr + columnIndex - 1);
				targetVector = Ssse3.Shuffle(targetVector.AsByte(), REVERSE_USHORT_AS_BYTE_128).AsUInt16();
				var substitutionCostAdjustment = Sse2.CompareEqual(sourceVector, targetVector);

				var substitutionCost = Sse2.Add(
					Sse3.LoadDquVector128(diag1Ptr + rowIndex - Vector128<T>.Count),
					substitutionCostAdjustment
				);

				var deleteCost = Sse3.LoadDquVector128(diag2Ptr + rowIndex - (Vector128<T>.Count - 1));
				var insertCost = Sse3.LoadDquVector128(diag2Ptr + rowIndex - Vector128<T>.Count);

				var localCost = Sse41.Min(Sse41.Min(insertCost, deleteCost), substitutionCost);
				localCost = Sse2.Add(localCost, Vector128.Create((ushort)1));

				Sse2.Store(diag1Ptr + rowIndex - (Vector128<T>.Count - 1), localCost);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDiagonalSection_Avx2<T>(void* refDiag1Ptr, void* refDiag2Ptr, char* sourcePtr, char* targetPtr, ref int rowIndex, int columnIndex) where T : struct
		{
			if (typeof(T) == typeof(int))
			{
				var diag1Ptr = (int*)refDiag1Ptr;
				var diag2Ptr = (int*)refDiag2Ptr;

				var sourceVector = Avx2.ConvertToVector256Int32((ushort*)sourcePtr + rowIndex - Vector256<T>.Count);
				var targetVector = Avx2.ConvertToVector256Int32((ushort*)targetPtr + columnIndex - 1);
				targetVector = Avx2.Shuffle(targetVector, 0x1b);
				targetVector = Avx2.Permute2x128(targetVector, targetVector, 1);
				var substitutionCostAdjustment = Avx2.CompareEqual(sourceVector, targetVector);

				var substitutionCost = Avx2.Add(
					Avx.LoadDquVector256(diag1Ptr + rowIndex - Vector256<T>.Count),
					substitutionCostAdjustment
				);
				var deleteCost = Avx.LoadDquVector256(diag2Ptr + rowIndex - (Vector256<T>.Count - 1));
				var insertCost = Avx.LoadDquVector256(diag2Ptr + rowIndex - Vector256<T>.Count);

				var localCost = Avx2.Min(Avx2.Min(insertCost, deleteCost), substitutionCost);
				localCost = Avx2.Add(localCost, Vector256.Create(1));

				Avx.Store(diag1Ptr + rowIndex - (Vector256<T>.Count - 1), localCost);
			}
			else if (typeof(T) == typeof(ushort))
			{
				var diag1Ptr = (ushort*)refDiag1Ptr;
				var diag2Ptr = (ushort*)refDiag2Ptr;

				var sourceVector = Avx.LoadDquVector256((ushort*)sourcePtr + rowIndex - Vector256<T>.Count);
				var targetVector = Avx.LoadDquVector256((ushort*)targetPtr + columnIndex - 1);
				targetVector = Avx2.Shuffle(targetVector.AsByte(), REVERSE_USHORT_AS_BYTE_256).AsUInt16();
				targetVector = Avx2.Permute2x128(targetVector, targetVector, 1);
				var substitutionCostAdjustment = Avx2.CompareEqual(sourceVector, targetVector);

				var substitutionCost = Avx2.Add(
					Avx.LoadDquVector256(diag1Ptr + rowIndex - Vector256<T>.Count),
					substitutionCostAdjustment
				);
				var deleteCost = Avx.LoadDquVector256(diag2Ptr + rowIndex - (Vector256<T>.Count - 1));
				var insertCost = Avx.LoadDquVector256(diag2Ptr + rowIndex - Vector256<T>.Count);

				var localCost = Avx2.Min(Avx2.Min(insertCost, deleteCost), substitutionCost);
				localCost = Avx2.Add(localCost, Vector256.Create((ushort)1));

				Avx.Store(diag1Ptr + rowIndex - (Vector256<T>.Count - 1), localCost);
			}
		}
	}
}
#endif