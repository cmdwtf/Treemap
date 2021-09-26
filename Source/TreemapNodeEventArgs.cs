using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Event args for a generic node event.
	/// </summary>
	public class TreemapNodeEventArgs : EventArgs
	{
		/// <summary>
		/// The node the event occurred on.
		/// </summary>
		public TreemapNode Node { get; init; }

		/// <summary>
		/// Creates a new instance of the <see cref="TreemapNodeEventArgs"/> type.
		/// </summary>
		/// <param name="node">The node the event occured on.</param>
		public TreemapNodeEventArgs(TreemapNode node)
		{
			Node = node;
		}
	}
}
