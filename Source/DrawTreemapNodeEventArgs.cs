using System.Drawing;

namespace cmdwtf.Treemap
{

	/// <summary>
	/// Provides data for the <see cref="TreemapView.DrawNode"/> and
	/// <see cref="TreemapView.DrawBranchNode"/> events.
	/// </summary>
	public class DrawTreemapNodeEventArgs : System.EventArgs
	{
		/// <summary>
		/// Constructs a <see cref="DrawTreemapNodeEventArgs"/ instance.>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="treemapNode"></param>
		/// <param name="boundsF"></param>
		/// <param name="step"></param>
		public DrawTreemapNodeEventArgs(Graphics g, TreemapNode treemapNode, RectangleF boundsF, TreemapNodeDrawStep step)
		{
			Graphics = g;
			Node = treemapNode;
			BoundsF = boundsF;
			Step = step;
		}

		/// <summary>
		/// The <see cref="Graphics"/> instance used to render in the current context.
		/// </summary>
		public Graphics Graphics { get; init; }

		/// <summary>
		/// The node being drawn.
		/// </summary>
		public TreemapNode Node { get; init; }

		/// <summary>
		/// The bounds of the node being drawn, in floating point.
		/// </summary>
		public RectangleF BoundsF { get; init; }

		/// <summary>
		/// The bounds of the node being drawn.
		/// </summary>
		public Rectangle Bounds => Rectangle.Round(BoundsF);

		/// <summary>
		/// The step of the draw process.
		/// </summary>
		public TreemapNodeDrawStep Step { get; init; }

		/// <summary>
		/// If true, the control will draw it's default display.
		/// </summary>
		public bool DrawDefault { get; set; } = false;
	}
}