using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quickenshtein.Internal;

namespace Quickenshtein.Tests.Internal
{
	[TestClass]
	public class DataHelperTests
	{
		[DataTestMethod]
		[DataRow(71, 1, 71)]
		[DataRow(17, 5, 21)]
		public unsafe void SequentialFill(int arraySize, int startValue, int expectedEndValue)
		{
			var data = new int[arraySize];
			fixed (int* targetPtr = data)
			{
				DataHelper.SequentialFill(targetPtr, startValue, arraySize);
			}

			Assert.AreEqual(startValue, data[0]);
			Assert.AreEqual(expectedEndValue, data[arraySize - 1]);
		}

		[DataTestMethod]
		[DataRow("Hello World - this is a long sentence", "Hello World - this is a long sentence", 37)]
		[DataRow("Hello World - this is a long sentence", "Hello World - this is a longer sentence", 28)]
		[DataRow("", "", 0)]
		[DataRow("a", "b", 0)]
		[DataRow("aaaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaaaaab", 19)]
		[DataRow("aaaaaaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaaab", 16)]
		[DataRow("baaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaa", 0)]
		[DataRow("aaaaaaaaaaaaaaaaaaaab", "aaaaaaaaaaaa", 12)]
		public unsafe void GetIndexOfFirstNonMatchingCharacter(string source, string target, int expectedIndex)
		{
			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				var index = DataHelper.GetIndexOfFirstNonMatchingCharacter(sourcePtr, targetPtr, source.Length, target.Length);
				Assert.AreEqual(expectedIndex, index);
			}
		}

		[DataTestMethod]
		[DataRow("Hello World - this is a long sentence", "Hello World - this is a long sentence", 0, 0)]
		[DataRow("Hello World - this is a long sentence", "Hello World - this is a longer sentence", 28, 30)]
		[DataRow("World", "World", 0, 0)]
		[DataRow("", "", 0, 0)]
		[DataRow("a", "b", 1, 1)]
		[DataRow("aaaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaaaaab", 20, 20)]
		[DataRow("aaaaaaaaaaaaaaaaaaab", "aaaaaaaaaaaaaaaab", 3, 0)]
		[DataRow("baaaaaaaaaaaaaaaaaaa", "aaaaaaaaaaaaaaaa", 4, 0)]
		[DataRow("aaaaaaaaaaaaaaaaaaaab", "aaaaaaaaaaaa", 21, 12)]
		public unsafe void GetIndexesOfLastNonMatchingCharacters(string source, string target, int expectedSourceEnd, int expectedTargetEnd)
		{
			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				DataHelper.GetIndexesOfLastNonMatchingCharacters(sourcePtr, targetPtr, 0, source.Length, target.Length, out var sourceEnd, out var targetEnd);
				Assert.AreEqual(expectedSourceEnd, sourceEnd);
				Assert.AreEqual(expectedTargetEnd, targetEnd);
			}
		}
	}
}
