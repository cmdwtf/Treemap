using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using static cmdwtf.Toolkit.WinForms.Drawing;
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
		/// </returns>
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
		/// </returns>
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

		#region Tiling

		internal void ReTile(RectangleF newBounds)
		{
			//Tiling.Strategies.Squarified.RecalculateBounds(this, newBounds);
			Tiling.Strategies.SliceAndDice.RecalculateBounds(this, newBounds);
		}

		#endregion Tiling

	}
}
