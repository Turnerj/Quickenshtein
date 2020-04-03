using System;
using System.Collections.Generic;
using System.Text;

namespace Quickenshtein
{
	internal unsafe class WorkerState
	{
		public int* RowCountPtr;
		public int WorkerIndex;

		public int ColumnIndex;

		public char* SourcePtr;
		public int SourceLength;
		public char* TargetSegmentPtr;
		public int TargetSegmentLength;

		public int[] BackColumnBoundary;
		public int[] ForwardColumnBoundary;
	}
}
