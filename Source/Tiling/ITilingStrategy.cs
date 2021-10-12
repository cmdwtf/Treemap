namespace cmdwtf.Treemap.Tiling
{
	/// <summary>
	/// Describes a treemap tiling strategy.
	/// </summary>
	/// <typeparam name="BoundsType"></typeparam>
	public interface ITilingStrategy<BoundsType> where BoundsType : struct
	{
		/// <summary>
		/// Recalculates the bounds of a Treemap, with the
		/// given node as the root within the given bounds.
		/// </summary>
		/// <param name="node">The node to recalculate bounds from.</param>
		/// <param name="newBounds">The bounds this node and all of its children should fit in.</param>
		void RecalculateBounds(TreemapNode node, BoundsType newBounds);
	}
}
