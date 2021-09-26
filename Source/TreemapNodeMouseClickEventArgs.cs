
using System.Windows.Forms;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides data for the <see cref="TreemapView.NodeMouseClick"/> and
	/// <see cref="TreemapView.NodeMouseDoubleClick"/> events.
	/// </summary>
	public class TreemapNodeMouseClickEventArgs : MouseEventArgs
	{
		/// <summary>
		/// The node related to the event.
		/// </summary>
		public TreemapNode Node { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNodeMouseClickEventArgs"/> class.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> associated with the event.</param>
		/// <param name="buttons">The <see cref="MouseButtons"/> associated with the event.</param>
		/// <param name="clicks">The number of clicks.</param>
		/// <param name="x">The x coordinate of the click.</param>
		/// <param name="y">The y coordinate of the click.</param>
		public TreemapNodeMouseClickEventArgs(TreemapNode node, MouseButtons buttons, int clicks, int x, int y)
			: base(buttons, clicks, x, y, 0)
		{
			Node = node;
		}
	}
}