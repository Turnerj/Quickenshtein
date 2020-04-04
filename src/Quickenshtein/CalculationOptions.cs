using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein
{
	public struct CalculationOptions
	{
		public int EnableThreadingAfterXCharacters { get; set; }
		public int MinimumCharactersPerThread { get; set; }

		public readonly static CalculationOptions Default = new CalculationOptions
		{
			EnableThreadingAfterXCharacters = int.MaxValue
		};

		public readonly static CalculationOptions DefaultWithThreading = new CalculationOptions
		{
#if NETCOREAPP
			EnableThreadingAfterXCharacters = 16000,
			MinimumCharactersPerThread = 4000
#else
			EnableThreadingAfterXCharacters = 4000,
			MinimumCharactersPerThread = 1000
#endif
		};
	}
}
