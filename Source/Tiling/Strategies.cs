using System.Drawing;

namespace cmdwtf.Treemap.Tiling
{
	/// <summary>
	/// A collection of built in <see cref="TreemapView"/> tiling
	/// strategy instances.
	/// </summary>
	public static class Strategies
	{
		/// <summary>
		/// A default tiling strategy. Currently set as <see cref="Squarified"/>.
		/// </summary>
		public static ITilingStrategy<RectangleF> Default => Squarified;

		/// <summary>
		/// A "Squarified" treemap tiling strategy. This is the
		/// defacto standard when it comes to treemapping, and what is
		/// regularly refered when speaking about treemaps generally.
		/// </summary>
		public static ITilingStrategy<RectangleF> Squarified { get; } = new SquarifiedTilingStrategy();

		/// <summary>
		/// A "Slice and Dice" treemap tiling strategy. This will
		/// result in long nodes in one direction, diced up the other
		/// with it's subnodes.
		/// </summary>
		public static ITilingStrategy<RectangleF> SliceAndDice { get; } = new SliceAndDiceTilingStrategy();
	}
}
