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
		/// A root node always knows where it's parent is.
		/// </summary>
		/// <returns>The <see cref="TreemapView"/> that this root node belongs to.</returns>
		internal override TreemapView? FindTreemapView() => ParentView;

		/// <summary>
		/// A root node doesn't have parts like a regular node does,
		/// so it will just return that any hit test just hit
		/// it as <see cref="TreemapViewHitTestLocations.Node"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		internal override TreemapViewHitTestLocations GetHitTestLocation(int x, int y)
			=> TreemapViewHitTestLocations.Node;

		/// <summary>
		/// Cloning root nodes is not supported.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// Not supported.
		/// </exception>
		public override object Clone() => throw new NotSupportedException();
	}
}
