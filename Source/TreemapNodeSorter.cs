using System;
using System.Collections;
using System.Collections.Generic;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A class that will sort <see cref="TreemapNode"/>s by the comparison
	/// function provided to it. This class is intended to be overriden. If you
	/// were using this in a prior revision of the control, you now want
	/// to use <see cref="TreemapNodeValueSorter"/>.
	/// </summary>
	public abstract class TreemapNodeSorter : IComparer, IComparer<TreemapNode>
	{
		/// <summary>
		/// If true, will sort in an ascending order (least to greatest)
		/// </summary>
		public bool Ascending { get; set; } = false;

		/// <summary>
		/// If true, will sort in a descending order (greatest to least)
		/// </summary>
		public bool Descending { get => !Ascending; set => Ascending = !value; }

		/// <summary>
		/// A helper to get the positive or negative one for sort direction.
		/// </summary>
		private int AscendingOne => Ascending ? 1 : -1;

		/// <summary>
		/// Creates a new instance of <see cref="TreemapNodeSorter"/>
		/// using a default value retrieval function.
		/// </summary>
		public TreemapNodeSorter()
		{

		}

		/// <summary>
		/// A delegate representing a comparison between two <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="lhs">The left hand side to compare.</param>
		/// <param name="rhs">The right hand side to compare.</param>
		/// <returns>
		/// Less than zero if the left hand has a smaller representation,
		/// Greater than zero if the right hand has a smaller representation.
		/// Zero if both sides are equal.
		/// </returns>
		protected abstract float CompareNodes(TreemapNode? lhs, TreemapNode? rhs);

		/// <summary>
		/// Compares two <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="x">The left hand side to compare.</param>
		/// <param name="y">The right hand side to compare.</param>
		/// <returns>
		/// -1 if the left hand has a smaller value,
		/// 1 if the right hand has a smaller value.
		/// If they are equal, will sort by the number of children nodes.
		/// And if those are equal, will return 0.
		/// The negative one and positive one are inverted if <see cref="Descending"/> is true.
		/// </returns>
		public int Compare(TreemapNode? x, TreemapNode? y)
			=> CompareNodes(x, y) switch
			{
				< 0 => -AscendingOne,
				> 0 => AscendingOne,
				_ => (x?.Nodes.Count - y?.Nodes.Count) switch
				{
					< 0 => -AscendingOne,
					> 0 => AscendingOne,
					_ => 0
				}
			};

		/// <inheritdoc cref="Compare(TreemapNode?, TreemapNode?)"/>
		int IComparer.Compare(object? x, object? y)
		{
			if (x is TreemapNode nodeX && y is TreemapNode nodeY)
			{
				return Compare(nodeX, nodeY);
			}

			throw new ArgumentException($"{nameof(TreemapNodeSorter)} can only sort {nameof(TreemapNode)}s!");
		}
	}
}
