using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using TreemapNodeValueDataType = System.Single;

namespace cmdwtf.Treemap.Tiling
{
	internal abstract class RectangleTilingStrategyBase : ITilingStrategy<RectangleF>
	{
		public abstract void RecalculateBounds(TreemapNode node, RectangleF newBounds);

		internal static float CalculateNodeArea(TreemapNode node)
			=> CalculateNodeArea(node.Nodes);

		internal static float CalculateNodeArea(IEnumerable<TreemapNode> nodes)
			=> nodes.Sum(child => child.Area);

		internal static void RecalculateChildAreas(TreemapNode node, TreemapView view)
		{
			node.Area = node.BoundsF.Width * node.BoundsF.Height;

			TreemapNodeValueDataType myValue = view.GetNodeValueForCurrentMode(node);

			foreach (TreemapNode child in node.Nodes)
			{
				TreemapNodeValueDataType childValue = view.GetNodeValueForCurrentMode(child);
				float childValuePercentage = childValue / myValue;
				child.Area = node.Area * childValuePercentage;
			}
		}

		internal static void HandleCollapsedBranchHeight(TreemapNode node, TreemapView view, ref RectangleF newBounds)
		{
			if (node.IsBranch && node.IsCollapsed)
			{
				newBounds.Height = node.GetBranchHeaderHeight(node.TreemapView);
#if TREEMAP_NODE_MAX_COLLAPSE
				// #COLLAPSING -- reduce newBounds.Width to the bare minimum for this branch?
				int contentWidth = 30;
				int plusMinusWidth = view.PlusMinusGlyphSizePixelsDevice(padded: true);
				int checkboxWidth = TreemapRenderer.LastCheckBoxGlyphSize.Width + TreemapRenderer.LastCheckboxPadding;
				int totalWidth = contentWidth + plusMinusWidth + checkboxWidth;
				newBounds.Width = totalWidth;
#endif // TREEMAP_NODE_MAX_COLLAPSE
			}
		}
	}
}
