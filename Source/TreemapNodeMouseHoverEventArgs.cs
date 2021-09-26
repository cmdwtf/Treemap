
namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides data for the <see cref="TreemapView.NodeMouseHover"/> event
	/// </summary>
	public class TreemapNodeMouseHoverEventArgs : TreemapNodeEventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNodeMouseHoverEventArgs"/> class.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> associated with the event.</param>
		public TreemapNodeMouseHoverEventArgs(TreemapNode node)
			: base(node)
		{

		}
	}
}