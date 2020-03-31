using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quickenshtein.Tests
{
	[TestClass]
	public class ThreadedLevenshteinTests : LevenshteinTestBase
	{
		public ThreadedLevenshteinTests()
		{
			CalculationOptions = new CalculationOptions
			{
				EnableThreadingAfterXCharacters = 0,
				MinimumCharactersPerThread = 16
			};
		}
	}
}
