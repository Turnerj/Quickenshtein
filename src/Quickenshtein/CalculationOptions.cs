using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein
{
	public struct CalculationOptions
	{
		public bool EnableMultiThreading { get; set; }
		public int MinimumCharactersPerThread { get; set; }

		public readonly static CalculationOptions Default = new CalculationOptions
		{
			EnableMultiThreading = true,

			//TODO: Find appropriate default values
#if NETCOREAPP
			MinimumCharactersPerThread = 8000
#else
			MinimumCharactersPerThread = 4000
#endif
		};
	}
}
