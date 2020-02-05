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
		private static unsafe void CalculateDistance_Sse3(char sourcePrevChar, ushort* targetPtr, int targetLength, ref int lastInsertionCost, ref int lastSubstitutionCost, int* previousRowPtr, ref int columnIndex)
		{
			var sourcePrevCharVector = Vector128.Create(sourcePrevChar);

			int deletionCost;
			int minOfInsertOrDelete;
			int localSubstitutionCost;

			while (targetLength >= 8)
			{
				targetLength -= 8;

				var targetCharVector = Sse3.LoadDquVector128(targetPtr + columnIndex);
				var charEqualityVector = Sse2.CompareEqual(sourcePrevCharVector, targetCharVector);

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
			}
		}
	}
}
#endif