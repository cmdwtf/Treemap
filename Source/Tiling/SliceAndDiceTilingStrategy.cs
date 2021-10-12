using System;
using System.Drawing;
using System.Windows.Forms;

using cmdwtf.Toolkit.WinForms;

namespace cmdwtf.Treemap.Tiling
{
	internal class SliceAndDiceTilingStrategy : RectangleTilingStrategyBase
	{
		public override void RecalculateBounds(TreemapNode node, RectangleF newBounds)
		{
			TreemapView? view = node.TreemapView;
			if (view is null)
			{
				throw new ArgumentException($"{nameof(RecalculateBounds)} expects the node given to have a non-null {nameof(node.TreemapView)}");
			}

			// set up the root node and start the recursive calculation
			node.BoundsF = newBounds;
			node.Area = newBounds.Width * newBounds.Height;
			SliceAndDice(node, null, Orientation.Horizontal);
		}

		private static void SliceAndDice(TreemapNode node, TreemapNode? parent, Orientation orientation)
		{
			TreemapView? view = node.TreemapView;
			if (view is null)
			{
				throw new ArgumentException($"{nameof(SliceAndDice)} expects the node given to have a non-null {nameof(node.TreemapView)}");
			}

			// fine, i'll be my own mom!
			if (parent is null)
			{
				parent = node;
			}

			RecalculateChildAreas(parent, view);

			float parentOffset = parent.IsBranch
				? parent.GetBranchHeaderHeight(view)
				: 0;

			float headerHeight = node.IsBranch
				? node.GetBranchHeaderHeight(view)
				: 0;

			float width = parent.BoundsF.Width;
			float height = Math.Max(parent.BoundsF.Height - parentOffset, 0);

			float childPercentage = parent == null ? 1.0f : node.Area / parent.Area;

			Orientation nextOrientation;
			RectangleF newBounds = new RectangleF(node.BoundsF.Location, SizeF.Empty);
			Func<TreemapNode, float> getOffsetForChild;


			if (orientation == Orientation.Horizontal)
			{
				nextOrientation = Orientation.Vertical;
				newBounds.Width = width * childPercentage;
				newBounds.Height = height;
				getOffsetForChild = n => n.BoundsF.Height;
			}
			else if (orientation == Orientation.Vertical)
			{
				nextOrientation = Orientation.Horizontal;
				newBounds.Width = width;
				newBounds.Height = height * childPercentage;
				getOffsetForChild = n => n.BoundsF.Width;
			}
			else
			{
				throw new Exception();
			}

			// apply the margin if we're a branch.
			newBounds = node.IsBranch
				? newBounds.ApplyPadding(node.BranchMargin ?? view.NodeBranchMargin)
				: newBounds;


			HandleCollapsedBranchHeight(node, view, ref newBounds);

			node.BoundsF = newBounds;

			float totalOffset = 0;

			foreach (TreemapNode child in node.Nodes)
			{
				PointF childPosition = Point.Empty;

				if (nextOrientation == Orientation.Vertical)
				{
					childPosition.X = node.BoundsF.X;
					childPosition.Y = node.BoundsF.Y + headerHeight + totalOffset;
				}
				else
				{
					childPosition.X = node.BoundsF.X + totalOffset;
					childPosition.Y = node.BoundsF.Y + headerHeight;
				}

				// collapsed branch nodes don't show their children,
				// so squish em down to nothing.
				if (node.IsBranch && node.IsCollapsed)
				{
					child.BoundsF = RectangleF.Empty;
					continue;
				}

				child.BoundsF = new RectangleF(childPosition, newBounds.Size);
				SliceAndDice(child, node, nextOrientation);
				totalOffset += getOffsetForChild(child);
			}
		}
	}
}
