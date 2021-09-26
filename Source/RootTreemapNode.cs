using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A specialized treemap node that represents the root of a tree.
	/// </summary>
	internal sealed class RootTreemapNode : TreemapNode
	{
		/// <summary>
		/// Similar to <see cref="TreemapNode.TreemapView"/>,
		/// but guaranteed to not be null since a root node
		/// always belongs to a <see cref="TreemapView"/>.
		/// </summary>
		public TreemapView ParentView { get; private set; }

		/// <summary>
		/// Creates a new <see cref="RootTreemapNode"/> instance.
		/// </summary>
		/// <param name="parentView">The parent view this node belongs to.</param>
		public RootTreemapNode(TreemapView parentView)
		{
			_parentView = parentView;
			ParentView = parentView;
		}

		/// <summary>
		/// Cloning root nodes is not supported.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// Not supported.
		/// </exception>
		public override object Clone() => throw new NotSupportedException();
	}
}
