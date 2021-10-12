using System.Drawing;

namespace cmdwtf.Treemap.Tiling
{
	public static class Strategies
	{
		public static ITilingStrategy<RectangleF> Squarified { get; } = new SquarifiedTilingStrategy();
	}
}
