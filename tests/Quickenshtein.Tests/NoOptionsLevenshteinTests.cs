using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Quickenshtein.Tests
{
	[TestClass]
	public class NoOptionsLevenshteinTests
	{
#if NET472
		[TestMethod]
		public void GetDistanceFromString()
		{
			var distance = Levenshtein.GetDistance("test", string.Empty);
			Assert.AreEqual(4, distance);
		}
#endif

		[TestMethod]
		public void GetDistanceFromReadOnlySpan()
		{
			var distance = Levenshtein.GetDistance("test".AsSpan(), string.Empty.AsSpan());
			Assert.AreEqual(4, distance);
		}
	}
}
