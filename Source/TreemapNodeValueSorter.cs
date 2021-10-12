using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A <see cref="TreemapNode"/> sorter that sorts
	/// by <see cref="TreemapNode.Value"/>, based on the
	/// <see cref="TreemapView"/> given to the sorter's
	/// <see cref="TreemapView.ValueMode"/>.
	/// </summary>
	public class TreemapNodeValueSorter : TreemapNodeSorter
	{
		/// <summary>
		/// A thunk to get the current value for the <see cref="TreemapNode"/>
		/// </summary>
		private Func<TreemapNode?, float> _valueFunc;

		/// <summary>
		/// Creates a new instance of the <see cref="TreemapNodeValueSorter"/> class.
		/// </summary>
		public TreemapNodeValueSorter()
		{
			_valueFunc = node => node?.Value ?? 0;
		}

		/// <summary>
		/// Creates a new instance of <see cref="TreemapNodeValueSorter"/>
		/// using a value retrieval function that uses the given view's mode.
		/// </summary>
		/// <param name="view">The view to use.</param>
		public TreemapNodeValueSorter(TreemapView view)
			: this()
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

		/// <inheritdoc/>
		protected override float CompareNodes(TreemapNode? lhs, TreemapNode? rhs)
			=> _valueFunc(lhs) - _valueFunc(rhs);
	}
}
