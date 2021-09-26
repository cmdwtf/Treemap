using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using static cmdwtf.Toolkit.WinForms.Drawing;
using static cmdwtf.Toolkit.WinForms.Forms;

using CheckBoxState = System.Windows.Forms.VisualStyles.CheckBoxState;
using ContentAlignment = System.Drawing.ContentAlignment;

//using VSE = System.Windows.Forms.VisualStyles.VisualStyleElement;
//using VSI = System.Windows.Forms.VisualStyles.VisualStyleInformation;
//using VSR = System.Windows.Forms.VisualStyles.VisualStyleRenderer;
//using VSS = System.Windows.Forms.VisualStyles.VisualStyleState;

namespace cmdwtf.Treemap
{
	public static class TreemapRenderer
	{
		#region Constants

		/// <summary>
		/// How large is the Plus/Minus (Expand/Collapse) glyph in (logical) pixels.
		/// 9px was chosen because that's what it was on my machine with 100% DPI,
		/// and was at 18px on another machine I had with 200% DPI.
		/// Looks a little small, but also looks 'correct', I guess.
		/// </summary>
		private const int _plusMinusGlyphSizePixels = 9;

		/// <summary>
		/// The message for the exception thrown if a draw is called with no node.
		/// </summary>
		private const string _currentNodeNullMessage = "The current render info needs a non-null node in order to draw!";

		#endregion Constants

		#region Cache

		/// <summary>
		/// A cache for dynamically generated underlined versions of <see cref="Font"/>s for hot tracking.
		/// </summary>
		private static readonly Dictionary<Font, Font> _cachedFonts = new();

		/// <summary>
		/// A cache for <see cref="Brush"/>es.
		/// </summary>
		private static readonly Dictionary<Color, Brush> _cachedBrushes = new();

		/// <summary>
		/// A cache for <see cref="Pen"/>s.
		/// </summary>
		private static readonly Dictionary<Color, Pen> _cachedPens = new();

		/// <summary>
		/// A cache for <see cref="VisualStyleRenderer"/>s.
		/// </summary>
		private static readonly Dictionary<VisualStyleElement, VisualStyleRenderer> _cachedRenderers = new();

