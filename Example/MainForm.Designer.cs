
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
            this.toolStripMenuItemDefaultDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDefaultContext = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorDefaultSeperator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemDefaultMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListDefault = new System.Windows.Forms.ImageList(this.components);
            this.imageListState = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripCustom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCustomCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonDrawStyle = new System.Windows.Forms.ToolStripSplitButton();
            this.flatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradientRadialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradientHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradientVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonCheckBoxes = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowPlusMinus = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowBranchesAsHeaders = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButtonSort = new System.Windows.Forms.ToolStripSplitButton();
            this.descendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ascendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unsortedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonHotTracking = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowGrid = new System.Windows.Forms.ToolStripButton();
            this.tabControl.SuspendLayout();
            this.tabPageTreemapView.SuspendLayout();
            this.tabPageTreeView.SuspendLayout();
            this.tabPageTreemapViewSimple.SuspendLayout();
            this.contextMenuStripDefault.SuspendLayout();
            this.contextMenuStripCustom.SuspendLayout();
            this.toolStripMain.SuspendLayout();
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
            this.tabControl.Location = new System.Drawing.Point(0, 48);
            this.tabControl.Margin = new System.Windows.Forms.Padding(6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1774, 1281);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // tabPageTreemapView
            // 
            this.tabPageTreemapView.Controls.Add(this.treemapView);
            this.tabPageTreemapView.Location = new System.Drawing.Point(8, 46);
            this.tabPageTreemapView.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapView.Name = "tabPageTreemapView";
            this.tabPageTreemapView.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapView.Size = new System.Drawing.Size(1758, 1227);
            this.tabPageTreemapView.TabIndex = 0;
            this.tabPageTreemapView.Text = "Sample Data (TreemapView)";
            this.tabPageTreemapView.UseVisualStyleBackColor = true;
            // 
            // treemapView
            // 
            this.treemapView.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.treemapView.NodeBranchBackColor = System.Drawing.SystemColors.ControlLight;
            this.treemapView.NodeBranchFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            stringFormat2.Alignment = System.Drawing.StringAlignment.Center;
            stringFormat2.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.LineLimit) 
            | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat2.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat2.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat2.Trimming = System.Drawing.StringTrimming.None;
            this.treemapView.NodeBranchHeaderStringFormat = stringFormat2;
            this.treemapView.NodeBranchMargin = new System.Windows.Forms.Padding(0);
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
            this.treemapView.Size = new System.Drawing.Size(1746, 1215);
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
            this.tabPageTreeView.Size = new System.Drawing.Size(1470, 858);
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
            this.treeView.Size = new System.Drawing.Size(1458, 846);
            this.treeView.TabIndex = 0;
            // 
            // tabPageTreemapViewSimple
            // 
            this.tabPageTreemapViewSimple.Controls.Add(this.treemapViewSimple);
            this.tabPageTreemapViewSimple.Location = new System.Drawing.Point(8, 46);
            this.tabPageTreemapViewSimple.Margin = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapViewSimple.Name = "tabPageTreemapViewSimple";
            this.tabPageTreemapViewSimple.Padding = new System.Windows.Forms.Padding(6);
            this.tabPageTreemapViewSimple.Size = new System.Drawing.Size(1470, 858);
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
            this.treemapViewSimple.Size = new System.Drawing.Size(1458, 846);
            this.treemapViewSimple.StateImageList = this.imageListState;
            this.treemapViewSimple.TabIndex = 1;
            // 
            // contextMenuStripDefault
            // 
            this.contextMenuStripDefault.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStripDefault.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDefaultDefault,
            this.toolStripMenuItemDefaultContext,
            this.toolStripSeparatorDefaultSeperator,
            this.toolStripMenuItemDefaultMenu});
            this.contextMenuStripDefault.Name = "contextMenuStripDefault";
            this.contextMenuStripDefault.Size = new System.Drawing.Size(188, 130);
            // 
            // toolStripMenuItemDefaultDefault
            // 
            this.toolStripMenuItemDefaultDefault.Image = global::Example.Properties.Resources.briefcase;
            this.toolStripMenuItemDefaultDefault.Name = "toolStripMenuItemDefaultDefault";
            this.toolStripMenuItemDefaultDefault.Size = new System.Drawing.Size(187, 40);
            this.toolStripMenuItemDefaultDefault.Text = "Default";
            // 
            // toolStripMenuItemDefaultContext
            // 
            this.toolStripMenuItemDefaultContext.Image = global::Example.Properties.Resources.address_book;
            this.toolStripMenuItemDefaultContext.Name = "toolStripMenuItemDefaultContext";
            this.toolStripMenuItemDefaultContext.Size = new System.Drawing.Size(187, 40);
            this.toolStripMenuItemDefaultContext.Text = "Context";
            // 
            // toolStripSeparatorDefaultSeperator
            // 
            this.toolStripSeparatorDefaultSeperator.Name = "toolStripSeparatorDefaultSeperator";
            this.toolStripSeparatorDefaultSeperator.Size = new System.Drawing.Size(184, 6);
            // 
            // toolStripMenuItemDefaultMenu
            // 
            this.toolStripMenuItemDefaultMenu.Image = global::Example.Properties.Resources.window_vista;
            this.toolStripMenuItemDefaultMenu.Name = "toolStripMenuItemDefaultMenu";
            this.toolStripMenuItemDefaultMenu.Size = new System.Drawing.Size(187, 40);
            this.toolStripMenuItemDefaultMenu.Text = "Menu";
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
            this.toolStripMenuItemCustomCustom});
            this.contextMenuStripCustom.Name = "contextMenuStripCustom";
            this.contextMenuStripCustom.Size = new System.Drawing.Size(347, 44);
            // 
            // toolStripMenuItemCustomCustom
            // 
            this.toolStripMenuItemCustomCustom.Image = global::Example.Properties.Resources.flag;
            this.toolStripMenuItemCustomCustom.Name = "toolStripMenuItemCustomCustom";
            this.toolStripMenuItemCustomCustom.Size = new System.Drawing.Size(346, 40);
            this.toolStripMenuItemCustomCustom.Text = "Custom Context Menu";
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonDrawStyle,
            this.toolStripButtonCheckBoxes,
            this.toolStripButtonShowPlusMinus,
            this.toolStripButtonShowBranchesAsHeaders,
            this.toolStripSplitButtonSort,
            this.toolStripButtonHotTracking,
            this.toolStripButtonShowGrid});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1774, 42);
            this.toolStripMain.TabIndex = 2;
            this.toolStripMain.Text = "Tool Strip";
            // 
            // toolStripSplitButtonDrawStyle
            // 
            this.toolStripSplitButtonDrawStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flatToolStripMenuItem,
            this.gradientRadialToolStripMenuItem,
            this.gradientHorizontalToolStripMenuItem,
            this.gradientVerticalToolStripMenuItem});
            this.toolStripSplitButtonDrawStyle.Image = global::Example.Properties.Resources.window_xp_edit;
            this.toolStripSplitButtonDrawStyle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonDrawStyle.Name = "toolStripSplitButtonDrawStyle";
            this.toolStripSplitButtonDrawStyle.Size = new System.Drawing.Size(251, 36);
            this.toolStripSplitButtonDrawStyle.Text = "Node Draw Style";
            this.toolStripSplitButtonDrawStyle.ButtonClick += new System.EventHandler(this.toolStripSplitButtonDrawStyle_ButtonClick);
            // 
            // flatToolStripMenuItem
            // 
            this.flatToolStripMenuItem.CheckOnClick = true;
            this.flatToolStripMenuItem.Image = global::Example.Properties.Resources.bullet_black_alt;
            this.flatToolStripMenuItem.Name = "flatToolStripMenuItem";
            this.flatToolStripMenuItem.Size = new System.Drawing.Size(369, 44);
            this.flatToolStripMenuItem.Text = "Flat";
            this.flatToolStripMenuItem.Click += new System.EventHandler(this.flatToolStripMenuItem_Click);
            // 
            // gradientRadialToolStripMenuItem
            // 
            this.gradientRadialToolStripMenuItem.CheckOnClick = true;
            this.gradientRadialToolStripMenuItem.Image = global::Example.Properties.Resources.bullet_purple_alt;
            this.gradientRadialToolStripMenuItem.Name = "gradientRadialToolStripMenuItem";
            this.gradientRadialToolStripMenuItem.Size = new System.Drawing.Size(369, 44);
            this.gradientRadialToolStripMenuItem.Text = "Gradient (Radial)";
            this.gradientRadialToolStripMenuItem.Click += new System.EventHandler(this.gradientRadialToolStripMenuItem_Click);
            // 
            // gradientHorizontalToolStripMenuItem
            // 
            this.gradientHorizontalToolStripMenuItem.CheckOnClick = true;
            this.gradientHorizontalToolStripMenuItem.Image = global::Example.Properties.Resources.bullet_orange_alt;
            this.gradientHorizontalToolStripMenuItem.Name = "gradientHorizontalToolStripMenuItem";
            this.gradientHorizontalToolStripMenuItem.Size = new System.Drawing.Size(369, 44);
            this.gradientHorizontalToolStripMenuItem.Text = "Gradient (Horizontal)";
            this.gradientHorizontalToolStripMenuItem.Click += new System.EventHandler(this.gradientHorizontalToolStripMenuItem_Click);
            // 
            // gradientVerticalToolStripMenuItem
            // 
            this.gradientVerticalToolStripMenuItem.CheckOnClick = true;
            this.gradientVerticalToolStripMenuItem.Image = global::Example.Properties.Resources.bullet_blue_alt;
            this.gradientVerticalToolStripMenuItem.Name = "gradientVerticalToolStripMenuItem";
            this.gradientVerticalToolStripMenuItem.Size = new System.Drawing.Size(369, 44);
            this.gradientVerticalToolStripMenuItem.Text = "Gradient (Vertical)";
            this.gradientVerticalToolStripMenuItem.Click += new System.EventHandler(this.gradientVerticalToolStripMenuItem_Click);
            // 
            // toolStripButtonCheckBoxes
            // 
            this.toolStripButtonCheckBoxes.CheckOnClick = true;
            this.toolStripButtonCheckBoxes.Image = global::Example.Properties.Resources.system_tick_alt;
            this.toolStripButtonCheckBoxes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCheckBoxes.Name = "toolStripButtonCheckBoxes";
            this.toolStripButtonCheckBoxes.Size = new System.Drawing.Size(177, 36);
            this.toolStripButtonCheckBoxes.Text = "CheckBoxes";
            this.toolStripButtonCheckBoxes.Click += new System.EventHandler(this.toolStripButtonCheckBoxes_Click);
            // 
            // toolStripButtonShowPlusMinus
            // 
            this.toolStripButtonShowPlusMinus.CheckOnClick = true;
            this.toolStripButtonShowPlusMinus.Image = global::Example.Properties.Resources.system_save_alt;
            this.toolStripButtonShowPlusMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowPlusMinus.Name = "toolStripButtonShowPlusMinus";
            this.toolStripButtonShowPlusMinus.Size = new System.Drawing.Size(217, 36);
            this.toolStripButtonShowPlusMinus.Text = "ShowPlusMinus";
            this.toolStripButtonShowPlusMinus.Click += new System.EventHandler(this.toolStripButtonShowPlusMinus_Click);
            // 
            // toolStripButtonShowBranchesAsHeaders
            // 
            this.toolStripButtonShowBranchesAsHeaders.CheckOnClick = true;
            this.toolStripButtonShowBranchesAsHeaders.Image = global::Example.Properties.Resources.table_column_alt;
            this.toolStripButtonShowBranchesAsHeaders.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowBranchesAsHeaders.Name = "toolStripButtonShowBranchesAsHeaders";
            this.toolStripButtonShowBranchesAsHeaders.Size = new System.Drawing.Size(316, 36);
            this.toolStripButtonShowBranchesAsHeaders.Text = "ShowBranchesAsHeaders";
            this.toolStripButtonShowBranchesAsHeaders.Click += new System.EventHandler(this.toolStripButtonShowBranchesAsHeaders_Click);
            // 
            // toolStripSplitButtonSort
            // 
            this.toolStripSplitButtonSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.descendingToolStripMenuItem,
            this.ascendingToolStripMenuItem,
            this.randomToolStripMenuItem,
            this.unsortedToolStripMenuItem});
            this.toolStripSplitButtonSort.Image = global::Example.Properties.Resources.notepad;
            this.toolStripSplitButtonSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonSort.Name = "toolStripSplitButtonSort";
            this.toolStripSplitButtonSort.Size = new System.Drawing.Size(116, 36);
            this.toolStripSplitButtonSort.Text = "Sort";
            this.toolStripSplitButtonSort.ButtonClick += new System.EventHandler(this.toolStripSplitButtonSort_ButtonClick);
            // 
            // descendingToolStripMenuItem
            // 
            this.descendingToolStripMenuItem.CheckOnClick = true;
            this.descendingToolStripMenuItem.Image = global::Example.Properties.Resources.sort_desc_alt;
            this.descendingToolStripMenuItem.Name = "descendingToolStripMenuItem";
            this.descendingToolStripMenuItem.Size = new System.Drawing.Size(273, 44);
            this.descendingToolStripMenuItem.Text = "Descending";
            this.descendingToolStripMenuItem.Click += new System.EventHandler(this.descendingToolStripMenuItem_Click);
            // 
            // ascendingToolStripMenuItem
            // 
            this.ascendingToolStripMenuItem.CheckOnClick = true;
            this.ascendingToolStripMenuItem.Image = global::Example.Properties.Resources.sort_asc;
            this.ascendingToolStripMenuItem.Name = "ascendingToolStripMenuItem";
            this.ascendingToolStripMenuItem.Size = new System.Drawing.Size(273, 44);
            this.ascendingToolStripMenuItem.Text = "Ascending";
            this.ascendingToolStripMenuItem.Click += new System.EventHandler(this.ascendingToolStripMenuItem_Click);
            // 
            // randomToolStripMenuItem
            // 
            this.randomToolStripMenuItem.CheckOnClick = true;
            this.randomToolStripMenuItem.Image = global::Example.Properties.Resources.system_question_alt;
            this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            this.randomToolStripMenuItem.Size = new System.Drawing.Size(273, 44);
            this.randomToolStripMenuItem.Text = "Random";
            this.randomToolStripMenuItem.Click += new System.EventHandler(this.randomToolStripMenuItem_Click);
            // 
            // unsortedToolStripMenuItem
            // 
            this.unsortedToolStripMenuItem.CheckOnClick = true;
            this.unsortedToolStripMenuItem.Image = global::Example.Properties.Resources.form_reset;
            this.unsortedToolStripMenuItem.Name = "unsortedToolStripMenuItem";
            this.unsortedToolStripMenuItem.Size = new System.Drawing.Size(273, 44);
            this.unsortedToolStripMenuItem.Text = "Unsorted";
            this.unsortedToolStripMenuItem.Click += new System.EventHandler(this.unsortedToolStripMenuItem_Click);
            // 
            // toolStripButtonHotTracking
            // 
            this.toolStripButtonHotTracking.CheckOnClick = true;
            this.toolStripButtonHotTracking.Image = global::Example.Properties.Resources.link;
            this.toolStripButtonHotTracking.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHotTracking.Name = "toolStripButtonHotTracking";
            this.toolStripButtonHotTracking.Size = new System.Drawing.Size(177, 36);
            this.toolStripButtonHotTracking.Text = "HotTracking";
            this.toolStripButtonHotTracking.Click += new System.EventHandler(this.toolStripButtonHotTracking_Click);
            // 
            // toolStripButtonShowGrid
            // 
            this.toolStripButtonShowGrid.CheckOnClick = true;
            this.toolStripButtonShowGrid.Image = global::Example.Properties.Resources.table;
            this.toolStripButtonShowGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowGrid.Name = "toolStripButtonShowGrid";
            this.toolStripButtonShowGrid.Size = new System.Drawing.Size(152, 36);
            this.toolStripButtonShowGrid.Text = "ShowGrid";
            this.toolStripButtonShowGrid.Click += new System.EventHandler(this.toolStripButtonShowGrid_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1774, 1329);
            this.Controls.Add(this.toolStripMain);
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
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDefaultDefault;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDefaultContext;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDefaultSeperator;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDefaultMenu;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripCustom;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCustomCustom;
		private System.Windows.Forms.ToolStrip toolStripMain;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonDrawStyle;
		private System.Windows.Forms.ToolStripMenuItem flatToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gradientRadialToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gradientHorizontalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gradientVerticalToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonCheckBoxes;
		private System.Windows.Forms.ToolStripButton toolStripButtonShowPlusMinus;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonSort;
		private System.Windows.Forms.ToolStripMenuItem ascendingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem descendingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem randomToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonShowBranchesAsHeaders;
		private System.Windows.Forms.ToolStripMenuItem unsortedToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonHotTracking;
		private System.Windows.Forms.ToolStripButton toolStripButtonShowGrid;
	}
}

