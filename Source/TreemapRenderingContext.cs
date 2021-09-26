
namespace cmdwtf.Treemap
{
	/// <summary>
	/// A small collection of callbacks that a TreemapNode will use to
	/// invoke the proper events on it's owning TreemapView. Also contains
	/// information about how to draw, and properties that make it easy
	/// to directly access that some of that information from the view.
	/// </summary>
	/// <param name="View">The <see cref="TreemapView"/> the nodes drawing belong to.</param>
	public sealed record TreemapRenderingContext(TreemapView View)
	{
		/// <summary>
		/// A delegate representing a draw event being raised.
		/// </summary>
		/// <param name="e">The draw event arguments.</param>
		public delegate void DrawEventRaiseMethod(DrawTreemapNodeEventArgs e);

		/// <summary>
		/// True if the control should draw the node.
		/// </summary>
		public bool ControlDrawNode => View.DrawMode is TreemapViewDrawMode.Normal or TreemapViewDrawMode.OwnerDrawText;

		/// <summary>
		/// True if the control should draw text.
		/// </summary>
		public bool ControlDrawText => View.DrawMode is TreemapViewDrawMode.Normal;

		/// <summary>
		/// True if the control should draw the grid.
		/// </summary>
		public bool ControlDrawGrid => ControlDrawNode;

		/// <summary>
		/// True if the control should draw overlays, such as
		/// the state image (checkbox), plus/minus, etc.
		/// </summary>
		public bool ControlDrawGlyphs => View.DrawMode is TreemapViewDrawMode.Normal;

		/// <summary>
		/// True if owner is drawing.
		/// </summary>
		public bool OwnerDraw => View.DrawMode is not TreemapViewDrawMode.Normal;

		/// <summary>
		/// The node draw event.
		/// </summary>
		public DrawEventRaiseMethod RaiseDrawNode { get; init; } = _drawNop;

		/// <summary>
		/// The header draw event.
		/// </summary>
		public DrawEventRaiseMethod RaiseDrawBranchNode { get; init; } = _drawNop;

		/// <summary>
		/// The grid draw event.
		/// </summary>
		public DrawEventRaiseMethod RaiseDrawGrid { get; init; } = _drawNop;

		/// <summary>
		/// The state glyph draw event.
		/// </summary>
		public DrawEventRaiseMethod RaiseDrawStateGlyph { get; init; } = _drawNop;

		/// <summary>
		/// The plus/minus glyph draw event.
		/// </summary>
		public DrawEventRaiseMethod RaiseDrawPlusMinusGlyph { get; init; } = _drawNop;

		/// <summary>
		/// A collection of variables updated as drawing occurs.
		/// </summary>
		public class CurrentDrawInfo
		{
			/// <summary>
			/// The current node being drawn.
			/// Will be set when <see cref="TreemapNode"/> draw functions are called.
			/// </summary>
			public TreemapNode? Node { get; set; } = null;

			public TreemapNodeDrawStep Step { get; set; } = TreemapNodeDrawStep.Unknown;

			/// <summary>
			/// Clears the properties of <see cref="CurrentDrawInfo"/>
			/// </summary>
			public void Clear()
			{
				Node = null;
				Step = TreemapNodeDrawStep.Unknown;
			}
		}

		public CurrentDrawInfo Current { get; init; } = new();

		/// <summary>
		/// A noop draw operation to be used if any of the raise delegates aren't set.
		/// </summary>
		private static readonly DrawEventRaiseMethod _drawNop = e => { };
	}
}
