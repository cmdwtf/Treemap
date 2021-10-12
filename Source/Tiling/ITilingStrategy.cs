namespace cmdwtf.Treemap.Tiling
{
	public interface ITilingStrategy<BoundsType> where BoundsType : struct
	{
		void RecalculateBounds(TreemapNode node, BoundsType newBounds);
	}
}
