using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using cmdwtf.Treemap;

using Data = Example.SampleData.MusicPlaybackData;

namespace Example
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			// Some sample data to display.
			Data data = SampleData.GetData();

			// First Tab - the TreemapView
			TreemapView_SampleData(treemapView, data);

			// Second tab - the same data as the first, but in a TreeView
			TreeView_SampleData(treeView, data);

			// Third tab - a super simple sample to get started with
			SimpleTest(treemapViewSimple);
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
	}
}
