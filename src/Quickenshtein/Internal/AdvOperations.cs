using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Quickenshtein.Internal
{
	internal static class AdvOperations
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MathMin(int x, int y)
		{
			return y + ((x - y) & ((x - y) >> (sizeof(int) * 8 - 1)));
		}
	}
}
