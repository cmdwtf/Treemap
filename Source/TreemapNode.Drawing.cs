using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Drawing;
using static cmdwtf.Toolkit.WinForms.Forms;

using TreemapNodeValueDataType = System.Single;
//using VSI = System.Windows.Forms.VisualStyles.VisualStyleInformation;
//using VSS = System.Windows.Forms.VisualStyles.VisualStyleState;

namespace cmdwtf.Treemap
{
	public partial class TreemapNode
	{
		#region Override Appearance Properties

		/// <summary>
		/// Gets or sets the foreground color of the <see cref="TreemapNode"/>.
		/// This color is used when drawing a leaf node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of the <see cref="TreemapNode"/>.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? ForeColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the hot tracking foreground color of the <see cref="TreemapNode"/>.
		/// This color is used when drawing a hot tracked leaf node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The hot track foreground <see cref="Color"/> of the <see cref="TreemapNode"/>.
		/// The default value is null.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? HotTrackForeColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the background color of the <see cref="TreemapNode"/>.
		/// This color is used when drawing a leaf node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The background <see cref="Color"/> of the <see cref="TreemapNode"/>.
		/// The default is null.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? BackColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the foreground color of the <see cref="TreemapNode"/>
		/// This color is used when drawing a branch node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of the <see cref="TreemapNode"/> branch as a header.
		/// The default is null.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? BranchForeColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the hot tracking foreground color of the <see cref="TreemapNode"/>
		/// This color is used when drawing a branch node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of the <see cref="TreemapNode"/> branch as a header.
		/// The default is null.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? BranchHotTrackForeColor { get; set; } = null;

		/// <summary>
		/// Gets or sets the background color of the <see cref="TreemapNode"/>
		/// This color is used when drawing a branch node.
		/// If null, will inherit the value from the owning view.
		/// </summary>
		/// <returns>
		/// The background <see cref="Color"/> of the <see cref="TreemapNode"/>.
		/// The default is null.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public Color? BranchBackColor { get; set; } = null;

		/// <summary>
		/// The height (in pixels) of the header section of a branch node.
		/// If the height is 0, the header drawing will be skipped.
		/// If the height is null, will inherit the header height from the owning view.
		/// </summary>
		/// <returns>
		/// The height (in pixels) of the header section of the node, or null if not specified.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public int? BranchHeight { get; set; } = null;

		/// <summary>
		/// Gets or sets the font that is used to draw the <see cref="TreemapNode"/> label.
		/// This value is provided for compatibility with <see cref="TreeView"/>,
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		[Localizable(true)]
		public Font? NodeFont
		{
			[Obsolete($"Use {nameof(NodeLeafFont)} or {nameof(NodeBranchFont)} instead. This will return {nameof(NodeLeafFont)}'s value.")]
			get => NodeLeafFont;
			set
			{
				NodeLeafFont = value;
				NodeBranchFont = value;
			}
		}

		/// <summary>
		/// Gets or sets the font that is used to display the text on the <see cref="TreemapNode"/> label.
		/// If it is null, will fall back to using the <see cref="TreemapView.NodeLeafFont"/>
		/// </summary>
		/// <returns>
		/// The <see cref="Font"/> that is used to display the text on the <see cref="TreemapNode"/> label.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		[Localizable(true)]
		public Font? NodeLeafFont { get; set; } = null;

		/// <summary>
		/// Gets or sets the font that is used to display the text on the <see cref="TreemapNode"/> label.
		/// If it is null, will fall back to using the <see cref="TreemapView.NodeBranchFont"/>
		/// </summary>
		/// <returns>
		/// The <see cref="Font"/> that is used to display the text on the <see cref="TreemapNode"/> label.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		[Localizable(true)]
		public Font? NodeBranchFont { get; set; } = null;

		/// <summary>
		/// Gets or sets the alignment of an image that is displayed in the control.
		/// </summary>
		/// <returns>
		/// One of the <see cref="ContentAlignment"/> values. The default is <see cref="ContentAlignment.MiddleCenter"/>
		/// </returns>
		/// <exception cref="InvalidEnumArgumentException">
		/// The value assigned is not one of the <see cref="ContentAlignment"/> values.
		/// </exception>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		[Localizable(true)]
		public ContentAlignment? ImageAlign { get; set; } = null;

		/// <summary>
		/// Gets or sets the style the node is drawn if it's a leaf mode.
		/// If the value is null, will fall back to using the <see cref="TreemapView.NodeLeafDrawStyle"/>
		/// </summary>
		/// <returns>
		/// One of the <see cref="TreemapNodeDrawStyle"/> values. The default is
		/// <see cref="TreemapNodeDrawStyle.Gradient"/>.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public TreemapNodeDrawStyle? LeafDrawStyle { get; set; } = null;