		/// <summary>
		/// Gets (or creates and caches) a <see cref="Font"/> with the <see cref="FontStyle.Underline"/>
		/// style, based on <paramref name="normalFont"/>.
		/// </summary>
		/// <param name="font">The font to base the modified font on.</param>
		/// <returns>The desired <see cref="Font"/>.</returns>
		private static Font GetCachedUnderlineFont(this Font font)
		{
			if (!_cachedFonts.ContainsKey(font))
			{
				_cachedFonts[font] = new Font(font, font.Style | FontStyle.Underline);
			}

			return _cachedFonts[font];
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="SolidBrush"/> for the given <see cref="Color"/>
		/// </summary>
		/// <param name="color">The <see cref="Color"/> to get a brush for.</param>
		/// <returns>The <see cref="Brush"/>.</returns>
		private static Brush GetCachedBrush(this Color color)
		{
			if (!_cachedBrushes.ContainsKey(color))
			{
				_cachedBrushes[color] = new SolidBrush(color);
			}

			return _cachedBrushes[color];
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="Pen"/> for the given <see cref="Color"/>.
		/// </summary>
		/// <param name="color">The <see cref="Color"/> to get a pen for.</param>
		/// <returns>The <see cref="Pen"/>.</returns>
		private static Pen GetCachedPen(this Color color)
		{
			if (!_cachedPens.ContainsKey(color))
			{
				_cachedPens[color] = new Pen(color);
			}

			return _cachedPens[color];
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="VisualStyleRenderer"/> for the given <see cref="VisualStyleElement"/>.
		/// </summary>
		/// <param name="element">The <see cref="VisualStyleElement"/> to get a renderer for.</param>
		/// <returns>The <see cref="VisualStyleRenderer"/>.</returns>
		private static VisualStyleRenderer GetCachedRenderer(this VisualStyleElement element)
		{
			if (!_cachedRenderers.ContainsKey(element))
			{
				_cachedRenderers[element] = new VisualStyleRenderer(element);
			}

			return _cachedRenderers[element];
		}

		#endregion Cache

		#region Calculators

		/// <summary>
		/// Gets a value representing the thickness of the grid at this node depth.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The view to get the <see cref="TreemapView.GridWidthDepthMultiplier"/> from.</param>
		/// <returns>The value of the thickness of the grid.</returns>
		public static int GetGridThickness(this TreemapNode node, TreemapView view)
			=> view.LogicalToDeviceUnits((view.GridWidthDepthMultiplier * node.Level) + 1);

		/// <summary>
		/// Gets a <see cref="RectangleF"/> shrunk and offset by <see cref="LeafPadding"/>
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The owning view to fallback on if <see cref="LeafPadding"/> is null.</param>
		/// <returns>The shrunk and offset <see cref="RectangleF"/></returns>
		public static RectangleF GetPaddedBoundsF(this TreemapNode node, TreemapView view)
			=> node.BoundsF.ApplyPadding(node.LeafPadding ?? view.NodeLeafPadding);

		/// <summary>
		/// Gets a <see cref="RectangleF"/> shrunk and offset by <see cref="LeafPadding"/>
		/// (if the node is a leaf), as well as the grid width.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The owning view to fallback on if <see cref="LeafPadding"/> is null.</param>
		/// <returns>The shrunk and offset <see cref="RectangleF"/></returns>
		public static RectangleF GetGridPaddedBoundsF(this TreemapNode node, TreemapView view)
		{
			RectangleF rect = node.IsLeaf
				? node.GetPaddedBoundsF(view)
				: node.BoundsF;
			return node.GetGridPadded(view, rect);
		}

		/// <summary>
		/// Pads a <see cref="RectangleF"/> by the width of the grid at this node's depth.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The <see cref="TreemapView"/> to get the <see cref="TreemapView.GridWidthDepthMultiplier"/> from.</param>
		/// <param name="rect">The <see cref="RectangleF"/> to pad.</param>
		/// <returns>The padded <see cref="RectangleF"/>.</returns>
		public static RectangleF GetGridPadded(this TreemapNode node, TreemapView view, RectangleF rect)
		{
			var gridPadding = new Padding(node.GetGridThickness(view));
			return rect.ApplyPadding(gridPadding);
		}

		/// <summary>
		/// Gets a DPI-aware device unit (pixel) value representing this node's header height, if any.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The <see cref="TreemapView"/> to get the <see cref="TreemapView.NodeBranchHeaderHeight"/> from.</param>
		/// <returns>
		/// The node's local header height value, if it has one.
		/// Otherwise, falls back to the owning view's header height.
		/// </returns>
		public static int GetBranchHeaderHeight(this TreemapNode node, TreemapView view)
		{
			if ((node.ShowBranchHeader ?? view.ShowBranchesAsHeaders) != false)
			{
				int pixels = node.BranchHeight ?? view.NodeBranchHeaderHeight;
				return view?.LogicalToDeviceUnits(pixels) ?? pixels;
			}
			return 0;
		}

		/// <summary>
		/// Gets a <see cref="Color"/> based on the given color.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The <see cref="TreemapView"/> to get the <see cref="TreemapView.HideSelection"/> from.</param>
		/// <param name="baseColor">The <see cref="Color"/> to base the modified color on.</param>
		/// <returns>
		/// If the <see cref="TreemapNode"/> <see cref="IsSelectedInHirearchy"/>,
		/// and if <see cref="TreemapView.HideSelection"/> is false,
		/// will return a modified color to designate the state.
		/// Otherwise, returns the base color.
		/// </returns>
		private static Color GetSelectionModifiedColor(this TreemapNode node, TreemapView view, Color baseColor)
		{
			if (node.IsSelectedInHirearchy && (!view.HideSelection || view.Focused))
			{
				return ControlPaint.Light(baseColor);
			}

			return baseColor;
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="Font"/> with the <see cref="FontStyle.Underline"/>
		/// style, based on <paramref name="normalFont"/> if the <see cref="TreeNode"/> is hot tracking.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="normalFont">The font to base the modified font on.</param>
		/// <returns>The desired <see cref="Font"/>.</returns>
		private static Font GetHotTrackingModifiedFont(this TreemapNode node, Font normalFont)
		{
			Font? font = node.IsHotTracking
				? normalFont.GetCachedUnderlineFont()
				: normalFont;

			return font;
		}

		/// <summary>
		/// Gets (or creates and caches) a <see cref="Brush"/> for the appropriate <see cref="Color"/> and hot tracking state.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The <see cref="TreemapView"/> to get the <see cref="TreemapView.HideSelection"/> from.</param>
		/// <param name="normalColor">The normal <see cref="Color"/>.</param>
		/// <param name="hotTrackingColor">The hot tracking <see cref="Color"/></param>
		/// <returns>The desired <see cref="Brush"/>.</returns>
		private static Brush GetHotTrackingModifiedBrush(this TreemapNode node, TreemapView view, Color normalColor, Color hotTrackingColor)
		{
			Brush brush = node.IsHotTracking
				? node.GetSelectionModifiedColor(view, hotTrackingColor).GetCachedBrush()
				: node.GetSelectionModifiedColor(view, normalColor).GetCachedBrush();

			return brush;
		}


		/// <summary>
		/// Gets (or creates and caches) a <see cref="Pen"/> for the appropriate <see cref="Color"/> and hot tracking state.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to use.</param>
		/// <param name="view">The <see cref="TreemapView"/> to get the <see cref="TreemapView.HideSelection"/> from.</param>
		/// <param name="normalColor">The normal <see cref="Color"/>.</param>
		/// <param name="hotTrackingColor">The hot tracking <see cref="Color"/></param>
		/// <returns>The desired <see cref="Pen"/>.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Future use.")]
		private static Pen GetHotTrackingModifiedPen(this TreemapNode node, TreemapView view, Color normalColor, Color hotTrackingColor)
		{
			Pen pen = node.IsHotTracking
				? node.GetSelectionModifiedColor(view, hotTrackingColor).GetCachedPen()
				: node.GetSelectionModifiedColor(view, normalColor).GetCachedPen();

			return pen;
		}

		/// <summary>
		/// Gets the size of the plus/minus ([+]/[-]; Expand/Collapse) glyph in device pixels.
		/// </summary>
		/// <param name="control">The control to use for DPI pixel translation.</param>
		/// <returns>The glyph <see cref="Size"/></returns>
		public static Size PlusMinusGlyphSizePixelsDevice(this Control control)
		{
			int pixels = control.LogicalToDeviceUnits(_plusMinusGlyphSizePixels);
			return new Size(pixels, pixels);
		}

		private static Size LastPlusMinusPadding { get; set; } = Size.Empty;

		internal static Size LastCheckBoxGlyphSize { get; private set; } = Size.Empty;

		internal static Size LastPlusMinusGlyphSize { get; private set; } = Size.Empty;

		#endregion Calculators

		#region Drawing

		/// <summary>
		/// Draws this node.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to draw.</param>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		public static void DrawNode(this TreemapNode node, Graphics graphics, TreemapRenderingContext ctx)
		{
			// store the node for the rest of the draw functions to use.
			ctx.Current.Node = node;

			if (node.IsLeaf)
			{
				// draw a leaf, the main part of the tree!
				DrawLeaf(graphics, ctx);
			}
			else
			{
				// not a leaf yet, draw a branch header if applicable, and recurse.
				DrawBranch(graphics, ctx);
			}

			DrawGrid(graphics, ctx);
		}

		/// <summary>
		/// Draws the node's grid.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="step">The current step of the draw pass.</param>
		/// <param name="ctx">The draw pass information.</param>
		public static void DrawGrid(Graphics graphics, TreemapRenderingContext ctx)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;

			// if the grid is turned off, we're done here.
			if (!view.ShowGrid)
			{
				return;
			}

			TreemapNodeDrawStyle style = node.LeafDrawStyle ?? ctx.View.NodeLeafDrawStyle;

			switch (style)
			{
				case TreemapNodeDrawStyle.Flat:
				case TreemapNodeDrawStyle.Gradient:
				case TreemapNodeDrawStyle.GradientVertical:
				case TreemapNodeDrawStyle.GradientHorizontal:
					break;
				default:
					// unknown draw style, don't draw grid.
					return;
			}

			RectangleF gridBounds = node.IsBranch ? node.BoundsF : node.GetPaddedBoundsF(view);

			int gridThickness = node.GetGridThickness(view);
			int halfThickness = (gridThickness / 2);

			gridBounds.Inflate(-halfThickness, -halfThickness);

			DrawTreemapNodeEventArgs args = new(graphics, node, gridBounds, TreemapNodeDrawStep.Grid);

			// draw the grid for this node
			if (ctx.OwnerDraw)
			{
				ctx.RaiseDrawGrid(args);
			}

			if (args.DrawDefault || ctx.ControlDrawGrid)
			{
				//Pen pen = node.GetHotTrackingModifiedPen(view, view.GridColor, view.GridHotTrackColor);
				Pen pen = view.GridColor.GetCachedPen();
				pen.Width = gridThickness;
				graphics.DrawRectangle(pen, gridBounds);
				//graphics.DrawLine(pen, gridBounds.TopLeft(), gridBounds.TopRight());
				//graphics.DrawLine(pen, gridBounds.TopLeft(), gridBounds.BottomLeft());
			}
		}

		#region Branch

		/// <summary>
		/// Draws a branch node's foreground and background. Invokes the owner draw events if needed.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		public static void DrawBranch(Graphics graphics, TreemapRenderingContext ctx)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			PointF location = node.BoundsF.Location;
			SizeF size = node.BoundsF.Size;
			int headerHeight = node.GetBranchHeaderHeight(view);
			DrawTreemapNodeEventArgs args = new(graphics, node, node.BoundsF, ctx.Current.Step);

			if (headerHeight > 0)
			{
				if (ctx.OwnerDraw)
				{
					ctx.RaiseDrawBranchNode(args);
				}

				var headerBounds = new RectangleF(location, new SizeF(size.Width, headerHeight));

				if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Background) &&
					(args.DrawDefault || ctx.ControlDrawNode))
				{
					DrawBranchBackground(graphics, ctx, headerBounds);
				}

				if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Foreground) &&
					(args.DrawDefault || ctx.ControlDrawText))
				{
					DrawBranchForeground(graphics, ctx, headerBounds);
				}

