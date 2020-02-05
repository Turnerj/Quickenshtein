#if NETCOREAPP3_0
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Quickenshtein
{
	public static partial class Levenshtein
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void CalculateDistance_Avx2(char sourcePrevChar, ushort* targetPtr, int targetLength, ref int lastInsertionCost, ref int lastSubstitutionCost, int* previousRowPtr, ref int columnIndex)
		{
			var sourcePrevCharVector = Vector256.Create(sourcePrevChar);

			int deletionCost;
			int minOfInsertOrDelete;
			int localSubstitutionCost;

			while (targetLength >= 16)
			{
				targetLength -= 16;

				var targetCharVector = Avx.LoadDquVector256(targetPtr + columnIndex);
				var charEqualityVector = Avx2.CompareEqual(sourcePrevCharVector, targetCharVector);

				//TODO: Actually have a benchmark that can make use of this
				//		The current benchmark will never have 16 of the same characters in a row
				//Optimization for strings with equal characters in the middle
				//var equalityMask = (uint)Avx.MoveMask(charEqualityVector.AsSingle());
				//var numberOfMatches = Popcnt.PopCount(equalityMask);
				//if (numberOfMatches == 16)
				//{
				//	lastInsertionCost = lastSubstitutionCost;
				//	lastSubstitutionCost = previousRow[columnIndex + 16];
				//	var lastInsertVector = Vector256.Create(lastInsertionCost);
				//	Avx.Store(previousRowPtr + columnIndex, lastInsertVector);
				//	columnIndex += 16;
				//	continue;
				//}

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(0) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(1) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(2) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(3) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(4) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(5) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(6) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(7) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(8) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(9) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(10) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(11) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(12) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(13) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(14) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;

				deletionCost = previousRowPtr[columnIndex];
				minOfInsertOrDelete = Math.Min(lastInsertionCost, deletionCost) + 1;
				localSubstitutionCost = lastSubstitutionCost + (~charEqualityVector.GetElement(15) & 1);
				lastInsertionCost = Math.Min(minOfInsertOrDelete, localSubstitutionCost);
				lastSubstitutionCost = deletionCost;
				previousRowPtr[columnIndex++] = lastInsertionCost;
			}
		}
	}
}
#endif