		/// <summary>
		/// A <see cref="System.Windows.Forms.Padding"/> to control how deflated a leaf node is drawn.
		/// If it is null, will fall back to using the <see cref="TreemapView.NodeLeafPadding"/> value.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public Padding? LeafPadding { get; set; } = null;

		/// <summary>
		/// A <see cref="System.Windows.Forms.Padding"/> to control how deflated a branch node is drawn.
		/// If it is null, will fall back to using the <see cref="TreemapView.NodeBranchMargin"/> value.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public Padding? BranchMargin { get; set; } = null;

		/// <summary>
		/// Gets or sets the distance to indent each child <see cref="TreemapNode"/> level. This is treated
		/// as equidistant padding on all sides, and directly sets <see cref="LeafPadding"/>.
		/// </summary>
		/// <returns>
		/// The distance, in pixels, to indent each child <see cref="TreemapNode"/> level.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public int? Indent
		{
			get => LeafPadding?.All ?? null;
			set => LeafPadding = value.HasValue ? new(value.Value) : null;
		}

		/// <summary>
		/// Gets or sets the image that is displayed on a <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="System.Drawing.Image"/> displayed on the <see cref="TreemapNode"/>. The default
		/// is null.
		/// </returns>
		[Localizable(true)]
		[Category(Categories.Appearance)]
		public Image? Image { get; set; } = null;

		/// <summary>
		/// If true, and if the node is a branch, it will be drawn as a header. If it is null,
		/// will fall back to using the <see cref="TreemapView.ShowBranchesAsHeaders"/> value.
		/// </summary>
		/// <returns>
		/// true, if the node should draw itself as a header, if it is a branch node. The default
		/// is null.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		public bool? ShowBranchHeader { get; set; } = null;

		#endregion Override Appearance Properties

		/// <summary>
		/// A <see cref="StringFormat"/> to control how the a branch node text is rendered.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public StringFormat? BranchStringFormat { get; set; } = new StringFormat(StringFormat.GenericTypographic)
			.SetAlignments(ContentAlignment.MiddleCenter);

		/// <summary>
		/// A <see cref="StringFormat"/> to control how the node leaf text is rendered.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public StringFormat? LeafStringFormat { get; set; } = new StringFormat(StringFormat.GenericTypographic)
			.SetAlignments(ContentAlignment.MiddleCenter);

		#region Calculated Layout Properties

		/// <summary>
		/// The entire 'empty' area of the node, calculated and used in layout.
		/// </summary>
		private RectangleF EmptyArea { get; set; } = RectangleF.Empty;

		/// <summary>
		/// The area of the node which is occupied by the image, if any.
		/// </summary>
		internal RectangleF ImageArea { get; set; } = RectangleF.Empty;

		/// <summary>
		/// The area of the node which is occupied by the checkbox, if any.
		/// </summary>
		internal RectangleF CheckboxArea { get; set; } = RectangleF.Empty;

		/// <summary>
		/// The area of the node which is occupied by the [+]/[-] box, if any.
		/// </summary>
		internal RectangleF PlusMinusArea { get; set; } = RectangleF.Empty;

		#endregion Calculated Layout Properties

		#region Drawing

		/// <summary>
		/// Draws the background of the <see cref="TreemapNode"/>
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		internal void DrawBackground(Graphics graphics, TreemapRenderingContext ctx)
		{
			// set up the current info
			ctx.Current.Node = this;
			ctx.Current.Step = TreemapNodeDrawStep.Background;

			DrawAll(graphics, ctx);

			ctx.Current.Clear();
		}

		/// <summary>
		/// Draws the foreground of the <see cref="TreemapNode"/>
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		internal void DrawForeground(Graphics graphics, TreemapRenderingContext ctx)
		{
			// set up the current info
			ctx.Current.Node = this;
			ctx.Current.Step = TreemapNodeDrawStep.Foreground;

			DrawAll(graphics, ctx);

			ctx.Current.Step = TreemapNodeDrawStep.Overlay;

			DrawAll(graphics, ctx);

			ctx.Current.Clear();
		}

		/// <summary>
		/// Draws the current <see cref="TreemapNode"/> and all the children after it.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		private void DrawAll(Graphics graphics, TreemapRenderingContext ctx)
		{
			// if we are a node of invalid size,
			// we nor our children will be able to draw,
			// so there's nothing left to do.
			if (Bounds.Size.IsValidSize() == false)
			{
				return;
			}


			this.DrawNode(graphics, ctx);
			DrawAllChildren(graphics, ctx);
		}

