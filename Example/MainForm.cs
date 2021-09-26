using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using cmdwtf.Treemap;

using Data = Example.SampleData.MusicPlaybackData;

namespace Example
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Designer generated.")]
	public partial class MainForm : Form
	{
		private TreemapView _toolStripTarget;

		private readonly List<IComparer<TreemapNode>> _sorters = new()
		{
			TreemapNodeSorters.Descending,
			TreemapNodeSorters.Ascending,
			TreemapNodeSorters.Random,
			TreemapNodeSorters.Unsorted,
		};

		public MainForm()
		{
			InitializeComponent();

			InitalizeViews();
		}

		private void InitalizeViews()
		{
			// Some sample data to display.
			Data data = SampleData.GetData();

			// First Tab - the TreemapView
			TreemapView_SampleData(treemapView, data);
			treemapView.TreemapViewNodeSorter = TreemapNodeSorters.Descending;

			// Set up the ToolStrip for the first TreemapView.
			SetToolStripTargetView(treemapView);

			// Second tab - the same data as the first, but in a TreeView
			TreeView_SampleData(treeView, data);

			// Third tab - a super simple sample to get started with
			SimpleTest(treemapViewSimple);
			treemapViewSimple.TreemapViewNodeSorter = TreemapNodeSorters.Descending;
		}

		private void TreemapView_SampleData(TreemapView tm, Data data)
		{
			tm.BeginUpdate();

			TreemapNode root = tm.Nodes.Add(data.Name);
			SampleDataToNodes(root, data.Children, (node, data) =>
			{
				if (data.Info != null)
				{
					node.BackColor = ColorTranslator.FromHtml(data.Info.Color);
					node.Value = data.Info.Area;
				}
			});

			tm.ExpandAll();
			tm.EndUpdate();
		}

		private void TreeView_SampleData(TreeView tv, Data data)
		{
			tv.BeginUpdate();

			TreeNode root = tv.Nodes.Add(data.Name);
			SampleDataToNodes(root, data.Children, (node, data) =>
			{
				if (data.Info != null)
				{
					node.BackColor = ColorTranslator.FromHtml(data.Info.Color);
					node.Text = $"{node.Text} - {data.Info.Playcount}";
				}
			});

			tv.ExpandAll();
			tv.EndUpdate();
		}

		private void SampleDataToNodes<T>(T parent, List<Data> dataList, Action<T, Data> onNodeCreated = null) where T : class
		{
			foreach (Data data in dataList)
			{
				T node = parent switch
				{
					TreemapNode tmn => tmn.Nodes.Add(data.Name) as T,
					TreeNode tn => tn.Nodes.Add(data.Name) as T,
					_ => throw new InvalidOperationException()
				};

				onNodeCreated?.Invoke(node, data);

				SampleDataToNodes(node, data.Children, onNodeCreated);
			}
		}

		private void SimpleTest(TreemapView tm)
		{
			TreemapNode testRoot = tm.Nodes.Add("My First node!");

			TreemapNode firstChild = testRoot.Nodes.Add("My First Child");
			TreemapNode secondChild = testRoot.Nodes.Add("My Second Child");

			firstChild.Nodes.Add("First-A");
			firstChild.Nodes.Add("First-B");
			firstChild.Nodes.Add("First-C");
			firstChild.Nodes.Add("First-D");

			secondChild.Nodes.Add("Second-A");
			secondChild.Nodes.Add("Second-B");
			secondChild.Nodes.Add("Second-C");
			secondChild.Nodes.Add("Second-D");

			var cloned = secondChild.Clone() as TreemapNode;
			cloned.Text = cloned.Text.Replace("Second", "Cloned");
			foreach (TreemapNode n in cloned.Nodes)
			{
				n.Text = n.Text.Replace("Second", "Cloned");
				// give these nodes the custom strip
				n.ContextMenuStrip = contextMenuStripCustom;
			}
			testRoot.Nodes.Add(cloned);
		}

		private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == tabPageTreemapView)
			{
				SetToolStripTargetView(treemapView);
			}
			else if (e.TabPage == tabPageTreemapViewSimple)
			{
				SetToolStripTargetView(treemapViewSimple);
			}
		}

		#region Tool Strip

		private void SetToolStripTargetView(TreemapView target)
		{
			_toolStripTarget = target;

			// draw styles
			UpdateDrawStylesChecked(target);

			// check boxes
			toolStripButtonCheckBoxes.Checked = target.CheckBoxes;

			// plus minus
			toolStripButtonShowPlusMinus.Checked = target.ShowPlusMinus;

			// headers
			toolStripButtonShowBranchesAsHeaders.Checked = target.ShowBranchesAsHeaders;

			// sort -- set the sort views for the sorts that use it.
			TreemapNodeSorters.Ascending.SetView(target);
			TreemapNodeSorters.Descending.SetView(target);

			// sort -- set checked menu item
			UpdateSortChecked(target);

			// hot tracking
			toolStripButtonHotTracking.Checked = target.HotTracking;

			// grid
			toolStripButtonShowGrid.Checked = target.ShowGrid;
		}

		private void UpdateDrawStylesChecked(TreemapView target)
		{
			flatToolStripMenuItem.Checked = target.NodeLeafDrawStyle == TreemapNodeDrawStyle.Flat;
			gradientRadialToolStripMenuItem.Checked = target.NodeLeafDrawStyle == TreemapNodeDrawStyle.Gradient;
			gradientHorizontalToolStripMenuItem.Checked = target.NodeLeafDrawStyle == TreemapNodeDrawStyle.GradientHorizontal;
			gradientVerticalToolStripMenuItem.Checked = target.NodeLeafDrawStyle == TreemapNodeDrawStyle.GradientVertical;
		}

		private void UpdateSortChecked(TreemapView target)
		{
			ascendingToolStripMenuItem.Checked = target.TreemapViewNodeSorter == TreemapNodeSorters.Ascending;
			descendingToolStripMenuItem.Checked = target.TreemapViewNodeSorter == TreemapNodeSorters.Descending;
			randomToolStripMenuItem.Checked = target.TreemapViewNodeSorter == TreemapNodeSorters.Random;
			unsortedToolStripMenuItem.Checked = target.TreemapViewNodeSorter == TreemapNodeSorters.Unsorted;
		}

		private void toolStripSplitButtonDrawStyle_ButtonClick(object sender, EventArgs e)
		{
			_toolStripTarget.NodeLeafDrawStyle = _toolStripTarget.NodeLeafDrawStyle.Next();
			UpdateDrawStylesChecked(_toolStripTarget);
		}

		private void flatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.NodeLeafDrawStyle = TreemapNodeDrawStyle.Flat;
			UpdateDrawStylesChecked(_toolStripTarget);
		}

		private void gradientRadialToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.NodeLeafDrawStyle = TreemapNodeDrawStyle.Gradient;
			UpdateDrawStylesChecked(_toolStripTarget);
		}

		private void gradientHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.NodeLeafDrawStyle = TreemapNodeDrawStyle.GradientHorizontal;
			UpdateDrawStylesChecked(_toolStripTarget);
		}

		private void gradientVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.NodeLeafDrawStyle = TreemapNodeDrawStyle.GradientVertical;
			UpdateDrawStylesChecked(_toolStripTarget);
		}

		private void toolStripButtonCheckBoxes_Click(object sender, EventArgs e)
		{
			_toolStripTarget.CheckBoxes = toolStripButtonCheckBoxes.Checked;
		}

		private void toolStripButtonShowPlusMinus_Click(object sender, EventArgs e)
		{
			_toolStripTarget.ShowPlusMinus = toolStripButtonShowPlusMinus.Checked;
		}

		private void toolStripButtonShowBranchesAsHeaders_Click(object sender, EventArgs e)
		{
			_toolStripTarget.ShowBranchesAsHeaders = toolStripButtonShowBranchesAsHeaders.Checked;
		}

		private void toolStripSplitButtonSort_ButtonClick(object sender, EventArgs e)
		{
			int index = _sorters.IndexOf(_toolStripTarget.TreemapViewNodeSorter);

			if (++index >= _sorters.Count)
			{
				index = 0;
			}

			_toolStripTarget.TreemapViewNodeSorter = _sorters[index];
			UpdateSortChecked(_toolStripTarget);
		}

		private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.TreemapViewNodeSorter = TreemapNodeSorters.Ascending;
			UpdateSortChecked(_toolStripTarget);
		}

		private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.TreemapViewNodeSorter = TreemapNodeSorters.Descending;
			UpdateSortChecked(_toolStripTarget);
		}

		private void randomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.TreemapViewNodeSorter = TreemapNodeSorters.Random;
			UpdateSortChecked(_toolStripTarget);
		}

		private void unsortedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_toolStripTarget.TreemapViewNodeSorter = TreemapNodeSorters.Unsorted;
			UpdateSortChecked(_toolStripTarget);
		}
		private void toolStripButtonHotTracking_Click(object sender, EventArgs e)
		{
			_toolStripTarget.HotTracking = toolStripButtonHotTracking.Checked;
		}

		private void toolStripButtonShowGrid_Click(object sender, EventArgs e)
		{
			_toolStripTarget.ShowGrid = toolStripButtonShowGrid.Checked;
		}

		#endregion Tool Strip
	}
}
