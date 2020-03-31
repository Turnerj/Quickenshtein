using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Quickenshtein.Tests
{
	public abstract class LevenshteinTestBase
	{
		protected CalculationOptions CalculationOptions { get; set; }

#if NET472
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
#endif

		[TestMethod]
		public void ZeroDistance_EmptyString()
		{
			var distance = Levenshtein.GetDistance(string.Empty, string.Empty, CalculationOptions);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_NonEmptyString()
		{
			var distance = Levenshtein.GetDistance("test", "test", CalculationOptions);
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

		[TestMethod]
		public void OneDistance()
		{
			var distance = Levenshtein.GetDistance("te-st", "test", CalculationOptions);
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void OneDistance_Reversed()
		{
			var distance = Levenshtein.GetDistance("test", "te-st", CalculationOptions);
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void TwoDistance()
		{
			var distance = Levenshtein.GetDistance("Hello World", "He11o World", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Reversed()
		{
			var distance = Levenshtein.GetDistance("He11o World", "Hello World", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Split()
		{
			var distance = Levenshtein.GetDistance("Hello World", "He1lo Wor1d", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Split_Reversed()
		{
			var distance = Levenshtein.GetDistance("He1lo Wor1d", "Hello World", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_OneEmpty()
		{
			var distance = Levenshtein.GetDistance(string.Empty, "abcdef", CalculationOptions);
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_OneEmpty_Reversed()
		{
			var distance = Levenshtein.GetDistance("abcdef", string.Empty, CalculationOptions);
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_NonEmpty()
		{
			var distance = Levenshtein.GetDistance("abcdef", "zyxwvu", CalculationOptions);
			Assert.AreEqual(6, distance);
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
		public void Deletion_Start()
		{
			var distance = Levenshtein.GetDistance("Hello World", "ello World", CalculationOptions);
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void Deletion_End()
		{
			var distance = Levenshtein.GetDistance("Hello World", "Hello Worl", CalculationOptions);
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void IsCaseSensitive()
		{
			var distance = Levenshtein.GetDistance("Hello World", "hello world", CalculationOptions);
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void Shorter_Target()
		{
			var distance = Levenshtein.GetDistance("This is a longer sentence.", "This is shorter.", CalculationOptions);
			Assert.AreEqual(13, distance);
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
		public void TranspositionHandling_EqualLength_1()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_2()
		{
			var distance = Levenshtein.GetDistance("yorwyeagwn", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_3()
		{
			var distance = Levenshtein.GetDistance("yorwyeagnw", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_4()
		{
			var distance = Levenshtein.GetDistance("yorwyaegnw", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1a()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgnb", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1b()
		{
			var distance = Levenshtein.GetDistance("byorwyeawgn", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1c()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "xcodeuwtnxb", CalculationOptions);
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1d()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "bxcodeuwtnx", CalculationOptions);
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_2()
		{
			var distance = Levenshtein.GetDistance("yorwyeagwnb", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_3()
		{
			var distance = Levenshtein.GetDistance("yorwyeagnwb", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_4()
		{
			var distance = Levenshtein.GetDistance("yorwyaegnwb", "xcodeuwtnx", CalculationOptions);
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_1()
		{
			var distance = Levenshtein.GetDistance("abdegjklsjgofmdlacmpdv", "adbegjlkjsogmdaalcmpvd", CalculationOptions);
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_2()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccccddddeeee", "aaababbcbccdcddedeee", CalculationOptions);
			Assert.AreEqual(5, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_3()
		{
			var distance = Levenshtein.GetDistance("xkzQJEnvucuhXyKYGqtY", "YTZkcmyTyrvuhDLmswfM", CalculationOptions);
			Assert.AreEqual(19, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_4()
		{
			var distance = Levenshtein.GetDistance("BDLZfcIOFxTwWBdGzZxp", "kDiHMMYqOMHkMTByTuGu", CalculationOptions);
			Assert.AreEqual(18, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_5()
		{
			var distance = Levenshtein.GetDistance("cBFZNfiKhzCtgbyoxqMP", "wwyUZFQsRbyUcozbPrtR", CalculationOptions);
			Assert.AreEqual(20, distance);
		}

		[TestMethod]
		public void AdditionalTest_UnEqualLength_1()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccffccddddeeee", "aaababbcbcfcdcddedeee", CalculationOptions);
			Assert.AreEqual(7, distance);
		}

		[TestMethod]
		public void AdditionalTest_UnEqualLength_2()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccfccddddeeee", "aaababbcbcffcdcddedeee", CalculationOptions);
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void TrimEnd()
		{
			var distance = Levenshtein.GetDistance("abbbbbbbbbbbbbbbbbbbbbbbb", "zbbbbbbbbbbbbbbbbbbbbbbbb", CalculationOptions);
			Assert.AreEqual(1, distance);
		}
	}
}
