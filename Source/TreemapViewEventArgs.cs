
using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides data for generic <see cref="TreemapView"/> events.
	/// </summary>
	public class TreemapViewEventArgs : EventArgs
	{
		/// <summary>
		/// The node related to the event, if any. Otherwise, null.
		/// </summary>
		public TreemapNode? Node { get; init; }

		/// <summary>
		/// The action related to the event.
		/// </summary>
		public TreemapViewAction Action { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapViewEventArgs"/> class.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> associated with the event, if any.</param>
		/// <param name="action">The action associated with the event.</param>
		public TreemapViewEventArgs(TreemapNode? node, TreemapViewAction action = TreemapViewAction.Unknown)
		{
			Node = node;
			Action = action;
		}
	}
}