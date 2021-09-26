namespace cmdwtf.Treemap
{
	public partial class TreemapNode
	{
		/// <summary>
		/// An enum representing which orientation the current treemap node should be laying out.
		/// This is used internally to layout the node boxes.
		/// </summary>
		private enum RowOrientation
		{
			Horizontal,
			Vertical
		}
	}
}