		/// <summary>
		/// Draws the all the children of the <see cref="TreemapNode"/>
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		private void DrawAllChildren(Graphics graphics, TreemapRenderingContext ctx)
		{
			foreach (TreemapNode child in Nodes)
			{
				child.DrawAll(graphics, ctx);
			}
		}

		#endregion Drawing

		#region Layout

		/// <summary>
		/// Recalculates the bounds of this <see cref="TreemapNode"/> based on it's value and it's children's values.
		/// </summary>
		/// <param name="newBounds">The maximum bounds this <see cref="TreemapNode"/> may exist in.</param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		internal void RecalculateBounds(TreemapView view, RectangleF newBounds)
		{
			if (IsBranch && IsCollapsed)
			{
				newBounds.Height = this.GetBranchHeaderHeight(view);
#if TREEMAP_NODE_MAX_COLLAPSE
				// #COLLAPSING -- reduce newBounds.Width to the bare minimum for this branch?
				int contentWidth = 30;
				int plusMinusWidth = view.PlusMinusGlyphSizePixelsDevice(padded: true);
				int checkboxWidth = TreemapRenderer.LastCheckBoxGlyphSize.Width + TreemapRenderer.LastCheckboxPadding;
				int totalWidth = contentWidth + plusMinusWidth + checkboxWidth;
				newBounds.Width = totalWidth;
#endif // TREEMAP_NODE_MAX_COLLAPSE
			}

			// apply the margin if we're a branch.
			BoundsF = IsBranch
				? newBounds.ApplyPadding(BranchMargin ?? view.NodeBranchMargin)
				: newBounds;

			EmptyArea = new RectangleF(0, 0, BoundsF.Width, BoundsF.Height);
			float area = BoundsF.Width * BoundsF.Height;

			TreemapNodeValueDataType myValue = view.GetNodeValueForCurrentMode(this);

			foreach (TreemapNode child in Nodes)
			{
				TreemapNodeValueDataType childValue = view.GetNodeValueForCurrentMode(child);
				float childValuePercentage = childValue / myValue;
				child.Area = area * childValuePercentage;
			}

			Squarify(Nodes, new List<TreemapNode>(), EmptyArea.ShortestSide());

			foreach (TreemapNode child in Nodes)
			{
				// collapsed branch nodes don't show their children,
				// so squish em down to nothing.
				if (IsBranch && IsCollapsed)
				{
					child.BoundsF = RectangleF.Empty;
					continue;
				}

				if (!child.BoundsF.Size.IsValidSize())
				{
					child.BoundsF = new RectangleF(child.BoundsF.Location, new Size(0, 0));
				}

				float yOffset = 0;

				// if this node (not child) is a branch with a zero height, and we have a header,
				// then we need to increase our y offset.
				if (IsBranch && (ShowBranchHeader ?? view.ShowBranchesAsHeaders) && child.BoundsF.Y == 0)
				{
					yOffset = this.GetBranchHeaderHeight(view);
				}

				var childBounds = new RectangleF
				{
					X = BoundsF.X + child.BoundsF.X,
					Y = BoundsF.Y + child.BoundsF.Y + yOffset,
					Width = child.BoundsF.Width,
					Height = child.BoundsF.Height - yOffset
				};

				child.RecalculateBounds(view, childBounds);
			}
		}

		/// <summary>
		/// Squarifies the <see cref="TreemapNode"/> items.
		/// </summary>
		/// <param name="items">The nodes to squarify.</param>
		/// <param name="row">The current row of nodes.</param>
		/// <param name="sideLength">The length of the size.</param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		private void Squarify(IEnumerable<TreemapNode> items, IEnumerable<TreemapNode> row, float sideLength)
		{
			if (!items.Any())
			{
				ComputeTreeMaps(row);
				return;
			}

			TreemapNode nodes = items.First();
			var row2 = new List<TreemapNode>(row)
			{
				nodes
			};

			var items2 = new List<TreemapNode>(items);
			items2.RemoveAt(0);

			float worst1 = Worst(row, sideLength);
			float worst2 = Worst(row2, sideLength);

			if (!row.Any() || worst1 > worst2)
			{
				Squarify(items2, row2, sideLength);
			}
			else
			{
				ComputeTreeMaps(row);
				Squarify(items, new List<TreemapNode>(), EmptyArea.ShortestSide());
			}
		}