				if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Overlay) &&
					(args.DrawDefault || ctx.ControlDrawGlyphs))
				{
					DrawPlusMinus(graphics, ctx, headerBounds);
					DrawCheckState(graphics, ctx, headerBounds);
				}
			}
		}

		/// <summary>
		/// Draws a branch node's foreground.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		/// <param name="headerBounds">THe boudns the branch header should draw in.</param>
		private static void DrawBranchForeground(Graphics graphics, TreemapRenderingContext ctx, RectangleF headerBounds)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			Color headerHotTrackForeColor = node.BranchHotTrackForeColor ?? view.NodeBranchHotTrackForeColor;
			Color headerForeColor = node.BranchForeColor ?? view.NodeBranchForeColor;
			Font headerFont = node.GetHotTrackingModifiedFont(node.NodeBranchFont ?? view.NodeBranchFont);
			StringFormat format = node.BranchStringFormat ?? view.NodeBranchHeaderStringFormat;
			Brush headerTextBrush = node.GetHotTrackingModifiedBrush(view, headerForeColor, headerHotTrackForeColor);

			format.UpdateRightToLeft(view);

			graphics.DrawString(node.Text, headerFont, headerTextBrush, headerBounds, format);
			graphics.DrawNodeImage(ctx, headerBounds);
		}

		/// <summary>
		/// Draws a branch node's background.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="view">The parent view.</param>
		/// <param name="headerBounds">The bounds the branch header should draw in.</param>
		private static void DrawBranchBackground(Graphics graphics, TreemapRenderingContext ctx, RectangleF headerBounds)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			Color headerBackColor = node.GetSelectionModifiedColor(view, node.BranchBackColor ?? view.NodeBranchBackColor);
			Brush headerBackBrush = headerBackColor.GetCachedBrush();
			graphics.FillRectangle(headerBackBrush, headerBounds);


		}

		#endregion Branch

		#region Leaf

		/// <summary>
		/// Draws a leaf node's foreground and background. Invokes the owner draw events if needed.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">The draw pass information.</param>
		public static void DrawLeaf(Graphics graphics, TreemapRenderingContext ctx)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			RectangleF paddedBounds = node.GetPaddedBoundsF(view);
			DrawTreemapNodeEventArgs args = new(graphics, node, paddedBounds, ctx.Current.Step);

			if (ctx.OwnerDraw)
			{
				ctx.RaiseDrawNode(args);
			}

			// draw the leaf if we've gotten to the end of a branch!
			if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Background) &&
				(args.DrawDefault || ctx.ControlDrawNode))
			{
				DrawLeafBackground(graphics, ctx);
			}

			if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Foreground) &&
				(args.DrawDefault || ctx.ControlDrawText))
			{
				DrawLeafForeground(graphics, ctx);
			}
			if (ctx.Current.Step.HasFlag(TreemapNodeDrawStep.Overlay) &&
				(args.DrawDefault || ctx.ControlDrawGlyphs))
			{
				RectangleF glyphArea = node.GetPaddedBoundsF(view);

				// no plus/minus glyph on leafs.
				if (node.PlusMinusArea.IsEmpty == false)
				{
					node.PlusMinusArea = RectangleF.Empty;
				}

				// but we can still have check states.
				DrawCheckState(graphics, ctx, glyphArea);
			}
		}

		/// <summary>
		/// Draws a leaf node's foreground.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		private static void DrawLeafForeground(Graphics graphics, TreemapRenderingContext ctx)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			RectangleF paddedBounds = node.GetGridPaddedBoundsF(view);
			Color foreColor = node.ForeColor ?? view.NodeLeafForeColor;
			Color hotTrackForeColor = node.HotTrackForeColor ?? view.NodeLeafHotTrackForeColor;
			Font nodeFont = node.GetHotTrackingModifiedFont(node.NodeLeafFont ?? view.NodeLeafFont);
			StringFormat format = node.LeafStringFormat ?? view.NodeLeafStringFormat;
			Brush textBrush = node.GetHotTrackingModifiedBrush(view, foreColor, hotTrackForeColor);

			format.UpdateRightToLeft(view);

			graphics.DrawString(node.Text, nodeFont, textBrush, paddedBounds, format);
			graphics.DrawNodeImage(ctx, paddedBounds);
		}

		/// <summary>
		/// Draws a leaf node's background.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		private static void DrawLeafBackground(Graphics graphics, TreemapRenderingContext ctx)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			Color backColor = node.BackColor ?? view.NodeLeafBackColor;
			TreemapNodeDrawStyle style = node.LeafDrawStyle ?? view.NodeLeafDrawStyle;
			RectangleF paddedBounds = node.GetPaddedBoundsF(view);

			if (paddedBounds.Width <= 0 && paddedBounds.Height <= 0)
			{
				// no bounds, nothing to draw
				return;
			}

			Color fillColor = node.GetSelectionModifiedColor(view, backColor);
			Color highlightColor = ControlPaint.Light(fillColor);

			switch (style)
			{
				case TreemapNodeDrawStyle.Flat:
				{
					Brush flatBrush = fillColor.GetCachedBrush();
					graphics.FillRectangle(flatBrush, paddedBounds);
					break;
				}
				case TreemapNodeDrawStyle.Gradient:
					graphics.FillRectangleRadialGradient(fillColor, paddedBounds, highlightColor);
					break;
				case TreemapNodeDrawStyle.GradientHorizontal:
					graphics.FillRectangleLinearGradient(fillColor, paddedBounds, highlightColor, 0.0f);
					break;
				case TreemapNodeDrawStyle.GradientVertical:
					graphics.FillRectangleLinearGradient(fillColor, paddedBounds, highlightColor, 90.0f);
					break;
				default:
					graphics.DrawErrorRectangle(paddedBounds);
					break;
			}
		}

		#endregion Leaf

		#region Glyphs

		/// <summary>
		/// Draws the <see cref="TreemapNode"/>'s image into the specified graphics and bounds.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		/// <param name="bounds">The bounds to draw the image into.</param>
		private static void DrawNodeImage(this Graphics graphics, TreemapRenderingContext ctx, RectangleF bounds)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;
			ContentAlignment imageAlign = node.ImageAlign ?? view.NodeImageAlign;

			// get an image to draw
			Image? img = node.Image ??
				(node.IsSelected ? node.SelectedImageIndexer.GetImage(view.SelectedImageIndexer) : null) ??
				(!node.IsSelected ? node.ImageIndexer.GetImage(view.ImageIndexer) : null);

			// draw it, if we have one.
			if (img is not null)
			{
				node.ImageArea = graphics.DrawImage(img, bounds, imageAlign);
			}
			else if (node.ImageArea.IsEmpty == false)
			{
				node.ImageArea = RectangleF.Empty;
			}
		}

		/// <summary>
		/// Draws the <see cref="TreemapNode"/>'s expand/collapse glyph.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		/// <param name="glyphArea">The maximum area the glyph may occupy.</param>
		private static void DrawPlusMinus(Graphics graphics, TreemapRenderingContext ctx, RectangleF glyphArea)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;

			if (!view.ShowPlusMinus)
			{
				if (node.PlusMinusArea.IsEmpty == false)
				{
					node.PlusMinusArea = RectangleF.Empty;
				}

				return;
			}

			var glyphRect = Rectangle.Round(glyphArea);

			// calculate and apply glyph size, DPI aware.
			LastPlusMinusGlyphSize = view.PlusMinusGlyphSizePixelsDevice();

			// adjust glyph to be centered vertically in branches, and move it in that far horizontally.
			int branchHeight = node.GetBranchHeaderHeight(view) + 1; // plus 1 to cause a round up.

			int verticalOffset = (int)Math.Round((branchHeight / 2.0f) - (LastPlusMinusGlyphSize.Height / 2.0f));
			int horizontalOffset = node.GetGridThickness(view) + view.LogicalToDeviceUnits(1);
			LastPlusMinusPadding = new Size(horizontalOffset, verticalOffset);

			glyphRect.X += LastPlusMinusPadding.Width;
			glyphRect.Y += LastPlusMinusPadding.Height;

			// set the glyph size
			glyphRect.Width = LastPlusMinusGlyphSize.Width;
			glyphRect.Height = LastPlusMinusGlyphSize.Height;

			// store this calculated area.
			node.PlusMinusArea = glyphRect;

			// tell the user we're going to draw the plus minus glyph, and stop if we don't need to.
			DrawTreemapNodeEventArgs args = new(graphics, node, glyphRect, ctx.Current.Step);
			ctx.RaiseDrawPlusMinusGlyph(args);

			if (!args.DrawDefault && !ctx.ControlDrawGlyphs)
			{
				return;
			}

			if (node.IsCollapsed)
			{
				VisualStyleRenderer plusRenderer =
#if USE_CLASSIC_PLUSMINUS_GLYPHS
				VisualStyleElement.TreeView.Glyph.Closed.GetCachedRenderer();
#else
					node.IsMouseOverPlusMinus
					? CustomVisualStyleElement.ExplorerTreeView.Glyph.Hover.Closed.GetCachedRenderer()
					: CustomVisualStyleElement.ExplorerTreeView.Glyph.Normal.Closed.GetCachedRenderer();
#endif // USE_CLASSIC_PLUSMINUS_GLYPHS
				plusRenderer.DrawBackground(graphics, glyphRect);
			}
			else
			{
				VisualStyleRenderer minusRenderer =
#if USE_CLASSIC_PLUSMINUS_GLYPHS
					VisualStyleElement.TreeView.Glyph.Opened.GetCachedRenderer();
#else
					node.IsMouseOverPlusMinus
					? CustomVisualStyleElement.ExplorerTreeView.Glyph.Hover.Opened.GetCachedRenderer()
					: CustomVisualStyleElement.ExplorerTreeView.Glyph.Normal.Opened.GetCachedRenderer();
#endif // USE_CLASSIC_PLUSMINUS_GLYPHS
				minusRenderer.DrawBackground(graphics, glyphRect);
			}
		}

		/// <summary>
		/// Draws the <see cref="TreemapNode"/>'s checked state.
		/// </summary>
		/// <param name="graphics">The context to draw into.</param>
		/// <param name="ctx">Information about the current rendering context.</param>
		/// <param name="glyphArea">The maximum area the check state may occupy.</param>
		private static void DrawCheckState(Graphics graphics, TreemapRenderingContext ctx, RectangleF glyphArea)
		{
			TreemapNode node = ctx.Current.Node ?? throw new ArgumentException(_currentNodeNullMessage, nameof(ctx));
			TreemapView view = ctx.View;

			if (!view.CheckBoxes)
			{
				if (node.CheckboxArea.IsEmpty == false)
				{
					node.CheckboxArea = RectangleF.Empty;
				}

				return;
			}

			// if we drew the plus/minus,
			// we need to bump our state glyph over.
			if (view.ShowPlusMinus && node.IsBranch)
			{
				float horizontalMove = node.PlusMinusArea.Width + LastPlusMinusPadding.Width;
				glyphArea.Width -= horizontalMove;
				glyphArea.X += horizontalMove;
			}

			Image? stateImage = node.StateImageIndexer.GetImage(view.StateImageIndexer);
			CheckBoxState visualState = CheckBoxState.UncheckedNormal;

			if (stateImage is not null)
			{
				node.CheckboxArea = stateImage.CalcImageRenderBounds(glyphArea, ContentAlignment.TopLeft);
				LastCheckBoxGlyphSize = stateImage.Size;
			}
			else
			{
				var glyphRect = Rectangle.Round(glyphArea);

				visualState = node.IsMouseOverCheckbox
						? node.CheckState.ToVisualStyleHot()
						: node.CheckState.ToVisualStyleNormal();

				// calculate glyph size, should be DPI aware.
				LastCheckBoxGlyphSize = CheckBoxRenderer.GetGlyphSize(graphics, visualState);

				// calculate horizontal offset as twice the grid thickness
				int horizontalOffset = node.GetGridThickness(view) * 2;

				// for leaf nodes, just use the same offset for vertical
				int verticalOffset = horizontalOffset;

				// for branch nodes, center vertically.
				if (node.IsBranch)
				{
					float halfBranchHeight = (node.GetBranchHeaderHeight(view) + 0.5f) / 2.0f;
					float halfGlyphHeight = LastCheckBoxGlyphSize.Height / 2.0f;
					verticalOffset = (int)Math.Ceiling(halfBranchHeight - halfGlyphHeight);
				}

				// apply offsets
				glyphRect.X += horizontalOffset;
				glyphRect.Y += verticalOffset;

				// set the glyph size
				glyphRect.Width = LastCheckBoxGlyphSize.Width;
				glyphRect.Height = LastCheckBoxGlyphSize.Height;

				// store this calculated area.
				node.CheckboxArea = glyphRect;
			}


			// tell the user we're going to draw the plus minus glyph, and stop if we don't need to.
			DrawTreemapNodeEventArgs args = new(graphics, node, node.CheckboxArea, ctx.Current.Step);
			ctx.RaiseDrawStateGlyph(args);

			if (!args.DrawDefault && !ctx.ControlDrawGlyphs)
			{
				return;
			}

			if (stateImage is not null)
			{
				graphics.DrawImage(stateImage, node.CheckboxArea);
			}
			else
			{
				var location = Point.Round(node.CheckboxArea.Location);
				CheckBoxRenderer.DrawCheckBox(graphics, location, visualState);
			}
		}

		#endregion Glyphs

		#endregion Drawing
	}
}
