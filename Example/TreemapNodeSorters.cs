using System;
using System.Collections;
using System.Collections.Generic;

using cmdwtf.Treemap;

namespace Example
{
	public static class TreemapNodeSorters
	{
		public abstract class BaseNodeSorter : IComparer, IComparer<TreemapNode>
		{
			protected BaseNodeSorter() { }

			public abstract int Compare(TreemapNode x, TreemapNode y);

			int IComparer.Compare(object x, object y)
			{
				if (x is TreemapNode nodeX && y is TreemapNode nodeY)
				{
					return Compare(nodeX, nodeY);
				}

				throw new ArgumentException($"{nameof(TreemapNodeSorter)} can only sort {nameof(TreemapNode)}s!");
			}
		}

		public class UnsortedSorter : BaseNodeSorter
		{
			public override int Compare(TreemapNode x, TreemapNode y)
				=> 0;
		}

		public class RandomNodeSorter : BaseNodeSorter
		{
			private readonly Random _random = new(1234);
			public override int Compare(TreemapNode x, TreemapNode y)
				=> _random.Next(3) switch
				{
					0 => 0,
					1 => 1,
					2 => -1,
					_ => 0,
				};
		}

		public static UnsortedSorter Unsorted { get; } = new UnsortedSorter();
		public static TreemapNodeValueSorter Ascending { get; } = new TreemapNodeValueSorter() { Ascending = true };
		public static TreemapNodeValueSorter Descending { get; } = new TreemapNodeValueSorter() { Descending = true };
		public static TreemapNodeNameSorter AscendingName { get; } = new TreemapNodeNameSorter() { Ascending = true };
		public static TreemapNodeNameSorter DescendingName { get; } = new TreemapNodeNameSorter() { Descending = true };
		public static RandomNodeSorter Random { get; } = new RandomNodeSorter();
	}
}
