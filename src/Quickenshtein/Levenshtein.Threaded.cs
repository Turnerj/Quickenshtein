using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;
using Quickenshtein.Internal;
#if NETCOREAPP
using System.Runtime.Intrinsics.X86;
#endif

namespace Quickenshtein
{
	public static partial class Levenshtein
	{
		private static readonly WaitCallback WorkerTask = new WaitCallback(WorkerTask_CalculateSegment);

		private static unsafe int CalculateDistance_MultiThreaded(char* sourcePtr, char* targetPtr, int sourceLength, int targetLength, CalculationOptions options)
		{
			var maximumNumberOfWorkers = Environment.ProcessorCount;
			var numberOfWorkers = targetLength / options.MinimumCharactersPerThread;
			if (numberOfWorkers == 0)
			{
				numberOfWorkers = 1;
			}
			else if (numberOfWorkers > maximumNumberOfWorkers)
			{
				numberOfWorkers = maximumNumberOfWorkers;
			}

			var numberOfColumnsPerWorker = targetLength / numberOfWorkers;
			var remainderColumns = targetLength % numberOfWorkers;

			var rowCountPtr = stackalloc int[Environment.ProcessorCount];
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
				DataHelper.SequentialFill(startBoundaryPtr, 0, sourceLength + 1);
			}

			for (var workerIndex = 0; workerIndex < numberOfWorkers - 1; workerIndex++)
			{
				var columnIndex = workerIndex * numberOfColumnsPerWorker;

				ThreadPool.QueueUserWorkItem(WorkerTask, new WorkerState
				{
					RowCountPtr = rowCountPtr,
					WorkerIndex = workerIndex,
					ColumnIndex = columnIndex,
					SourcePtr = sourcePtr,
					SourceLength = sourceLength,
					TargetSegmentPtr = targetPtr + columnIndex,
					TargetSegmentLength = numberOfColumnsPerWorker,
					BackColumnBoundary = columnBoundariesPool[workerIndex],
					ForwardColumnBoundary = columnBoundariesPool[workerIndex + 1]
				});
			}

			//Run last segment synchronously (ie. in the current thread)
			var lastWorkerIndex = numberOfWorkers - 1;
			var lastWorkerColumnIndex = lastWorkerIndex * numberOfColumnsPerWorker;
			WorkerTask_CalculateSegment(new WorkerState
			{
				RowCountPtr = rowCountPtr,
				WorkerIndex = numberOfWorkers - 1,
				ColumnIndex = (numberOfWorkers - 1) * numberOfColumnsPerWorker,
				SourcePtr = sourcePtr,
				SourceLength = sourceLength,
				TargetSegmentPtr = targetPtr + lastWorkerColumnIndex,
				TargetSegmentLength = numberOfColumnsPerWorker + remainderColumns,
				BackColumnBoundary = columnBoundariesPool[lastWorkerIndex],
				ForwardColumnBoundary = columnBoundariesPool[lastWorkerIndex + 1]
			});

			//Extract last value in forward column boundary of last task (the actual distance)
			var result = columnBoundariesPool[numberOfWorkers][sourceLength];

			//Cleanup
			//Return all column boundaries then the container of boundaries
			for (var i = 0; i < numberOfWorkers + 1; i++)
			{
				ArrayPool<int>.Shared.Return(columnBoundariesPool[i]);
			}
			ArrayPool<int[]>.Shared.Return(columnBoundariesPool);

			return result;
		}

		private static unsafe void WorkerTask_CalculateSegment(object state)
		{
			var workerState = (WorkerState)state;
			var rowCountPtr = workerState.RowCountPtr;
			var workerIndex = workerState.WorkerIndex;
			var columnIndex = workerState.ColumnIndex;
			var sourcePtr = workerState.SourcePtr;
			var sourceLength = workerState.SourceLength;
			var targetSegmentPtr = workerState.TargetSegmentPtr;
			var targetSegmentLength = workerState.TargetSegmentLength;
			var backColumnBoundary = workerState.BackColumnBoundary;
			var forwardColumnBoundary = workerState.ForwardColumnBoundary;

			var pooledArray = ArrayPool<int>.Shared.Rent(targetSegmentLength);

			fixed (int* previousRowPtr = pooledArray)
			{
				DataHelper.SequentialFill(previousRowPtr, columnIndex + 1, targetSegmentLength);

				ref var selfWorkerRowCount = ref rowCountPtr[workerIndex];

				for (var rowIndex = 0; rowIndex < sourceLength;)
				{
					if (workerIndex > 0)
					{
						ref var previousWorkerRowCount = ref rowCountPtr[workerIndex - 1];
						while (Interlocked.CompareExchange(ref previousWorkerRowCount, 0, 0) == rowIndex);
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

				ArrayPool<int>.Shared.Return(pooledArray);
			}
		}
	}
}
