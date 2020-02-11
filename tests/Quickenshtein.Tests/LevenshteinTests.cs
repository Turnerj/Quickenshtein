using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Quickenshtein.Tests
{
	[TestClass]
	public class LevenshteinTests
	{
#if NET472
		[TestMethod]
		public void Null_FirstArg()
		{
			var distance = Levenshtein.GetDistance(null, "test");
			Assert.AreEqual(4, distance);
		}

		[TestMethod]
		public void Null_SecondArg()
		{
			var distance = Levenshtein.GetDistance("test", null);
			Assert.AreEqual(4, distance);
		}
#endif

		[TestMethod]
		public void ZeroDistance_EmptyString()
		{
			var distance = Levenshtein.GetDistance(string.Empty, string.Empty);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_NonEmptyString()
		{
			var distance = Levenshtein.GetDistance("test", "test");
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_128_Characters()
		{
			var value = Utilities.BuildString("abcd", 128);
			var distance = Levenshtein.GetDistance(value, value);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void ZeroDistance_512_Characters()
		{
			var value = Utilities.BuildString("abcd", 512);
			var distance = Levenshtein.GetDistance(value, value);
			Assert.AreEqual(0, distance);
		}

		[TestMethod]
		public void OneDistance()
		{
			var distance = Levenshtein.GetDistance("te-st", "test");
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void OneDistance_Reversed()
		{
			var distance = Levenshtein.GetDistance("test", "te-st");
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void TwoDistance()
		{
			var distance = Levenshtein.GetDistance("Hello World", "He11o World");
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Reversed()
		{
			var distance = Levenshtein.GetDistance("He11o World", "Hello World");
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Split()
		{
			var distance = Levenshtein.GetDistance("Hello World", "He1lo Wor1d");
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void TwoDistance_Split_Reversed()
		{
			var distance = Levenshtein.GetDistance("He1lo Wor1d", "Hello World");
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_OneEmpty()
		{
			var distance = Levenshtein.GetDistance(string.Empty, "abcdef");
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_OneEmpty_Reversed()
		{
			var distance = Levenshtein.GetDistance("abcdef", string.Empty);
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_NonEmpty()
		{
			var distance = Levenshtein.GetDistance("abcdef", "zyxwvu");
			Assert.AreEqual(6, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_128_Characters()
		{
			var firstArg = Utilities.BuildString("abcd", 128);
			var secondArg = Utilities.BuildString("wxyz", 128);
			var distance = Levenshtein.GetDistance(firstArg, secondArg);
			Assert.AreEqual(128, distance);
		}

		[TestMethod]
		public void CompletelyDifferent_512_Characters()
		{
			var firstArg = Utilities.BuildString("abcd", 512);
			var secondArg = Utilities.BuildString("wxyz", 512);
			var distance = Levenshtein.GetDistance(firstArg, secondArg);
			Assert.AreEqual(512, distance);
		}

		[TestMethod]
		public void Deletion_Start()
		{
			var distance = Levenshtein.GetDistance("Hello World", "ello World");
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void Deletion_End()
		{
			var distance = Levenshtein.GetDistance("Hello World", "Hello Worl");
			Assert.AreEqual(1, distance);
		}

		[TestMethod]
		public void IsCaseSensitive()
		{
			var distance = Levenshtein.GetDistance("Hello World", "hello world");
			Assert.AreEqual(2, distance);
		}

		[TestMethod]
		public void Shorter_Target()
		{
			var distance = Levenshtein.GetDistance("This is a longer sentence.", "This is shorter.");
			Assert.AreEqual(13, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_1()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "xcodeuwtnx");
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_2()
		{
			var distance = Levenshtein.GetDistance("yorwyeagwn", "xcodeuwtnx");
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_3()
		{
			var distance = Levenshtein.GetDistance("yorwyeagnw", "xcodeuwtnx");
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_EqualLength_4()
		{
			var distance = Levenshtein.GetDistance("yorwyaegnw", "xcodeuwtnx");
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1a()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgnb", "xcodeuwtnx");
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1b()
		{
			var distance = Levenshtein.GetDistance("byorwyeawgn", "xcodeuwtnx");
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1c()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "xcodeuwtnxb");
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_1d()
		{
			var distance = Levenshtein.GetDistance("yorwyeawgn", "bxcodeuwtnx");
			Assert.AreEqual(8, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_2()
		{
			var distance = Levenshtein.GetDistance("yorwyeagwnb", "xcodeuwtnx");
			Assert.AreEqual(9, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_3()
		{
			var distance = Levenshtein.GetDistance("yorwyeagnwb", "xcodeuwtnx");
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void TranspositionHandling_UnEqualLength_4()
		{
			var distance = Levenshtein.GetDistance("yorwyaegnwb", "xcodeuwtnx");
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_1()
		{
			var distance = Levenshtein.GetDistance("abdegjklsjgofmdlacmpdv", "adbegjlkjsogmdaalcmpvd");
			Assert.AreEqual(10, distance);
		}

		[TestMethod]
		public void AdditionalTest_EqualLength_2()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccccddddeeee", "aaababbcbccdcddedeee");
			Assert.AreEqual(5, distance);
		}

		[TestMethod]
		public void AdditionalTest_UnEqualLength_1()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccffccddddeeee", "aaababbcbcfcdcddedeee");
			Assert.AreEqual(7, distance);
		}

		[TestMethod]
		public void AdditionalTest_UnEqualLength_2()
		{
			var distance = Levenshtein.GetDistance("aaaabbbbccfccddddeeee", "aaababbcbcffcdcddedeee");
			Assert.AreEqual(6, distance);
		}
	}
}