		/// <summary>
		/// Computes the tree map for the given <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="nodes"></param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		private void ComputeTreeMaps(IEnumerable<TreemapNode> nodes)
		{
			RowOrientation orientation = GetOrientation();

			float areaSum = 0;

			foreach (TreemapNode child in nodes)
			{
				areaSum += child.Area;
			}

			RectangleF currentRow;
			if (orientation == RowOrientation.Horizontal)
			{
				currentRow = new RectangleF(EmptyArea.X, EmptyArea.Y, areaSum / EmptyArea.Height, EmptyArea.Height);
				EmptyArea = new RectangleF(EmptyArea.X + currentRow.Width, EmptyArea.Y, Math.Max(0, EmptyArea.Width - currentRow.Width), EmptyArea.Height);
			}
			else
			{
				currentRow = new RectangleF(EmptyArea.X, EmptyArea.Y, EmptyArea.Width, areaSum / EmptyArea.Width);
				EmptyArea = new RectangleF(EmptyArea.X, EmptyArea.Y + currentRow.Height, EmptyArea.Width, Math.Max(0, EmptyArea.Height - currentRow.Height));
			}

			float prevX = currentRow.X;
			float prevY = currentRow.Y;

			foreach (TreemapNode child in nodes)
			{
				child.BoundsF = GetRectangle(orientation, child, prevX, prevY, currentRow.Width, currentRow.Height);

				ComputeNextPosition(orientation, ref prevX, ref prevY, child.BoundsF.Width, child.BoundsF.Height);
			}
		}

		/// <summary>
		/// Gets a rectangle in the given row orientation with the specified values,
		/// based on the area of the given <see cref="TreemapNode"/>
		/// </summary>
		/// <param name="orientation">The orientation of the current row.</param>
		/// <param name="item">The item to base the <see cref="RectangleF"/>'s size on.</param>
		/// <param name="x">The horizontal position of the result.</param>
		/// <param name="y">The vertical position of the result.</param>
		/// <param name="width">The original width of the result.</param>
		/// <param name="height">The original height of the result.</param>
		/// <returns>A <see cref="RectangleF"/> based on the given parameters, but modified by the area of the item.</returns>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		private static RectangleF GetRectangle(RowOrientation orientation, TreemapNode item, float x, float y, float width, float height)
		{
			if (orientation == RowOrientation.Horizontal)
			{
				return new RectangleF(x, y, width, item.Area / width);
			}
			else
			{
				return new RectangleF(x, y, item.Area / height, height);
			}
		}

		/// <summary>
		/// Computes the next position, by moving the referenced x or y position depending on the orientation of the row.
		/// </summary>
		/// <param name="orientation">The row orientation.</param>
		/// <param name="xPos">The current horizontal position. Modified if the row is vertical.</param>
		/// <param name="yPos">The current vertical position. Modified if the row is horizontal.</param>
		/// <param name="width">The width to move the position by.</param>
		/// <param name="height">The height to move the position by.</param>
		private static void ComputeNextPosition(RowOrientation orientation, ref float xPos, ref float yPos, float width, float height)
		{
			if (orientation == RowOrientation.Horizontal)
			{
				yPos += height;
			}
			else
			{
				xPos += width;
			}
		}

		/// <summary>
		/// Gets a row orientation based on the empty area of the node.
		/// </summary>
		/// <returns>A <see cref="RowOrientation"/> based on which is larger: the width or height.</returns>
		private RowOrientation GetOrientation()
			=> EmptyArea.Width > EmptyArea.Height ?
			RowOrientation.Horizontal :
			RowOrientation.Vertical;

		/// <summary>
		/// Gets the 'worst' row length.
		/// </summary>
		/// <param name="row">The row being tested.</param>
		/// <param name="sideLength">The current length of the side.</param>
		/// <returns>The worst case length depending on the length of the side and the node's area.</returns>
		private static float Worst(IEnumerable<TreemapNode> row, float sideLength)
		{
			if (!row.Any())
			{
				return 0;
			}

			float maxArea = 0;
			float minArea = float.MaxValue;
			float totalArea = 0;

			foreach (TreemapNode item in row)
			{
				maxArea = Math.Max(maxArea, item.Area);
				minArea = Math.Min(minArea, item.Area);
				totalArea += item.Area;
			}

			if (minArea == float.MaxValue)
			{
				minArea = 0;
			}

			float val1 = sideLength * sideLength * maxArea / (totalArea * totalArea);
			float val2 = totalArea * totalArea / (sideLength * sideLength * minArea);

			return Math.Max(val1, val2);
		}

		#endregion Layout

	}
}
