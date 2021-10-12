using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using cmdwtf.Toolkit.WinForms;

namespace cmdwtf.Treemap.Tiling
{
	internal class SquarifiedTilingStrategy : RectangleTilingStrategyBase
	{
		/// <summary>
		/// Recalculates the bounds of this <see cref="TreemapNode"/> based on it's value and it's children's values.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> that should recalculate it's bounds of.</param>
		/// <param name="newBounds">The maximum bounds this <see cref="TreemapNode"/> may exist in.</param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		public override void RecalculateBounds(TreemapNode node, RectangleF newBounds)
		{
			TreemapView? view = node.TreemapView;

			if (view is null)
			{
				throw new ArgumentException($"{nameof(RecalculateBounds)} expects the node given to have a non-null {nameof(node.TreemapView)}");
			}

			if (node.IsBranch && node.IsCollapsed)
			{
				newBounds.Height = node.GetBranchHeaderHeight(view);
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
			node.BoundsF = node.IsBranch
				? newBounds.ApplyPadding(node.BranchMargin ?? view.NodeBranchMargin)
				: newBounds;

			var workingArea = new RectangleF(0, 0, node.BoundsF.Width, node.BoundsF.Height);

			RecalculateChildAreas(node, view);

			Squarify(node.Nodes, new List<TreemapNode>(), ref workingArea);

			foreach (TreemapNode child in node.Nodes)
			{
				// collapsed branch nodes don't show their children,
				// so squish em down to nothing.
				if (node.IsBranch && node.IsCollapsed)
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
				if (node.IsBranch && (node.ShowBranchHeader ?? view.ShowBranchesAsHeaders) && child.BoundsF.Y == 0)
				{
					yOffset = node.GetBranchHeaderHeight(view);
				}

				var childBounds = new RectangleF
				{
					X = node.BoundsF.X + child.BoundsF.X,
					Y = node.BoundsF.Y + child.BoundsF.Y + yOffset,
					Width = child.BoundsF.Width,
					Height = child.BoundsF.Height - yOffset
				};

				RecalculateBounds(child, childBounds);
			}
		}

		/// <summary>
		/// Squarifies the <see cref="TreemapNode"/> items.
		/// </summary>
		/// <param name="items">The nodes to squarify.</param>
		/// <param name="row">The current row of nodes.</param>
		/// <param name="workingArea">The node's current occupying space.</param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		private void Squarify(IEnumerable<TreemapNode> items, IEnumerable<TreemapNode> row, ref RectangleF workingArea)
		{
			if (!items.Any())
			{
				ComputeTreeMaps(row, ref workingArea);
				return;
			}

			TreemapNode nodes = items.First();
			var row2 = new List<TreemapNode>(row)
			{
				nodes
			};

			var items2 = new List<TreemapNode>(items);
			items2.RemoveAt(0);

			float sideLength = workingArea.ShortestSide();
			float worst1 = Worst(row, sideLength);
			float worst2 = Worst(row2, sideLength);

			if (!row.Any() || worst1 > worst2)
			{
				Squarify(items2, row2, ref workingArea);
			}
			else
			{
				ComputeTreeMaps(row, ref workingArea);
				Squarify(items, new List<TreemapNode>(), ref workingArea);
			}
		}

		/// <summary>
		/// Computes the tree map for the given <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="nodes"></param>
		/// <param name="workingArea">The node's current occupying space.</param>
		/// <remarks>
		/// License: CC BY-SA 3.0
		/// Original work: https://stackoverflow.com/a/37243686
		/// </remarks>
		private void ComputeTreeMaps(IEnumerable<TreemapNode> nodes, ref RectangleF workingArea)
		{
			RowOrientation orientation = GetOrientation(workingArea);

			float areaSum = CalculateNodeArea(nodes);

			RectangleF currentRow;
			if (orientation == RowOrientation.Horizontal)
			{
				currentRow = new RectangleF(workingArea.X, workingArea.Y, areaSum / workingArea.Height, workingArea.Height);
				workingArea = new RectangleF(workingArea.X + currentRow.Width, workingArea.Y, Math.Max(0, workingArea.Width - currentRow.Width), workingArea.Height);
			}
			else
			{
				currentRow = new RectangleF(workingArea.X, workingArea.Y, workingArea.Width, areaSum / workingArea.Width);
				workingArea = new RectangleF(workingArea.X, workingArea.Y + currentRow.Height, workingArea.Width, Math.Max(0, workingArea.Height - currentRow.Height));
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
		private static RowOrientation GetOrientation(RectangleF rect)
			=> rect.Width > rect.Height ?
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
	}
}
