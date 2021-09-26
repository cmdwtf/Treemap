using System;
using System.Collections;
using System.Collections.Generic;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A class that will sort <see cref="TreemapNode"/>s by the difference in their values.
	/// </summary>
	public class TreemapNodeSorter : IComparer, IComparer<TreemapNode>
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
		/// A thunk to get the current value for the <see cref="TreemapNode"/>
		/// </summary>
		private Func<TreemapNode?, float> _valueFunc;

		/// <summary>
		/// Creates a new instance of <see cref="TreemapNodeSorter"/>
		/// using a default value retrieval function.
		/// </summary>
		public TreemapNodeSorter()
		{
			_valueFunc = node => node?.Value ?? 0;
		}

		/// <summary>
		/// Creates a new instance of <see cref="TreemapNodeSorter"/>
		/// using a value retrieval function that uses the given view's mode.
		/// </summary>
		/// <param name="view">The view to use.</param>
		public TreemapNodeSorter(TreemapView view) : this()
		{
			SetView(view);
		}

		/// <summary>
		/// Sets the <see cref="TreemapView"/> used to
		/// get the node's value based on <see cref="TreemapView.ValueMode"/>
		/// </summary>
		/// <param name="view">The view to use.</param>
		public void SetView(TreemapView view)
			=> _valueFunc = node => view.GetNodeValueForCurrentMode(node);

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
			=> (_valueFunc(x) - _valueFunc(y)) switch
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
