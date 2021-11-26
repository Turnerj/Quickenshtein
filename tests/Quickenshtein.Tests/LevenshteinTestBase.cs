using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Quickenshtein.Tests
{
	public abstract class LevenshteinTestBase
	{
		protected CalculationOptions CalculationOptions { get; set; }

		[TestMethod]
		public void Null_FirstArg_1()
		{
			var distance = Levenshtein.GetDistance(null, string.Empty, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void Null_FirstArg_2()
		{
			var distance = Levenshtein.GetDistance(null, "test", CalculationOptions);
			Assert.AreEqual(4, distance);
		}

		[TestMethod]
		public void Null_SecondArg_1()
		{
			var distance = Levenshtein.GetDistance(string.Empty, null, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void Null_SecondArg_2()
		{
			var distance = Levenshtein.GetDistance("test", null, CalculationOptions);
			Assert.AreEqual(4, distance);
		}

		[DataTestMethod]
		[DataRow("", "", DisplayName = "Empty")]
		[DataRow("test", "test", DisplayName = "String with 4 characters")]
		[DataRow("abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz", DisplayName = "Full Alphabet")]
		public void ZeroDistance(string source, string target)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_128_Characters()
		{
			var value = Utilities.BuildString("abcd", 128);
			var distance = Levenshtein.GetDistance(value, value, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_512_Characters()
		{
			var value = Utilities.BuildString("abcd", 512);
			var distance = Levenshtein.GetDistance(value, value, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[DataTestMethod]
		[DataRow("He1lo Wor1d", "Hello World", 2)]
		[DataRow("Hello World", "He1lo Wor1d", 2)]
		[DataRow("He1lo Wor1d", "Hell0 World", 3)]
		[DataRow("Hell0 World", "He1lo Wor1d", 3)]
		public void SplitDifference(string source, string target, int expectedDistance)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(expectedDistance, distance);
		}

		[DataTestMethod]
		[DataRow("", "abcdef", 6)]
		[DataRow("abcdef", "", 6)]
		[DataRow("abcdef", "zyxwvu", 6)]
		public void CompletelyDifferent(string source, string target, int expectedDistance)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(expectedDistance, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_128_Characters()
		{
			var firstArg = Utilities.BuildString("abcd", 128);
			var secondArg = Utilities.BuildString("wxyz", 128);
			var distance = Levenshtein.GetDistance(firstArg, secondArg, CalculationOptions);
			Assert.AreEqual(128, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_512_Characters()
		{
			var firstArg = Utilities.BuildString("abcd", 512);
			var secondArg = Utilities.BuildString("wxyz", 512);
			var distance = Levenshtein.GetDistance(firstArg, secondArg, CalculationOptions);
			Assert.AreEqual(512, distance);
		}

		[TestMethod]
		public void IsCaseSensitive()
		{
			var distance = Levenshtein.GetDistance("Hello World", "hello world", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[DataTestMethod]
		[DataRow("Hello World", "Hello World!", 1)]
		[DataRow("Hello World, this is a string.", "Hello World.", 18)]
		[DataRow("Hello World!", "Hello World", 1)]
		[DataRow("Hello World.", "Hello World, this is a string.", 18)]
		public void Addition(string source, string target, int expectedDistance)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(expectedDistance, distance);
		}

		[DataTestMethod]
		[DataRow("ello World", "Hello World", 1, DisplayName = "Deletion Start (Source)")]
		[DataRow("Hello Worl", "Hello World", 1, DisplayName = "Deletion End (Source)")]
		[DataRow("Hello World", "ello World", 1, DisplayName = "Deletion Start (Target)")]
		[DataRow("Hello World", "Hello Worl", 1, DisplayName = "Deletion End (Target)")]
		[DataRow("Hell World", "Hello World", 1, DisplayName = "Deletion Middle (Source)")]
		[DataRow("Hello World", "Hell World", 1, DisplayName = "Deletion Middle (Target)")]
		public void Deletion(string source, string target, int expectedDistance)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(expectedDistance, distance);
		}

		[TestMethod]
		public void EdgeDifference()
		{
			var distance = Levenshtein.GetDistance(
				$"a{Utilities.BuildString("b", 256 + 7)}c",
				$"y{Utilities.BuildString("b", 256 + 7)}z", 
				CalculationOptions
			);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		[DataRow("bbbbbbbbbbbbbbbbbbbbbbbba", "bbbbbbbbbbbbbbbbbbbbbbbbz", DisplayName = "Trim Start")]
		[DataRow("abbbbbbbbbbbbbbbbbbbbbbbb", "zbbbbbbbbbbbbbbbbbbbbbbbb", DisplayName = "Trim End")]
		public void Trim(string source, string target)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(1, distance);
		}

		[DataTestMethod]
		[DataRow("yorwyeawgn", "xcodeuwtnx", 8)]
		[DataRow("yorwyeagwn", "xcodeuwtnx", 9)]
		[DataRow("yorwyeagnw", "xcodeuwtnx", 9)]
		[DataRow("yorwyaegnw", "xcodeuwtnx", 9)]
		[DataRow("yorwyeawgnb", "xcodeuwtnx", 8)]
		[DataRow("byorwyeawgn", "xcodeuwtnx", 8)]
		[DataRow("yorwyeawgn", "xcodeuwtnxb", 9)]
		[DataRow("yorwyeawgn", "bxcodeuwtnx", 8)]
		[DataRow("yorwyeagwnb", "xcodeuwtnx", 9)]
		[DataRow("yorwyeagnwb", "xcodeuwtnx", 10)]
		[DataRow("yorwyaegnwb", "xcodeuwtnx", 10)]
		[DataRow("abdegjklsjgofmdlacmpdv", "adbegjlkjsogmdaalcmpvd", 10)]
		[DataRow("aaaabbbbccccddddeeee", "aaababbcbccdcddedeee", 5)]
		[DataRow("xkzQJEnvucuhXyKYGqtY", "YTZkcmyTyrvuhDLmswfM", 19)]
		[DataRow("BDLZfcIOFxTwWBdGzZxp", "kDiHMMYqOMHkMTByTuGu", 18)]
		[DataRow("cBFZNfiKhzCtgbyoxqMP", "wwyUZFQsRbyUcozbPrtR", 20)]
		[DataRow("aaaabbbbccffccddddeeee", "aaababbcbcfcdcddedeee", 7)]
		[DataRow("aaaabbbbccfccddddeeee", "aaababbcbcffcdcddedeee", 6)]
		[DataRow("Seven", " of Nine", 7)]
		public void MiscDistances(string source, string target, int expectedDistance)
		{
			var distance = Levenshtein.GetDistance(source, target, CalculationOptions);
			Assert.AreEqual(expectedDistance, distance);
		}
	}
}
