using System.ComponentModel;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides data for listener cancelable <see cref="TreemapView"/>
	/// and <see cref="TreemapNode"/> events.
	/// </summary>
	public class TreemapViewCancelEventArgs : CancelEventArgs
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
		/// Initializes a new instance of the <see cref="TreemapViewCancelEventArgs"/> class.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> associated with the event, if any.</param>
		/// <param name="action">The action associated with the event.</param>
		public TreemapViewCancelEventArgs(TreemapNode? node, TreemapViewAction action = TreemapViewAction.Unknown)
			: this(node, false, action)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapViewCancelEventArgs"/> class.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> associated with the event, if any.</param>
		/// <param name="cancel">The default cancelation state, with true represnting the event should be cancelled.</param>
		/// <param name="action">The action associated with the event.</param>
		public TreemapViewCancelEventArgs(TreemapNode? node, bool cancel, TreemapViewAction action = TreemapViewAction.Unknown)
			: base(cancel)
		{
			Node = node;
			Action = action;
		}
	}
}