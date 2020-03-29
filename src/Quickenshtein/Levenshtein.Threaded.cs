using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
#if NETCOREAPP
using System.Runtime.Intrinsics.X86;
#endif

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private static unsafe int CalculateDistance2(ReadOnlySpan<char> source, ReadOnlySpan<char> target)
		{
			var sourceLength = source.Length;
			var targetLength = target.Length;

			fixed (char* sourcePtr = source)
			fixed (char* targetPtr = target)
			{
				return CalculateInParallel(sourcePtr, sourceLength, targetPtr, targetLength);
			}
		}

		private static unsafe int CalculateInParallel(char* sourcePtr, int sourceLength, char* targetPtr, int targetLength)
		{
			var numberOfWorkers = Environment.ProcessorCount;
			if (numberOfWorkers > targetLength)
			{
				numberOfWorkers = targetLength;
			}

			var numberOfColumnsPerWorker = targetLength / numberOfWorkers;
			var remainderColumns = targetLength % numberOfWorkers;

			var workerRowCountPool = ArrayPool<int>.Shared.Rent(numberOfWorkers);
			((Span<int>)workerRowCountPool).Fill(0);

			var workerPool = ArrayPool<Task>.Shared.Rent(numberOfWorkers);
			var columnBoundariesPool = ArrayPool<int[]>.Shared.Rent(numberOfWorkers + 1);

			//Initialise shared task boundaries
			for (var i = 0; i < numberOfWorkers + 1; i++)
			{
				columnBoundariesPool[i] = ArrayPool<int>.Shared.Rent(sourceLength + 1);
				columnBoundariesPool[i][0] = i * numberOfColumnsPerWorker;
			}
			columnBoundariesPool[numberOfWorkers][0] += remainderColumns;

			//Fill first column boundary (ColumnIndex = 0) with incrementing numbers
			fixed (int* startBoundaryPtr = columnBoundariesPool[0])
			{
				Fill_Custom(startBoundaryPtr, -1, sourceLength + 1);
			}

			for (var i = 0; i < numberOfWorkers; i++)
			{
				var workerIndex = i;
				workerPool[workerIndex] = Task.Run(() =>
				{
					var backColumnBoundary = columnBoundariesPool[workerIndex];
					var forwardColumnBoundary = columnBoundariesPool[workerIndex + 1];
					var columnIndex = workerIndex * numberOfColumnsPerWorker;
					var targetSegmentPtr = targetPtr + columnIndex;
					var targetSegmentLength = numberOfColumnsPerWorker;

					if (workerIndex + 1 == numberOfWorkers)
					{
						targetSegmentLength += remainderColumns;
					}

					CalculateSegment(workerRowCountPool, workerIndex, columnIndex, sourcePtr, sourceLength, targetSegmentPtr, targetSegmentLength, backColumnBoundary, forwardColumnBoundary);
				});
			}
			var finalWorker = workerPool[numberOfWorkers - 1];
			finalWorker.Wait();

			//Extract last value in forward column boundary of last task (the actual distance)
			var result = columnBoundariesPool[numberOfWorkers][sourceLength];

			//Cleanup
			//Return all column boundaries then the container of boundaries
			for (var i = 0; i < numberOfWorkers + 1; i++)
			{
				ArrayPool<int>.Shared.Return(columnBoundariesPool[i]);
			}
			ArrayPool<int[]>.Shared.Return(columnBoundariesPool);

			ArrayPool<Task>.Shared.Return(workerPool);
			ArrayPool<int>.Shared.Return(workerRowCountPool);

			return result;
		}

		private static unsafe Task CalculateSegment(int[] workerRowCount, int workerIndex, int columnIndex, char* sourcePtr, int sourceLength, char* targetSegmentPtr, int targetSegmentLength, int[] backColumnBoundary, int[] forwardColumnBoundary)
		{
			var arrayPool = ArrayPool<int>.Shared;
			var pooledArray = arrayPool.Rent(targetSegmentLength);

			fixed (int* previousRowPtr = pooledArray)
			{
				//TODO: Support intrinsics for FillRow_Custom
				Fill_Custom(previousRowPtr, columnIndex, targetSegmentLength);

				ref var selfWorkerRowCount = ref workerRowCount[workerIndex];

				for (var rowIndex = 0; rowIndex < sourceLength;)
				{
					if (workerIndex > 0)
					{
						ref var previousWorkerRowCount = ref workerRowCount[workerIndex - 1];
						while (Interlocked.CompareExchange(ref previousWorkerRowCount, 0, 0) == rowIndex)
						{
							//No-op :(
						}
					}

					var lastSubstitutionCost = backColumnBoundary[rowIndex];
					var lastInsertionCost = backColumnBoundary[rowIndex + 1];

					var sourcePrevChar = sourcePtr[rowIndex];

#if NETCOREAPP
					if (Sse41.IsSupported)
					{
						CalculateRow_Sse41(previousRowPtr, targetSegmentPtr, targetSegmentLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
					else
					{
						CalculateRow(previousRowPtr, targetSegmentPtr, targetSegmentLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
					}
#else
					CalculateRow(previousRowPtr, targetSegmentPtr, targetSegmentLength, sourcePrevChar, lastInsertionCost, lastSubstitutionCost);
#endif

					forwardColumnBoundary[++rowIndex] = previousRowPtr[targetSegmentLength - 1];
					Interlocked.Increment(ref selfWorkerRowCount);
				}

				arrayPool.Return(pooledArray);
				return Task.CompletedTask;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static unsafe void Fill_Custom(int* arrayPtr, int startAfterValue, int columns)
		{
			var index = 0;
			var value = startAfterValue;

			while (columns >= 8)
			{
				columns -= 8;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
			}

			if (columns > 4)
			{
				columns -= 4;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
				arrayPtr[index++] = ++value;
			}

			while (columns > 0)
			{
				columns--;
				arrayPtr[index++] = ++value;
			}
		}
	}
}
