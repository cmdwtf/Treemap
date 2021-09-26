
namespace Example
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat2 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat3 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat4 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat5 = new System.Drawing.StringFormat();
            System.Drawing.StringFormat stringFormat6 = new System.Drawing.StringFormat();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageTreemapView = new System.Windows.Forms.TabPage();
            this.treemapView = new cmdwtf.Treemap.TreemapView();
            this.tabPageTreeView = new System.Windows.Forms.TabPage();
            this.treeView = new System.Windows.Forms.TreeView();
            this.tabPageTreemapViewSimple = new System.Windows.Forms.TabPage();
            this.treemapViewSimple = new cmdwtf.Treemap.TreemapView();
            this.contextMenuStripDefault = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListDefault = new System.Windows.Forms.ImageList(this.components);
            this.imageListState = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripCustom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageTreemapView.SuspendLayout();
            this.tabPageTreeView.SuspendLayout();
            this.tabPageTreemapViewSimple.SuspendLayout();
            this.contextMenuStripDefault.SuspendLayout();
            this.contextMenuStripCustom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageTreemapView);
            this.tabControl.Controls.Add(this.tabPageTreeView);
            this.tabControl.Controls.Add(this.tabPageTreemapViewSimple);
            this.tabControl.Location = new System.Drawing.Point(22, 26);
            this.tabControl.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1440, 908);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageTreemapView
            // 
            this.tabPageTreemapView.Controls.Add(this.treemapView);
            this.tabPageTreemapView.Location = new System.Drawing.Point(8, 46);
            this.tabPageTreemapView.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapView.Name = "tabPageTreemapView";
            this.tabPageTreemapView.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapView.Size = new System.Drawing.Size(1424, 854);
            this.tabPageTreemapView.TabIndex = 0;
            this.tabPageTreemapView.Text = "Sample Data (TreemapView)";
            this.tabPageTreemapView.UseVisualStyleBackColor = true;
            // 
            // treemapView
            // 
            this.treemapView.CheckBoxes = true;
            this.treemapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treemapView.HotTracking = true;
            this.treemapView.Location = new System.Drawing.Point(6, 6);
            this.treemapView.Margin = new System.Windows.Forms.Padding(4);
            this.treemapView.Name = "treemapView";
            stringFormat1.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat1.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.None;
            this.treemapView.NoDataTextFormat = stringFormat1;
            this.treemapView.NodeBranchFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            stringFormat2.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat2.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat2.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat2.Trimming = System.Drawing.StringTrimming.None;
            this.treemapView.NodeBranchHeaderStringFormat = stringFormat2;
            this.treemapView.NodeBranchMargin = new System.Windows.Forms.Padding(0);
            this.treemapView.NodeLeafDrawStyle = cmdwtf.Treemap.TreemapNodeDrawStyle.GradientHorizontal;
            this.treemapView.NodeLeafFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.treemapView.NodeLeafPadding = new System.Windows.Forms.Padding(0);
            stringFormat3.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat3.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat3.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat3.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat3.Trimming = System.Drawing.StringTrimming.None;
            this.treemapView.NodeLeafStringFormat = stringFormat3;
            this.treemapView.ShowNodeToolTips = true;
            this.treemapView.Size = new System.Drawing.Size(1412, 842);
            this.treemapView.TabIndex = 1;
            this.treemapView.Text = "Sample Data Treemap. The \'Text\' property is shown if there is no data.\r\nIf Text i" +
    "s null or empty, a generic no data message will be shown.";
            // 
            // tabPageTreeView
            // 
            this.tabPageTreeView.Controls.Add(this.treeView);
            this.tabPageTreeView.Location = new System.Drawing.Point(8, 46);
            this.tabPageTreeView.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageTreeView.Name = "tabPageTreeView";
            this.tabPageTreeView.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageTreeView.Size = new System.Drawing.Size(1424, 854);
            this.tabPageTreeView.TabIndex = 1;
            this.tabPageTreeView.Text = "Sample Data (TreeView)";
            this.tabPageTreeView.UseVisualStyleBackColor = true;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(6, 6);
            this.treeView.Margin = new System.Windows.Forms.Padding(6);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(1412, 842);
            this.treeView.TabIndex = 0;
            // 
            // tabPageTreemapViewSimple
            // 
            this.tabPageTreemapViewSimple.Controls.Add(this.treemapViewSimple);
            this.tabPageTreemapViewSimple.Location = new System.Drawing.Point(8, 46);
            this.tabPageTreemapViewSimple.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapViewSimple.Name = "tabPageTreemapViewSimple";
            this.tabPageTreemapViewSimple.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapViewSimple.Size = new System.Drawing.Size(1424, 854);
            this.tabPageTreemapViewSimple.TabIndex = 2;
            this.tabPageTreemapViewSimple.Text = "Simple Data (TreemapView)";
            this.tabPageTreemapViewSimple.UseVisualStyleBackColor = true;
            // 
            // treemapViewSimple
            // 
            this.treemapViewSimple.CheckBoxes = true;
            this.treemapViewSimple.ContextMenuStrip = this.contextMenuStripDefault;
            this.treemapViewSimple.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treemapViewSimple.HideSelection = false;
            this.treemapViewSimple.ImageIndex = 0;
            this.treemapViewSimple.ImageList = this.imageListDefault;
            this.treemapViewSimple.Location = new System.Drawing.Point(6, 6);
            this.treemapViewSimple.Margin = new System.Windows.Forms.Padding(4);
            this.treemapViewSimple.Name = "treemapViewSimple";
            stringFormat4.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat4.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat4.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat4.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat4.Trimming = System.Drawing.StringTrimming.None;
            this.treemapViewSimple.NoDataTextFormat = stringFormat4;
            this.treemapViewSimple.NodeBranchFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            stringFormat5.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat5.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat5.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat5.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat5.Trimming = System.Drawing.StringTrimming.None;
            this.treemapViewSimple.NodeBranchHeaderStringFormat = stringFormat5;
            this.treemapViewSimple.NodeBranchMargin = new System.Windows.Forms.Padding(0);
            this.treemapViewSimple.NodeImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.treemapViewSimple.NodeLeafFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.treemapViewSimple.NodeLeafPadding = new System.Windows.Forms.Padding(0);
            stringFormat6.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat6.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat6.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat6.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat6.Trimming = System.Drawing.StringTrimming.None;
            this.treemapViewSimple.NodeLeafStringFormat = stringFormat6;
            this.treemapViewSimple.SelectedImageIndex = 2;
            this.treemapViewSimple.ShowNodeToolTips = true;
            this.treemapViewSimple.ShowPlusMinus = true;
            this.treemapViewSimple.Size = new System.Drawing.Size(1412, 842);
            this.treemapViewSimple.StateImageList = this.imageListState;
            this.treemapViewSimple.TabIndex = 1;
            // 
            // contextMenuStripDefault
            // 
            this.contextMenuStripDefault.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStripDefault.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.toolStripMenuItem3});
            this.contextMenuStripDefault.Name = "contextMenuStripDefault";
            this.contextMenuStripDefault.Size = new System.Drawing.Size(172, 124);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(171, 38);
            this.toolStripMenuItem1.Text = "Default";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(171, 38);
            this.toolStripMenuItem2.Text = "Context";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(171, 38);
            this.toolStripMenuItem3.Text = "Menu";
            // 
            // imageListDefault
            // 
            this.imageListDefault.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListDefault.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDefault.ImageStream")));
            this.imageListDefault.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDefault.Images.SetKeyName(0, "bullet-black.png");
            this.imageListDefault.Images.SetKeyName(1, "bullet-black-alt.png");
            this.imageListDefault.Images.SetKeyName(2, "bullet-blue.png");
            this.imageListDefault.Images.SetKeyName(3, "bullet-blue-alt.png");
            // 
            // imageListState
            // 
            this.imageListState.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListState.ImageStream")));
            this.imageListState.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListState.Images.SetKeyName(0, "bullet-orange.png");
            this.imageListState.Images.SetKeyName(1, "bullet-orange-alt.png");
            // 
            // contextMenuStripCustom
            // 
            this.contextMenuStripCustom.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStripCustom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4});
            this.contextMenuStripCustom.Name = "contextMenuStripCustom";
            this.contextMenuStripCustom.Size = new System.Drawing.Size(331, 42);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(330, 38);
            this.toolStripMenuItem4.Text = "Custom Context Menu";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1486, 960);
            this.Controls.Add(this.tabControl);
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "cmdwtf.Treemap Example";
            this.tabControl.ResumeLayout(false);
            this.tabPageTreemapView.ResumeLayout(false);
            this.tabPageTreeView.ResumeLayout(false);
            this.tabPageTreemapViewSimple.ResumeLayout(false);
            this.contextMenuStripDefault.ResumeLayout(false);
            this.contextMenuStripCustom.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageTreemapView;
		private cmdwtf.Treemap.TreemapView treemapView;
		private System.Windows.Forms.TabPage tabPageTreeView;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.TabPage tabPageTreemapViewSimple;
		private cmdwtf.Treemap.TreemapView treemapViewSimple;
		private System.Windows.Forms.ImageList imageListState;
		private System.Windows.Forms.ImageList imageListDefault;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripDefault;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripCustom;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
		private System.Windows.Forms.ToolTip toolTip;
	}
}

