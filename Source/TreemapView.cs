using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using Lazy;

using static cmdwtf.Toolkit.WinForms.Controls;
using static cmdwtf.Toolkit.WinForms.Drawing;
using static cmdwtf.Toolkit.WinForms.Forms;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Displays a hierarchical collection of labeled items, each represented by a <see cref="TreemapNode"/>.
	/// </summary>
	[DefaultEvent(nameof(AfterSelect))]
	[DefaultProperty(nameof(Nodes))]
	[Docking(DockingBehavior.Ask)]
	public class TreemapView : Control
	{
		// some fixed rendering settings.
		private const SmoothingMode _smoothingMode = SmoothingMode.AntiAlias;
		private const PixelOffsetMode _pixelOffsetMode = PixelOffsetMode.HighQuality;
		private const TextRenderingHint _textRenderingHint = TextRenderingHint.ClearTypeGridFit;
		private const InterpolationMode _interpolationMode = InterpolationMode.HighQualityBicubic;

		[Flags]
		private enum States
		{
			Unknown = 0x0000_0000,
			#region Internal States

			// behavior
			ShouldReTile = 0x0000_0001,
			IgnoreSelects = 0x0000_0002,

			// input
			MouseUpFired = 0x0000_0004,
			DoubleClickFired = 0x0000_0008,
			HoveredAlready = 0x0000_0010,

			#endregion Internal States

			#region Property States

			// appearance
			ShowGrid = 0x0001_0000,
			CheckBoxes = 0x0002_0000,
			RightToLeft = 0x0004_0000,
			ShowBranchesAsHeaders = 0x0008_0000,
			ShowModernPlusMinusGlyphs = 0x0010_0000,

			// behavior
			LabelEdit = 0x0020_0000,
			HotTracking = 0x0040_0000,
			HideSelection = 0x0080_0000,
			Sorted = 0x0100_0000,
			ShowPlusMinus = 0x0200_0000,
			ShowNodeToolTips = 0x0400_0000,
			FullBranchSelect = 0x0800_0000,

			#endregion Property States
		}

		private readonly RootTreemapNode _root;
		private readonly TreemapRenderingContext _nodePaintCallbacks;

		private TreemapNode _topNode;
		private int _updatesActive = 0;

		// state variables
		private cmdwtf.Toolkit.StateFlags<States> _state = new(0);
		private TreemapNode? _stateSelectedNode;
		private TreemapNode? _stateEditingNode;
		private TreemapNode? _stateHoveringNode;
		private TreemapNode? _stateMouseOverNode;
		private TreemapNode? _stateMouseDownNode;

		private MouseButtons _stateDownButton = MouseButtons.None;

		// property backing fields
		/// <summary>
		/// The backing field for <see cref="NodeBranchFont"/>.
		/// </summary>
		protected Font? _nodeBranchFont;

		/// <summary>
		/// The backing field for <see cref="NodeLeafFont"/>.
		/// </summary>
		protected Font? _nodeLeafFont;

		/// <summary>
		/// The backing field for <see cref="ToolTip"/>.
		/// </summary>
		protected ToolTip? _toolTip = null;

		/// <summary>
		/// The backing field for <see cref="TreemapViewNodeSorter"/>
		/// </summary>
		protected IComparer<TreemapNode> _treemapViewNodeSorter;

		/// <summary>
		/// Creates a new instance of a <see cref="TreemapView"/>.
		/// </summary>
		public TreemapView()
		{
			//SetStyle(ControlStyles.UserPaint, false);
			SetStyle(ControlStyles.StandardClick, true);
			SetStyle(ControlStyles.UseTextForAccessibility, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			// set initial states for non-false values.

			// property states
			_state[States.ShowGrid] = true;
			_state[States.ShowBranchesAsHeaders] = true;
			_state[States.ShowModernPlusMinusGlyphs] = true;
			_state[States.HideSelection] = true;
			_state[States.Sorted] = true;
			_state[States.FullBranchSelect] = true;

			_root = new RootTreemapNode(this)
			{
				Name = nameof(_root),
				Text = Text,
				Parent = null,
				Value = 0,
				BranchHeight = 0,
			};
			_root.SubtreeChanged += NodeEventRequiresRetile;

			_topNode = _root;

			_nodePaintCallbacks = new(this)
			{
				RaiseDrawNode = OnDrawNode,
				RaiseDrawBranchNode = OnDrawBranchNode,
				RaiseDrawGrid = OnDrawNodeGrid,
				RaiseDrawStateGlyph = OnDrawNodeState,
				RaiseDrawPlusMinusGlyph = OnDrawNodePlusMinus,
			};

			_treemapViewNodeSorter ??= new TreemapNodeSorter(this);

			ImageIndexer.Index = 0;
			SelectedImageIndexer.Index = 0;
		}

		#region Properties

		#region State Properties

		/// <summary>
		/// Gets if there are active calls to <see cref="BeginUpdate"/> that haven't
		/// been <see cref="EndUpdate"/>.
		/// </summary>
		[Browsable(false)]
		public bool IsUpdating => _updatesActive > 0;

		/// <inheritdoc cref="IsUpdating"/>
		[Browsable(false)]
		public bool Updating => IsUpdating;

		/// <summary>
		/// Returns true if the <see cref="TreemapView"/> has a value or any children nodes.
		/// </summary>
		[Browsable(false)]
		public virtual bool HasData => _root.Value > 0 || _root.GetNodeCount(false) > 0;

		/// <summary>
		/// Gets the number of <see cref="TreemapNode"/>s that can be fully visible in the <see cref="TreemapView"/>.
		/// </summary>
		/// <returns>
		/// The number of <see cref="TreemapNode"/> items that can be fully visible in
		/// the <see cref="TreemapView"/> control.
		/// </returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category(Categories.Appearance)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual int VisibleCount => _topNode.VisibleCount;

		#endregion State Properties

		#region Appearance Properties

		#region TreemapView Appearance Properties

		/// <summary>
		/// Gets or sets if this control should draw a grid.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(true)]
		public virtual bool ShowGrid
		{
			get => _state[States.ShowGrid];
			[InvalidatesControl]
			set => _state[States.ShowGrid] = value;
		}

		/// <summary>
		/// Gets or sets the color of this control's grid.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(typeof(Color), nameof(SystemColors.ControlText))]
		public virtual Color GridColor { get; [InvalidatesControl] set; } = SystemColors.ControlText;

		/// <summary>
		/// Gets or sets the color of this control's grid when a node is hot tracking.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(typeof(Color), nameof(SystemColors.HotTrack))]
		[Obsolete("This property doesn't work right, so it's marked protected for now.")]
		protected virtual Color GridHotTrackColor { get; [InvalidatesControl] set; } = SystemColors.HotTrack;

		/// <summary>
		/// Gets or sets a multiplier used when drawing the grid.
		/// Each layer deeper a node is, the grid width will be multiplied by this value.
		/// This causes deeper nodes to have thicker boarders. The default is 0.0f.
		/// </summary>
		[Category(Categories.Appearance)]
		[DefaultValue(0.0f)]
		public virtual float GridWidthDepthMultiplier { get; [InvalidatesControl] set; } = 0.0f;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <returns>
		/// A <see cref="Color"/> that represents the background color of the control. The
		/// default is the value of the <see cref="Control.BackColor"/> property.
		/// </returns>
		[Category(Categories.Appearance)]
		public override Color BackColor { get; [InvalidatesControl] set; } = SystemColors.Window;

		/// <summary>
		/// Gets or sets a value indicating whether check boxes are displayed next to the
		/// <see cref="TreemapNode"/>s in the <see cref="TreemapView"/>.
		/// </summary>
		/// <returns>
		/// true if a check box is displayed next to each <see cref="TreemapNode"/> in the <see cref="TreemapView"/>;
		/// otherwise, false. The default is false.
		/// </returns>
		[DefaultValue(false)]
		[Category(Categories.Appearance)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual bool CheckBoxes
		{
			get => _state[States.CheckBoxes];
			[InvalidatesControl]
			set => _state[States.CheckBoxes] = value;
		}

		/// <summary>
		/// Gets or sets the first fully-visible <see cref="TreemapNode"/> in the <see cref="TreemapView"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the first fully-visible tree
		/// node in the <see cref="TreemapView"/>.
		/// </returns>
		[Browsable(false)]
		[Category(Categories.Appearance)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual TreemapNode TopNode
		{
			get => _topNode;
			[InvalidatesControl, ForcesReTile]
			set => _topNode = value ?? _root;
		}

		/// <summary>
		/// Gets or sets the <see cref="TreemapNode"/> that is currently selected in the <see cref="TreemapView"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="TreemapNode"/> that is currently selected in the <see cref="TreemapView"/>
		/// control.
		/// </returns>
		[Browsable(false)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category(Categories.Appearance)]
		public virtual TreemapNode? SelectedNode
		{
			get => _stateSelectedNode;
			[InvalidatesControl]
			set => SetSelectedNode(value);
		}

		/// <summary>
		/// A <see cref="StringFormat"/> used to describe how the control will render the
		/// no data message if the <see cref="TreemapView"/> has no data.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Appearance)]
		public virtual StringFormat NoDataTextFormat { get; [InvalidatesControl] set; } = new StringFormat(StringFormat.GenericTypographic)
			.SetAlignments(ContentAlignment.MiddleCenter);

		/// <summary>
		/// How to align the text shown when the <see cref="TreemapView"/> has no data.
		/// </summary>
		[Browsable(true)]
		[Category(Categories.Appearance)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public virtual ContentAlignment NoDataTextAlignment
		{
			get => NoDataTextFormat.GetContentAlignment();
			[InvalidatesControl]
			set => NoDataTextFormat.SetAlignments(value);
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the <see cref="TreemapView"/>
		/// should be laid out from right-to-left.
		/// </summary>
		/// <returns>
		/// true if the control should be laid out from right-to-left; otherwise, false.
		/// The default is false.
		/// </returns>
		[DefaultValue(false)]
		[Localizable(true)]
		[Category(Categories.Appearance)]
		public virtual bool RightToLeftLayout
		{
			get => _state[States.RightToLeft];
			[InvalidatesControl]
			set => _state[States.RightToLeft] = value;
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the <see cref="TreemapView"/>
		/// should use the modern PlusMinus ("Expando") glyphs or not.
		/// </summary>
		/// <returns>
		/// true if the control should render PlusMinus glyphs using the modern system renderer.
		/// The default is true.
		/// </returns>
		[DefaultValue(true)]
		[Localizable(true)]
		[Category(Categories.Appearance)]
		public virtual bool UseModernPlusMinusGlyphs
		{
			get => _state[States.ShowModernPlusMinusGlyphs];
			[InvalidatesControl]
			set => _state[States.ShowModernPlusMinusGlyphs] = value;
		}

		#endregion TreemapView Appearance Properties

		#region Node Appearance Properties

		/// <summary>
		/// Gets or sets a value used as the height (in pixels) of a node branch header.
		/// If this value is 0, the header will not be drawn.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchHeight"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(16)]
		public virtual int NodeBranchHeaderHeight { get; [InvalidatesControl, ForcesReTile] set; } = 16;

		/// <summary>
		/// A <see cref="StringFormat"/> used to describe how the control will render the
		/// <see cref="TreemapNode"/>'s text if it's a branch node.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchStringFormat"/> property.
		/// </remarks>
		[Browsable(false)]
		[Category(Categories.AppearanceNode)]
		public virtual StringFormat NodeBranchHeaderStringFormat { get; [InvalidatesControl] set; } =
			new StringFormat(StringFormat.GenericTypographic)
			.SetAlignments(ContentAlignment.MiddleCenter);

		/// <summary>
		/// Gets or sets how to align the text shown on a <see cref="TreemapNode"/>
		/// when it's a branch node.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchStringFormat"/> property.
		/// </remarks>
		[Browsable(true)]
		[Category(Categories.AppearanceNode)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public virtual ContentAlignment NodeBranchHeaderTextAlignment
		{
			get => NodeBranchHeaderStringFormat.GetContentAlignment();
			[InvalidatesControl]
			set => NodeBranchHeaderStringFormat.SetAlignments(value);
		}

		/// <summary>
		/// A <see cref="StringFormat"/> used to describe how the control will render the
		/// <see cref="TreemapNode"/>'s text if it's a leaf node.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.LeafStringFormat"/> property.
		/// </remarks>
		[Browsable(false)]
		[Category(Categories.AppearanceNode)]
		public virtual StringFormat NodeLeafStringFormat { get; [InvalidatesControl] set; } =
			new StringFormat(StringFormat.GenericTypographic)
			.SetAlignments(ContentAlignment.MiddleCenter);

		/// <summary>
		/// Gets or sets how to align the text shown on a <see cref="TreemapNode"/>
		/// when it's a leaf node.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.LeafStringFormat"/> property.
		/// </remarks>
		[Browsable(true)]
		[Category(Categories.AppearanceNode)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public virtual ContentAlignment NodeLeafTextAlignment
		{
			get => NodeLeafStringFormat.GetContentAlignment();
			[InvalidatesControl]
			set => NodeLeafStringFormat.SetAlignments(value);
		}

		/// <summary>
		/// Gets or sets the style a <see cref="TreemapNode"/> is drawn if it's a leaf mode.
		/// </summary>
		/// <returns>
		/// One of the <see cref="TreemapNodeDrawStyle"/> values. The default is
		/// <see cref="TreemapNodeDrawStyle.Flat"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.LeafDrawStyle"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(TreemapNodeDrawStyle.Flat)]
		public virtual TreemapNodeDrawStyle NodeLeafDrawStyle { get; [InvalidatesControl] set; } = TreemapNodeDrawStyle.Flat;

		/// <summary>
		/// Gets or sets the font that is used to display the text on the <see cref="TreemapNode"/> label.
		/// </summary>
		/// <returns>
		/// The <see cref="Font"/> that is used to display the text on the <see cref="TreemapNode"/> label.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.NodeLeafFont"/> property.
		/// </remarks>
		[AmbientValue(null)]
		[Category(Categories.AppearanceNode)]
		[Localizable(true)]
		public virtual Font? NodeLeafFont
		{
			get => _nodeLeafFont ?? Parent.Font;
			[InvalidatesControl]
			set
			{
				_nodeLeafFont = value;
			}
		}

		/// <summary>
		/// Gets or sets the font that is used to display the text on the <see cref="TreemapNode"/> header label.
		/// </summary>
		/// <returns>
		/// The <see cref="Font"/> that is used to display the text on the <see cref="TreemapNode"/> header label.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.NodeBranchFont"/> property.
		/// </remarks>
		[AmbientValue(null)]
		[Category(Categories.AppearanceNode)]
		[Localizable(true)]
		public virtual Font? NodeBranchFont
		{
			get => _nodeBranchFont ?? Parent.Font;
			[InvalidatesControl]
			set
			{
				_nodeBranchFont = value;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color of a <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of a <see cref="TreemapNode"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.ForeColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.ControlText))]
		public virtual Color NodeLeafForeColor { get; [InvalidatesControl] set; } = SystemColors.ControlText;

		/// <summary>
		/// Gets or sets the hot tracking foreground color of a <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of a <see cref="TreemapNode"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.HotTrackForeColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.HotTrack))]
		public virtual Color NodeLeafHotTrackForeColor { get; [InvalidatesControl] set; } = SystemColors.HotTrack;

		/// <summary>
		/// Gets or sets the background color of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The background <see cref="Color"/> of a <see cref="TreemapNode"/>. The default is <see cref="SystemColors.Control"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BackColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.Control))]
		public virtual Color NodeLeafBackColor { get; [InvalidatesControl] set; } = SystemColors.Control;

		/// <summary>
		/// Gets or sets the foreground color of a <see cref="TreemapNode"/> branch (header).
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of a <see cref="TreemapNode"/> branch (header). The default is <see cref="SystemColors.ControlText"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchForeColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.ControlText))]
		public virtual Color NodeBranchForeColor { get; [InvalidatesControl] set; } = SystemColors.ControlText;

		/// <summary>
		/// Gets or sets the hot tracking foreground color of a <see cref="TreemapNode"/> branch (header).
		/// </summary>
		/// <returns>
		/// The foreground <see cref="Color"/> of a <see cref="TreemapNode"/> branch (header). The default is <see cref="SystemColors.HotTrack"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchHotTrackForeColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.HotTrack))]
		public virtual Color NodeBranchHotTrackForeColor { get; [InvalidatesControl] set; } = SystemColors.HotTrack;

		/// <summary>
		/// Gets or sets the background color of a <see cref="TreemapNode"/> branch (header).
		/// </summary>
		/// <returns>
		/// The background <see cref="Color"/> of a <see cref="TreemapNode"/>. The default is <see cref="SystemColors.ControlLight"/>.
		/// </returns>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchBackColor"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Color), nameof(SystemColors.ControlLight))]
		public virtual Color NodeBranchBackColor { get; [InvalidatesControl] set; } = SystemColors.ControlLight;

		/// <summary>
		/// Gets or sets the alignment of an image that is displayed in the control.
		/// </summary>
		/// <returns>
		/// One of the <see cref="ContentAlignment"/> values. The default is <see cref="ContentAlignment.MiddleCenter"/>
		/// </returns>
		/// <exception cref="InvalidEnumArgumentException">
		/// The value assigned is not one of the <see cref="ContentAlignment"/> values.
		/// </exception>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.ImageAlign"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(ContentAlignment.TopLeft)]
		[Localizable(true)]
		public virtual ContentAlignment NodeImageAlign { get; [InvalidatesControl] set; } = ContentAlignment.TopLeft;

		/// <summary>
		/// A <see cref="System.Windows.Forms.Padding"/> to control how deflated a leaf node is drawn.
		/// This can be modified by using the <see cref="Indent"/> property.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.LeafPadding"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Padding), "Empty")]
		public virtual Padding NodeLeafPadding { get; [InvalidatesControl] set; } = Padding.Empty;

		/// <summary>
		/// A <see cref="System.Windows.Forms.Padding"/> to control how deflated a branch node is drawn.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.BranchMargin"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(typeof(Padding), "Empty")]
		public virtual Padding NodeBranchMargin { get; [InvalidatesControl] set; } = Padding.Empty;

		/// <summary>
		/// Gets or sets a value that controls if branch <see cref="TreemapNode"/>s should be
		/// drawn as headers or not.
		/// </summary>
		/// <remarks>
		/// This value can be overridden by individual <see cref="TreemapNode"/>s using the
		/// <see cref="TreemapNode.ShowBranchHeader"/> property.
		/// </remarks>
		[Category(Categories.AppearanceNode)]
		[DefaultValue(true)]
		public virtual bool ShowBranchesAsHeaders
		{
			get => _state[States.ShowBranchesAsHeaders];
			[InvalidatesControl, ForcesReTile]
			set => _state[States.ShowBranchesAsHeaders] = value;
		}

		#endregion Node Appearance Properties

		#endregion Appearance Properties

		#region Behavior Properties

		/// <summary>
		/// The mode to use to determine the value of the <see cref="TreemapNode"/>s.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(TreemapValueMode.LeavesOnly)]
		public virtual TreemapValueMode ValueMode { get; [InvalidatesControl] set; } = TreemapValueMode.LeavesOnly;

		/// <summary>
		/// Gets the collection of <see cref="TreemapNode"/>s that are assigned to the <see cref="TreemapView"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNodeCollection"/> that represents the <see cref="TreemapNode"/>s assigned
		/// to the <see cref="TreemapView"/>.
		/// </returns>
		[Category(Categories.Behavior)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[MergableProperty(false)]
		public virtual TreemapNodeCollection Nodes => _root.Nodes;

		/// <summary>
		/// Gets or sets a value indicating whether the label text of the <see cref="TreemapNode"/>s can
		/// be edited.
		/// </summary>
		/// <returns>
		/// true if the label text of the <see cref="TreemapNode"/>s can be edited; otherwise, false. The
		/// default is false.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		[Obsolete($"{nameof(LabelEdit)} is not yet implemented.")] // #EDITING
		public virtual bool LabelEdit
		{
			get => _state[States.LabelEdit];
			[InvalidatesControl]
			set => _state[States.LabelEdit] = value;
		}

		/// <summary>
		/// Gets or sets the distance to indent each child <see cref="TreemapNode"/> level. This is treated
		/// as equidistant padding on all sides.
		/// </summary>
		/// <returns>
		/// The distance, in pixels, to indent each child <see cref="TreemapNode"/> level. The default value
		/// is 0.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(0)]
		[Localizable(true)]
		public virtual int Indent
		{
			get => NodeLeafPadding.All;
			[InvalidatesControl]
			set => NodeLeafPadding = new(value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether a <see cref="TreemapNode"/> label takes on the appearance
		/// of a hyperlink as the mouse pointer passes over it.
		/// </summary>
		/// <returns>
		/// true if a <see cref="TreemapNode"/> label takes on the appearance of a hyperlink as the mouse
		/// pointer passes over it; otherwise, false. The default is false.
		/// </returns>
		/// <remarks>
		/// If the <see cref="CheckBoxes"/> property is set to `true`, the <see cref="HotTracking"/> property has no effect.
		/// This is a limitation based on the design of <see cref="TreeView"/>. See the documentation for it
		/// here: <see href="https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.treeview.hottracking?view=net-5.0#remarks">
		/// system.windows.forms.treeview.hottracking
		/// </see>
		/// If you think this limitation should be removed or modified, please open an issue or PR on github.
		/// </remarks>
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		public virtual bool HotTracking
		{
			get => _state[States.HotTracking];
			[InvalidatesControl]
			set => _state[States.HotTracking] = value;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the selected <see cref="TreemapNode"/> remains highlighted
		/// even when the <see cref="TreemapView"/> has lost the focus.
		/// </summary>
		/// <returns>
		/// true if the selected <see cref="TreemapNode"/> is not highlighted when the <see cref="TreemapView"/> has lost
		/// the focus; otherwise, false. The default is true.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(true)]
		public virtual bool HideSelection
		{
			get => _state[States.HideSelection];
			[InvalidatesControl]
			set => _state[States.HideSelection] = value;
		}

		/// <summary>
		/// Gets or sets the mode in which the control is drawn.
		/// </summary>
		/// <returns>
		/// One of the <see cref="TreemapViewDrawMode"/> values. The default is <see cref="TreemapViewDrawMode"/>.Normal.
		/// </returns>
		/// <exception cref="InvalidEnumArgumentException">
		/// The property value is not a valid <see cref="TreemapViewDrawMode"/> value.
		/// </exception>
		[Category(Categories.Behavior)]
		[DefaultValue(TreemapViewDrawMode.Normal)]
		public virtual TreemapViewDrawMode DrawMode { get; [InvalidatesControl] set; } = TreemapViewDrawMode.Normal;

		/// <summary>
		/// Gets or sets the implementation of <see cref="System.Collections.IComparer"/> to perform a
		/// custom sort of the <see cref="TreemapView"/> nodes.
		/// </summary>
		/// <returns>
		/// The <see cref="System.Collections.IComparer"/> to perform the custom sort.
		/// </returns>
		[AmbientValue(null)]
		[Browsable(false)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category(Categories.Behavior)]
		public virtual IComparer<TreemapNode> TreemapViewNodeSorter
		{
			get => _treemapViewNodeSorter;
			[InvalidatesControl, ForcesReTile]
			set
			{
				_treemapViewNodeSorter = value ?? new TreemapNodeSorter(this);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="TreemapNode"/>s in the <see cref="TreemapView"/> are sorted.
		/// </summary>
		/// <returns>
		/// true if the nodes should be sorted; otherwise false. The default is true.
		/// </returns>
		[Browsable(false)]
		[Category(Categories.Behavior)]
		[DefaultValue(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual bool Sorted
		{
			get => _state[States.Sorted];
			[InvalidatesControl, ForcesReTile]
			set => _state[States.Sorted] = value;
		}

		/// <summary>
		/// Gets or sets the delimiter string that the <see cref="TreemapNode"/> path uses.
		/// </summary>
		/// <returns>
		/// The delimiter string that the <see cref="TreemapNode"/> <see cref="TreemapNode"/>.FullPath
		/// property uses. The default is the forward slash character (/).
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue("/")]
		public virtual string PathSeparator { get; set; } = "/";

		/// <summary>
		/// Gets or sets a value indicating whether plus-sign (+) and minus-sign (-) buttons
		/// are displayed next to <see cref="TreemapNode"/>s that contain child <see cref="TreemapNode"/>s.
		/// </summary>
		/// <returns>
		/// true if plus sign and minus sign buttons are displayed next to <see cref="TreemapNode"/>s that
		/// contain child <see cref="TreemapNode"/>s; otherwise, false. The default is false.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		public virtual bool ShowPlusMinus
		{
			get => _state[States.ShowPlusMinus];
			[InvalidatesControl]
			set => _state[States.ShowPlusMinus] = value;
		}

		/// <summary>
		/// A <see cref="System.Windows.Forms.ToolTip"/> for hovering tooltips.
		/// </summary>
		[AmbientValue(null)]
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public virtual ToolTip ToolTip
		{
			get => _toolTip ??= MakeToolTip();
			set => _toolTip = value;
		}

		/// <summary>
		/// Gets or sets a value indicating ToolTips are shown when the mouse pointer hovers
		/// over a <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// true if ToolTips are shown when the mouse pointer hovers over a <see cref="TreemapNode"/>;
		/// otherwise, false. The default is false.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		public virtual bool ShowNodeToolTips
		{
			get => _state[States.ShowNodeToolTips];
			set => _state[States.ShowNodeToolTips] = value;
		}

		/// <summary>
		/// Gets or sets a value indicating the <see cref="TreemapView"/> should have a selection that encompasses
		/// a given <see cref="TreemapNode"/> and it's descendants or just a single node when a branch is selected.
		/// This controls the visual behavior, but does not affect the actual <see cref="SelectedNode"/> value.
		/// </summary>
		/// <returns>
		/// true if a branch <see cref="TreemapNode"/> and it's descendants should be shown
		/// as selected; otherwise false. The default value is true.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(true)]
		public virtual bool FullBranchSelect
		{
			get => _state[States.FullBranchSelect];
			[InvalidatesControl]
			set => _state[States.FullBranchSelect] = value;
		}

		#endregion Behavior Properties

		#region ImageList Properties

		/// <summary>
		/// An image indexer for the default image for a <see cref="TreemapNode"/>.
		/// </summary>
		[Lazy]
		internal ImageIndexer ImageIndexer => new();

		/// <summary>
		/// An image indexer for the default image for a <see cref="TreemapNode"/>.
		/// </summary>
		[Lazy]
		internal ImageIndexer SelectedImageIndexer => new();

		/// <summary>
		/// An indexer for the image for a <see cref="TreemapNode"/> when the node is displaying it's checked state.
		/// </summary>
		[Lazy]
		internal ImageIndexer StateImageIndexer => new();

		/// <summary>
		/// Gets or sets the image list that is used to indicate the state of the <see cref="TreemapView"/>
		/// and its nodes.
		/// </summary>
		/// <returns>
		/// The <see cref="ImageList"/> used for indicating the state of the <see cref="TreemapView"/>
		/// and its nodes.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ImageList? StateImageList
		{
			get => StateImageIndexer.ImageList;
			[InvalidatesControl]
			set => StateImageIndexer.ImageList = value;
		}

		/// <summary>
		/// Gets or sets the S<see cref="ImageList"/> that contains the <see cref="Image"/>
		/// objects that are used by the <see cref="TreemapNode"/>s.
		/// </summary>
		/// <returns>
		/// The S<see cref="ImageList"/> that contains the <see cref="Image"/> objects
		/// that are used by the <see cref="TreemapNode"/>s. The default value is null.
		/// </returns>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ImageList? ImageList
		{
			get => ImageIndexer.ImageList;
			[InvalidatesControl]
			set
			{
				ImageIndexer.ImageList = value;
				SelectedImageIndexer.ImageList = value;
			}
		}

		/// <summary>
		/// Gets or sets the key of the default image for each node in the <see cref="TreemapView"/>
		/// control when it is in an unselected state.
		/// </summary>
		/// <returns>
		/// The key of the default image shown for each node <see cref="TreemapView"/>
		/// control when the node is in an unselected state.
		/// </returns>
		/// <remarks>
		/// This property mimics the <see cref="TreeView.ImageKey"/> property.
		/// </remarks>
		[Category(Categories.Behavior)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("ImageList")]
		[TypeConverter(typeof(ImageKeyConverter))]
		public string ImageKey
		{
			get => ImageIndexer.Key;
			[InvalidatesControl]
			set
			{
				if (ImageIndexer.Key == value)
				{
					return;
				}

				ImageIndexer.Key = value;

				if (string.IsNullOrEmpty(value) || value.Equals(ImageIndexer.NoneKey))
				{
					ImageIndex = (ImageList is not null)
						? 0
						: ImageIndexer.DefaultIndex;
				}
			}
		}

		/// <summary>
		/// Gets or sets the image-list index value of the default image that is displayed
		/// by the <see cref="TreemapNode"/>s.
		/// </summary>
		/// <returns>
		/// A zero-based index that represents the position of an <see cref="Image"/> in
		/// an S<see cref="ImageList"/>. The default is zero.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified index is less than 0.
		/// </exception>
		/// <remarks>
		/// This property mimics the <see cref="TreeView.ImageIndex"/> property.
		/// </remarks>
		[Category(Categories.Behavior)]
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("ImageList")]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				if (ImageList is null)
				{
					return ImageIndexer.DefaultIndex;
				}

				if (ImageIndexer.Index >= ImageList.Images.Count)
				{
					return ImageList.Images.Count > 0
						? ImageList.Images.Count
						: 0;
				}

				return ImageIndexer.Index;
			}
			[InvalidatesControl]
			set
			{
				if (value == ImageIndexer.DefaultIndex)
				{
					value = 0;
				}

				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be >= 0");
				}

				if (ImageIndexer.Index != value)
				{
					ImageIndexer.Index = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the key of the default image shown when a <see cref="TreemapNode"/>
		/// is in a selected state.
		/// </summary>
		/// <returns>
		/// The key of the default image shown when a <see cref="TreemapNode"/> is in
		/// a selected state.
		/// </returns>
		/// <remarks>
		/// This property mimics the <see cref="TreeView.SelectedImageKey"/> property.
		/// </remarks>
		[Category(Categories.Behavior)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("ImageList")]
		[TypeConverter(typeof(ImageKeyConverter))]
		public string SelectedImageKey
		{
			get => SelectedImageIndexer.Key;
			[InvalidatesControl]
			set
			{
				if (SelectedImageIndexer.Key == value)
				{
					return;
				}

				SelectedImageIndexer.Key = value;

				if (string.IsNullOrEmpty(value) || value.Equals(ImageIndexer.NoneKey))
				{
					SelectedImageIndex = (ImageList is not null)
						? 0
						: ImageIndexer.DefaultIndex;
				}
			}
		}

		/// <summary>
		/// Gets or sets the image list index value of the image that is displayed when a
		/// <see cref="TreemapNode"/> is selected.
		/// </summary>
		/// <returns>
		/// A zero-based index value that represents the position of an <see cref="Image"/>
		/// in an S<see cref="ImageList"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The index assigned value is less than zero.
		/// </exception>
		/// <remarks>
		/// This property mimics the <see cref="TreeView.SelectedImageIndex"/> property.
		/// </remarks>
		[Category(Categories.Behavior)]
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RelatedImageList("ImageList")]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		public int SelectedImageIndex
		{
			get
			{
				if (ImageList is null)
				{
					return ImageIndexer.DefaultIndex;
				}

				if (SelectedImageIndexer.Index >= ImageList.Images.Count)
				{
					return ImageList.Images.Count > 0
						? ImageList.Images.Count
						: 0;
				}

				return SelectedImageIndexer.Index;
			}
			[InvalidatesControl]
			set
			{
				if (value == ImageIndexer.DefaultIndex)
				{
					value = 0;
				}

				if (value < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be >= 0");
				}

				if (SelectedImageIndexer.Index != value)
				{
					SelectedImageIndexer.Index = value;
				}
			}
		}

		#endregion ImageList Properties

		#region Unsupported Properties

		/// <summary>
		/// <see cref="TreemapView"/>s are never scrollable. This value will always be false.
		/// </summary>
		/// <returns>
		/// false. <see cref="TreemapView"/>s are not scrollable.
		/// </returns>
		[Browsable(false)]
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"{nameof(TreemapView)} draws to fill it's control size, so it isn't scrollable.")]
		public bool Scrollable { get => false; set { } }

		/// <summary>
		/// This property exists for compatibility with <see cref="TreeView"/>. It has no parallel concept
		/// for <see cref="TreemapView"/>, and is unused.
		/// </summary>
		/// <returns>
		/// 0.
		/// </returns>
		[Browsable(false)]
		[Category(Categories.Appearance)]
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"{nameof(ItemHeight)} is not supported.")]
		public int ItemHeight { get => 0; set { } }

		/// <summary>
		/// This property exists for compatibility with <see cref="TreeView"/>. It has no parallel concept
		/// for <see cref="TreemapView"/>, and is unused.
		/// </summary>
		/// <returns>
		/// false.
		/// </returns>
		[Browsable(false)]
		[Category(Categories.Behavior)]
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"{nameof(TreemapView)} has no concept of root lines.")]
		public bool ShowRootLines { get => false; set { } }

		#region Obsolete Properties

		/// <summary>
		/// An obsolete property kept to maintain API uniformity with <see cref="TreeView"/>.
		/// It's value is forwarded to <see cref="FullBranchSelect"/>, and you should use that property instead.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Behavior)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"Use {nameof(FullBranchSelect)} instead.")]
		public bool FullRowSelect
		{
			get => _state[States.FullBranchSelect];
			[InvalidatesControl]
			set => _state[States.FullBranchSelect] = value;
		}

		/// <summary>
		/// An obsolete property kept to maintain API uniformity with <see cref="TreeView"/>.
		/// It's value is forwarded to <see cref="ShowGrid"/>, and you should use that property instead.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Behavior)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"Use {nameof(ShowGrid)} instead.")]
		public virtual bool ShowLines
		{
			get => _state[States.ShowGrid];
			[InvalidatesControl]
			set => _state[States.ShowGrid] = value;
		}

		/// <summary>
		/// An obsolete property kept to maintain API uniformity with <see cref="TreeView"/>.
		/// It's value is forwarded to <see cref="GridColor"/>, and you should use that property instead.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Appearance)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"Use {nameof(GridColor)} instead.")]
		public virtual Color LineColor
		{
			get => GridColor;
			[InvalidatesControl]
			set => GridColor = value;
		}

		#endregion Obsolete Properties

		#endregion Unsupported Properties

		#region Inherited Properties

		/// <inheritdoc/>
		[Category(Categories.Appearance)]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		public override string Text
		{
			get => base.Text;
			[InvalidatesControl]
			set => base.Text = value;
		}


		/// <summary>
		/// Gets or sets a value indicating whether the control should redraw its surface
		/// using a secondary buffer. The <see cref="TreemapView"/>.DoubleBuffered property
		/// does not affect the <see cref="TreemapView"/> control.
		/// </summary>
		/// <returns>
		/// true if the control uses a secondary buffer; otherwise, false.
		/// </returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get => true;
			set { }
		}

		#endregion Inherited Properties

		#endregion Properties

		#region Events

		#region Editing

		/// <summary>
		/// Occurs before the <see cref="TreemapNode"/> label text is edited.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		[Obsolete($"{nameof(BeforeLabelEdit)} is not yet implemented.")] // #EDITING
		public event TreemapNodeLabelEditEventHandler? BeforeLabelEdit;

		/// <summary>
		/// Occurs after the <see cref="TreemapNode"/> label text is edited.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		[Obsolete($"{nameof(AfterLabelEdit)} is not yet implemented.")] // #EDITING
		public event TreemapNodeLabelEditEventHandler? AfterLabelEdit;

		#endregion Editing

		#region State

		/// <summary>
		/// Occurs before the <see cref="TreemapNode"/> check box is checked.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewCancelEventHandler? BeforeCheck;

		/// <summary>
		/// Occurs after the <see cref="TreemapNode"/> check box is checked.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewEventHandler? AfterCheck;

		#endregion State

		#region Collapsing

		/// <summary>
		/// Occurs before a <see cref="TreemapNode"/> is collapsed.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewCancelEventHandler? BeforeCollapse;

		/// <summary>
		/// Occurs after a <see cref="TreemapNode"/> is collapsed.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewEventHandler? AfterCollapse;

		/// <summary>
		/// Occurs before a <see cref="TreemapNode"/> is expanded.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewCancelEventHandler? BeforeExpand;

		/// <summary>
		/// Occurs after a <see cref="TreemapNode"/> is expanded.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapViewEventHandler? AfterExpand;

		#endregion Collapsing

		#region Rendering

		/// <summary>
		/// Occurs when a <see cref="TreemapNode"/> is drawn and the <see cref="TreemapView"/>.DrawMode
		/// property is set to a <see cref="TreemapViewDrawMode"/> value other than <see cref="TreemapViewDrawMode"/>.Normal.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event DrawTreemapNodeEventHandler? DrawNode;

		/// <summary>
		/// Occurs when a <see cref="TreemapNode"/> is drawn and the <see cref="TreemapView"/>.DrawMode
		/// property is set to a <see cref="TreemapViewDrawMode"/> value other than <see cref="TreemapViewDrawMode"/>.Normal.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event DrawTreemapNodeEventHandler? DrawBranchNode;

		/// <summary>
		/// Occurs when a <see cref="TreemapNode"/> is drawn and the <see cref="TreemapView"/>.DrawMode
		/// property is set to a <see cref="TreemapViewDrawMode"/>.OwnerDrawAll.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event DrawTreemapNodeEventHandler? DrawNodeGrid;

		/// <summary>
		/// Occurs when a <see cref="TreemapNode"/> is drawn and the <see cref="TreemapView"/>.DrawMode
		/// property is set to a <see cref="TreemapViewDrawMode"/>.OwnerDrawAll. Is invoked when
		/// the node will draw it's State (checkbox) glyph.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event DrawTreemapNodeEventHandler? DrawNodeState;

		/// <summary>
		/// Occurs when a <see cref="TreemapNode"/> is drawn and the <see cref="TreemapView"/>.DrawMode
		/// property is set to a <see cref="TreemapViewDrawMode"/>.OwnerDrawAll. Is invoked when
		/// the node will draw it's Plus/Minus (Expand/Collapse) glyph.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event DrawTreemapNodeEventHandler? DrawNodePlusMinus;

		#endregion Rendering

		#region Selection

		/// <summary>
		/// Occurs before the <see cref="TreemapNode"/> is selected.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		public event TreemapViewCancelEventHandler? BeforeSelect;

		/// <summary>
		/// Occurs after the <see cref="TreemapNode"/> is selected.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		public event TreemapViewEventHandler? AfterSelect;

		#endregion Selection

		#region Input

		#region Keyboard

		#endregion Keyboard

		#region Mouse

		/// <summary>
		/// Occurs when the mouse hovers over a <see cref="TreemapNode"/>.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		public event TreemapNodeMouseHoverEventHandler? NodeMouseHover;

		/// <summary>
		/// Occurs when the user begins dragging a node.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		[Obsolete($"{nameof(ItemDrag)} is not yet implemented.")] // #DRAGDROP
		public event ItemDragEventHandler? ItemDrag;

		/// <summary>
		/// Occurs when the user clicks a <see cref="TreemapNode"/> with the mouse.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		public event TreemapNodeMouseClickEventHandler? NodeMouseClick;

		/// <summary>
		/// Occurs when the user double-clicks a <see cref="TreemapNode"/> with the mouse.
		/// </summary>
		[Category(Categories.Action)]
		[DefaultValue(null)]
		public event TreemapNodeMouseClickEventHandler? NodeMouseDoubleClick;

		#endregion Mouse

		#endregion Input

		#endregion Events

		#region Event Raising Methods

		#region Rendering

		/// <inheritdoc/>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (IsUpdating)
			{
				// don't try to paint while updating.
				return;
			}

			e.Graphics.SmoothingMode = _smoothingMode;
			e.Graphics.PixelOffsetMode = _pixelOffsetMode;
			e.Graphics.TextRenderingHint = _textRenderingHint;
			e.Graphics.InterpolationMode = _interpolationMode;

			// let the base paint our regular back color.
			// this should also handle backgroundimage and the sort.
			base.OnPaintBackground(e);

			if (HasData)
			{
				_topNode.DrawBackground(e.Graphics, _nodePaintCallbacks);
			}
			else
			{
				PaintBackgroundNoData(e.Graphics);
			}
		}

		/// <inheritdoc/>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (IsUpdating)
			{
				// don't try to paint while updating.
				return;
			}

			base.OnPaint(e);

			e.Graphics.SmoothingMode = _smoothingMode;
			e.Graphics.PixelOffsetMode = _pixelOffsetMode;
			e.Graphics.TextRenderingHint = _textRenderingHint;
			e.Graphics.InterpolationMode = _interpolationMode;

			if (HasData)
			{
				_topNode.DrawForeground(e.Graphics, _nodePaintCallbacks);
			}
			else
			{
				PaintNoData(e.Graphics);
			}
		}

		/// <inheritdoc/>
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);

			if (IsUpdating)
			{
				return;
			}

			CheckForReTile();
		}

		#endregion Rendering

		#region State

		/// <summary>
		/// Raises the <see cref="BeforeCheck"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewCancelEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnBeforeCheck(TreemapViewCancelEventArgs e)
			=> BeforeCheck?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="AfterCheck"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnAfterCheck(TreemapViewEventArgs e)
			=> AfterCheck?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="Control.GotFocus"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="EventArgs"/> that contains the event data.
		/// </param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (HideSelection && SelectedNode is not null)
			{
				Invalidate(SelectedNode);
			}
		}

		/// <summary>
		/// Raises the <see cref="Control.LostFocus"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="EventArgs"/> that contains the event data.
		/// </param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (HideSelection && SelectedNode is not null)
			{
				Invalidate(SelectedNode);
			}
		}

		#endregion State

		#region Selection

		/// <summary>
		/// Raises the <see cref="BeforeSelect"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewCancelEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnBeforeSelect(TreemapViewCancelEventArgs e)
			=> BeforeSelect?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="AfterSelect"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnAfterSelect(TreemapViewEventArgs e)
			=> AfterSelect?.Invoke(this, e);

		#endregion Selection

		#region Editing

		/// <summary>
		/// Raises the <see cref="BeforeLabelEdit"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapNodeLabelEditEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnBeforeLabelEdit(TreemapNodeLabelEditEventArgs e)
			=> BeforeLabelEdit?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="AfterLabelEdit"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapNodeLabelEditEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnAfterLabelEdit(TreemapNodeLabelEditEventArgs e)
			=> AfterLabelEdit?.Invoke(this, e);

		#endregion Editing

		#region Rendering

		/// <summary>
		/// Raises the <see cref="DrawNode"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnDrawNode(DrawTreemapNodeEventArgs e)
			=> DrawNode?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="DrawBranchNode"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnDrawBranchNode(DrawTreemapNodeEventArgs e)
			=> DrawBranchNode?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="DrawNodeGrid"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnDrawNodeGrid(DrawTreemapNodeEventArgs e)
			=> DrawNodeGrid?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="DrawNodeState"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnDrawNodeState(DrawTreemapNodeEventArgs e)
			=> DrawNodeState?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="DrawNodePlusMinus"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnDrawNodePlusMinus(DrawTreemapNodeEventArgs e)
			=> DrawNodePlusMinus?.Invoke(this, e);

		#endregion Rendering

		#region Input

		#region Keyboard

		/// <summary>
		/// Raises the <see cref="Control.KeyDown"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="KeyEventArgs"/> that contains the event data.
		/// </param>
		/// <remarks>This handler mimic's <see cref="TreeView.OnKeyDown(KeyEventArgs)"/>.</remarks>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Handled)
			{
				return;
			}

			// space is what toggles our checkbox on keydown.
			// this is why we ignore it in keypress/keyup.
			if (CheckBoxes && (e.KeyData & Keys.KeyCode).HasFlag(Keys.Space))
			{
				if (SelectedNode is not null)
				{
					ToggleNodeChecked(SelectedNode, TreemapViewAction.ByKeyboard);
					e.Handled = true;
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="Control.KeyPress"/> event.
		/// </summary>
		/// <param name="e">
		/// <see cref="KeyPressEventArgs"/> that contains the event data.
		/// </param>
		/// <remarks>This handler mimic's <see cref="TreeView.OnKeyPress(KeyPressEventArgs)"/>.</remarks>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			if (e.Handled)
			{
				return;
			}

			// ignore space
			if (e.KeyChar == ' ')
			{
				e.Handled = true;
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnKeyUp(KeyEventArgs)"/>
		/// </summary>
		/// <param name="e">
		/// A <see cref="KeyEventArgs"/> that contains the event data.
		/// </param>
		/// <remarks>This handler mimic's <see cref="TreeView.OnKeyUp(KeyEventArgs)"/>.</remarks>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.Handled)
			{
				return;
			}

			// ignore space
			if ((e.KeyData & Keys.KeyCode).HasFlag(Keys.Space))
			{
				e.Handled = true;
			}
		}

		/// <summary>
		/// Determines whether the specified key is a regular input key or a special key
		/// that requires preprocessing.
		/// </summary>
		/// <param name="keyData">
		/// One of the Keys values.
		/// </param>
		/// <returns>
		/// true if the specified key is a regular input key; otherwise, false.
		/// </returns>
		protected override bool IsInputKey(Keys keyData)
		{
			// if we are editing a node, we
			// need to handle that case specifically
			// this block based on System.Windows.Forms.TreeView.cs
			// and is MIT licensed.
			if (_stateEditingNode is not null && (keyData & Keys.Alt) == 0)
			{
				switch (keyData & Keys.KeyCode)
				{
					case Keys.Return:
					case Keys.Escape:
					case Keys.PageUp:
					case Keys.PageDown:
					case Keys.Home:
					case Keys.End:
						return true;
					default:
						break;
				}
			}

			return base.IsInputKey(keyData);
		}

		#endregion Keyboard

		#region Mouse

		/// <summary>
		/// Raises the <see cref="ItemDrag"/> event.
		/// </summary>
		/// <param name="e">
		/// An <see cref="ItemDragEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnItemDrag(ItemDragEventArgs e)
			=> ItemDrag?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (_stateMouseDownNode is not null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						SetSelectedNode(_stateMouseDownNode);
						break;
					case MouseButtons.Right:
						Point screenLocation = PointToScreen(e.Location);
						ShowContextMenuStrip(_stateMouseDownNode, screenLocation);
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="Control.MouseLeave"/> event.
		/// As well, resets the already hovered state.
		/// </summary>
		/// <param name="e">
		/// An <see cref="EventArgs"/> that contains the event data.
		/// </param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_state[States.HoveredAlready] = false;
		}

		/// <summary>
		/// Raises the <see cref="Control.MouseHover"/> event.
		/// </summary>
		/// <param name="e">
		/// An <see cref="EventArgs"/> that contains the event data.
		/// </param>
		/// <remarks>
		/// This function based on <see cref="TreeView"/>'s source.
		/// Licensed under the MIT license.
		/// Copyright (c) 2021 Chris Marc Dailey (nitz)
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/src/System/Windows/Forms/TreeView.cs">TreeView.cs</see>
		/// </remarks>
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);

			Point clientPoint = PointToClient(Cursor.Position);

			TreemapViewHitTestInfo info = HitTest(clientPoint);

			if (info.Node is not null && (info.Location & TreemapViewHitTestLocations.OnItem) != 0)
			{
				if (info.Node != _stateHoveringNode)
				{
					OnNodeMouseHover(new TreemapNodeMouseHoverEventArgs(info.Node));
					_stateHoveringNode = info.Node;
					_stateHoveringNode.OnMouseHover(info);
					ShowToolTip(info.Node);
				}
			}

			if (!_state[States.HoveredAlready])
			{
				base.OnMouseHover(e);
				_state[States.HoveredAlready] = true;
			}

			ResetMouseEventArgs();
		}

		/// <summary>
		/// Raises the <see cref="Control.MouseMove"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> that contains the event data.
		/// </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			TreemapViewHitTestInfo? hit = HitTest(e.Location);
			if (hit.Node is not null)
			{
				if (_stateMouseOverNode != hit.Node)
				{
					_stateMouseOverNode?.OnMouseLeave();
					_stateMouseOverNode = hit.Node;
					HideToolTip();
				}

				_stateMouseOverNode.OnMouseOver(hit);
			}
		}

		/// <summary>
		/// Raises the <see cref="NodeMouseClick"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapNodeMouseClickEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnNodeMouseClick(TreemapNodeMouseClickEventArgs e)
			=> NodeMouseClick?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="NodeMouseDoubleClick"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapNodeMouseClickEventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnNodeMouseDoubleClick(TreemapNodeMouseClickEventArgs e)
			=> NodeMouseDoubleClick?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="NodeMouseHover"/> event.
		/// </summary>
		/// <param name="e">
		/// The <see cref="TreemapNodeMouseHoverEventArgs"/> that contains the event
		/// data.
		/// </param>
		protected virtual void OnNodeMouseHover(TreemapNodeMouseHoverEventArgs e)
			=> NodeMouseHover?.Invoke(this, e);

		#endregion Mouse

		#endregion Input

		#region Collapsing

		/// <summary>
		/// Raises the <see cref="BeforeCollapse"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewCancelEventArgs"/> that contains the event data.
		/// </param>
		protected internal virtual void OnBeforeCollapse(TreemapViewCancelEventArgs e)
			=> BeforeCollapse?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="AfterCollapse"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewEventArgs"/> that contains the event data.
		/// </param>
		protected internal virtual void OnAfterCollapse(TreemapViewEventArgs e)
		{
			AfterCollapse?.Invoke(this, e);
			ShouldReTile();
		}

		/// <summary>
		/// Raises the <see cref="BeforeExpand"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewCancelEventArgs"/> that contains the event data.
		/// </param>
		protected internal virtual void OnBeforeExpand(TreemapViewCancelEventArgs e)
			=> BeforeExpand?.Invoke(this, e);

		/// <summary>
		/// Raises the <see cref="AfterExpand"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="TreemapViewEventArgs"/> that contains the event data.
		/// </param>
		protected internal virtual void OnAfterExpand(TreemapViewEventArgs e)
		{
			AfterExpand?.Invoke(this, e);
			ShouldReTile();
		}

		#endregion Collapsing

		#region Layout

		/// <summary>
		/// Handles the resize event, re-tiling if needed.
		/// </summary>
		/// <param name="e">The resize evente args.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ShouldReTile();
		}

		#endregion Layout

		#endregion Event Raising Methods

		#region Public Methods

		/// <summary>
		/// Disables any redrawing of the <see cref="TreemapView"/>.
		/// </summary>
		public void BeginUpdate()
		{
			if (_updatesActive == 0)
			{
				SuspendLayout();
				this.SuspendPainting();
			}

			_updatesActive++;
		}

		/// <summary>
		/// Collapses all the <see cref="TreemapNode"/>s.
		/// </summary>
		public void CollapseAll() => _topNode.Collapse(true);

		/// <summary>
		/// Enables the redrawing of the <see cref="TreemapView"/>.
		/// </summary>
		public void EndUpdate()
		{
			if (_updatesActive > 0)
			{
				_updatesActive--;

				if (_updatesActive == 0)
				{
					ResumeLayout();
					this.ResumePainting();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Expands all the <see cref="TreemapNode"/>s.
		/// </summary>
		public void ExpandAll() => _topNode.ExpandAll();

		/// <summary>
		/// Retrieves the <see cref="TreemapNode"/> that is at the specified point.
		/// </summary>
		/// <param name="pt">
		/// The <see cref="Point"/> to evaluate and retrieve the node from.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> at the specified point, in <see cref="TreemapView"/> (client)
		/// coordinates, or null if there is no node at that location.
		/// </returns>
		public TreemapNode? GetNodeAt(Point pt)
			=> GetNodeAt(pt.X, pt.Y);

		/// <summary>
		/// Retrieves the <see cref="TreemapNode"/> at the point with the specified coordinates.
		/// </summary>
		/// <param name="x">
		/// The <see cref="Point.X"/> position to evaluate and retrieve the node from.
		/// </param>
		/// <param name="y">
		/// The <see cref="Point.Y"/> position to evaluate and retrieve the node from.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> at the specified location, in <see cref="TreemapView"/> (client)
		/// coordinates, or null if there is no node at that location.
		/// </returns>
		public TreemapNode? GetNodeAt(int x, int y) => _topNode.ChildNodeAt(x, y);

		/// <summary>
		/// Retrieves the number of <see cref="TreemapNode"/>s, optionally including those in all subtrees,
		/// assigned to the <see cref="TreemapView"/>.
		/// </summary>
		/// <param name="includeSubTrees">
		/// true to count the <see cref="TreemapNode"/> items that the subtrees contain;
		/// otherwise, false.
		/// </param>
		/// <returns>
		/// The number of <see cref="TreemapNode"/>s, optionally including those in all subtrees, assigned
		/// to the <see cref="TreemapView"/>.
		/// </returns>
		public int GetNodeCount(bool includeSubTrees) => _root.GetNodeCount(includeSubTrees);

		/// <summary>
		/// Provides node information, given x- and y-coordinates.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate at which to retrieve node information
		/// </param>
		/// <param name="y">
		/// The y-coordinate at which to retrieve node information.
		/// </param>
		/// <returns>
		/// The node information.
		/// </returns>
		public TreemapViewHitTestInfo HitTest(int x, int y)
			=> HitTest(new Point(x, y));

		/// <summary>
		/// Provides node information, given a point.
		/// </summary>
		/// <param name="clientPoint">
		/// The <see cref="Point"/> at which to retrieve node information.
		/// </param>
		/// <returns>
		/// The node information.
		/// </returns>
		///
		public TreemapViewHitTestInfo HitTest(Point clientPoint)
		{
			TreemapNode? node = _topNode.ChildNodeAt(clientPoint);

			if (node is not null)
			{
				TreemapViewHitTestLocations location = node.GetHitTestLocation(clientPoint);

				return new(
					node,
					location,
					clientPoint
				);
			}

			return TreemapViewHitTestInfo.Miss;
		}
		/// <summary>
		/// Sorts the items in <see cref="TreemapView"/> control.
		/// </summary>
		public void Sort()
		{
			if (TreemapViewNodeSorter == null)
			{
				throw new InvalidOperationException($"There must be a {nameof(TreemapViewNodeSorter)} assigned in order to sort.");
			}

			Nodes.Sort(TreemapViewNodeSorter, sortAllChildren: true);
		}

		#endregion Public Methods

		#region Private / Internal Methods

		/// <summary>
		/// Constructs a new <see cref="System.Windows.Forms.ToolTip"/>.
		/// </summary>
		/// <returns>The new <see cref="System.Windows.Forms.ToolTip"/>.</returns>
		private static ToolTip MakeToolTip()
		{
			ToolTip tt = new();
			tt.ReshowDelay = tt.InitialDelay;
			return tt;
		}

		/// <summary>
		/// Shows the <see cref="ToolTip"/> with the associated <see cref="TreemapNode.ToolTipText"/>
		/// if <see cref="ShowNodeToolTips"/> is enabled.
		/// </summary>
		/// <param name="node">the node to get the text from.</param>
		private void ShowToolTip(TreemapNode? node)
		{
			if (ShowNodeToolTips && node is not null && !string.IsNullOrEmpty(node.ToolTipText))
			{
				ToolTip.SetToolTip(this, node.ToolTipText);
			}
		}

		/// <summary>
		/// Hides the <see cref="ToolTip"/>.
		/// </summary>
		private void HideToolTip()
		{
			if (ToolTip.GetToolTip(this).Length > 0)
			{
				ToolTip.SetToolTip(this, string.Empty);
			}
		}

		/// <summary>
		/// Shows the <see cref="TreemapNode"/>'s <see cref="TreemapNode.ContextMenuStrip"/>
		/// at the location given. If it doesn't have one, tries to show <see cref="TreemapView"/>'s
		/// <see cref="Control.ContextMenuStrip"/> instead.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to try to show the context menu strip of.</param>
		/// <param name="location">The location to show the context menu strip at.</param>
		private void ShowContextMenuStrip(TreemapNode node, Point location)
		{
			if (!node.ShowContextMenuStrip(location))
			{
				ContextMenuStrip?.Show(location);
			}
		}

		/// <summary>
		/// Sets the internal <see cref="SelectedNode"/>, if the event doen't cancel it.
		/// </summary>
		/// <param name="nodeToSelect">The <see cref="TreemapNode"/> to set as selected, or null to unselect any node.</param>
		private void SetSelectedNode(TreemapNode? nodeToSelect)
		{
			if (_state[States.IgnoreSelects])
			{
				return;
			}

			// don't allow selects while we are selecting.
			_state[States.IgnoreSelects] = true;

			var args = new TreemapViewCancelEventArgs(nodeToSelect);

			OnBeforeSelect(args);

			if (args.Cancel)
			{
				_state[States.IgnoreSelects] = false;
				return;
			}

			if (SelectedNode is not null)
			{
				SelectedNode.IsSelected = false;
				Invalidate(SelectedNode.Bounds);
			}

			// update actual selected node backing field.
			_stateSelectedNode = nodeToSelect;

			if (SelectedNode is not null)
			{
				SelectedNode.IsSelected = true;
				Invalidate(SelectedNode.Bounds);
			}

			OnAfterSelect(new TreemapViewEventArgs(SelectedNode));

			_state[States.IgnoreSelects] = false;
		}

		/// <summary>
		/// Raises the <see cref="OnBeforeCheck(TreemapViewCancelEventArgs)"/> event,
		/// and returns the canceled state as a bool.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> that's check state is changing.</param>
		/// <param name="action">The action that triggered the check change.</param>
		/// <returns>true, if the check state change should be canceled.</returns>
		internal bool DoBeforeCheck(TreemapNode? node, TreemapViewAction action)
		{
			TreemapViewCancelEventArgs args = new(node, action);
			OnBeforeCheck(args);
			return args.Cancel;
		}

		/// <summary>
		/// Raises the <see cref="OnAfterCheck(TreemapViewEventArgs)"/> event.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> that's check state is changing.</param>
		/// <param name="action">The action that triggered the check change.</param>
		internal void DoAfterCheck(TreemapNode node, TreemapViewAction action)
		{
			TreemapViewEventArgs args = new(node, action);
			OnAfterCheck(args);
		}

		/// <summary>
		/// Shorthand for <see cref="Control.Invalidate(Rectangle)"/> using
		/// the given <see cref="TreemapNode.Bounds"/>.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> region to invalidate.</param>
		internal void Invalidate(TreemapNode node)
			=> Invalidate(node.Bounds);


		#region Designer Ambient Magic Methods

		/// <summary>
		/// Magic method to tell the designer when not to serialize
		/// <see cref="NodeLeafFont" />.
		/// </summary>
		/// <returns></returns>
		private bool ShouldSerializeNodeLeafFont()
			=> (Parent == null || !Parent.Font.Equals(NodeLeafFont))
				&& NodeLeafFont != null;

		/// <summary>
		/// Magic method to reset <see cref="NodeLeafFont"/>
		/// </summary>
		private void ResetNodeLeafFont()
			=> NodeLeafFont = Parent != null
			? Parent.Font
			: DefaultFont;

		/// <summary>
		/// Magic method to tell the designer when not to serialize
		/// <see cref="NodeBranchFont" />.
		/// </summary>
		/// <returns></returns>
		private bool ShouldSerializeNodeBranchFont()
			=> (Parent == null || !Parent.Font.Equals(NodeBranchFont))
				&& NodeBranchFont != null;

		/// <summary>
		/// Magic method to reset <see cref="NodeBranchFont"/>
		/// </summary>
		private void ResetNodeBranchFont()
			=> NodeBranchFont = Parent != null
			? Parent.Font
			: DefaultFont;

		#endregion Designer Ambient Magic Methods

		#endregion Private / Internal Methods

		#region Layout Methods

		/// <summary>
		/// Checks if a re-tile is pending, recalculates and invalidates drawing if so.
		/// </summary>
		private void CheckForReTile()
		{
			if (_state[States.ShouldReTile])
			{
				CalculateTiling();
				Invalidate();
			}

			_state[States.ShouldReTile] = false;
		}

		/// <summary>
		/// Recalculates the tiling bounds for the view, resorting if needed.
		/// </summary>
		private void CalculateTiling()
		{
			if (Sorted)
			{
				Sort();
			}

			_topNode.RecalculateBounds(this, RectangleF.FromLTRB(0, 0, Width - 1, Height - 1));
		}

		/// <summary>
		/// Tells the <see cref="TreemapView"/> that it needs to re-tile.
		/// It also requests a layout via <see cref="Control.PerformLayout()"/>,
		/// which is when the re-tiling will happen. If this behavior is undesirable
		/// (e.g.: you have several operations to perform that would cause re-tiling,)
		/// then use <see cref="Control.SuspendLayout()"/> and <see cref="Control.ResumeLayout()"/>
		/// to suppress the intermediate layout request.
		/// </summary>
		internal void ShouldReTile()
		{
			_state[States.ShouldReTile] = true;
			PerformLayout();
		}

		#endregion Layout Methods

		#region Painting Methods

		/// <summary>
		/// The default handler to draw the background when there's no data in the <see cref="TreemapView"/>.
		/// </summary>
		/// <param name="graphics">The graphics context to draw to.</param>
		private void PaintBackgroundNoData(Graphics graphics)
		{
			using SolidBrush brush = new(BackColor);
			graphics.FillRectangle(brush, ClientRectangle);
		}

		/// <summary>
		/// The default handler to draw the foreground when there's no data in the <see cref="TreemapView"/>.
		/// </summary>
		/// <param name="graphics">The graphics context to draw to.</param>
		private void PaintNoData(Graphics graphics)
		{
			using SolidBrush brush = new(ForeColor);

			string outputText;
			if (string.IsNullOrWhiteSpace(Text))
			{
				outputText = $"{Name} has no data or nodes.";
			}
			else
			{
				outputText = Text;
			}

			graphics.DrawString(outputText, Font, brush, ClientRectangle, NoDataTextFormat);
		}

		#endregion Painting Methods

		#region Event Handlers

		/// <summary>
		/// The <see cref="TreemapView"/>'s WndProc handler.
		/// </summary>
		/// <param name="m">The <see cref="Message"/> to process.</param>
		/// <remarks>
		/// This function based on <see cref="TreeView"/>'s source.
		/// Licensed under the MIT license.
		/// Copyright (c) 2021 Chris Marc Dailey (nitz)
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/src/System/Windows/Forms/TreeView.cs">TreeView.cs</see>
		/// </remarks>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case (int)WindowsMessages.LBUTTONDBLCLK:
					WmMouseDown(ref m, MouseButtons.Left, 2);
					_state[States.DoubleClickFired] = true;
					_state[States.MouseUpFired] = false;
					Capture = true;
					break;
				case (int)WindowsMessages.LBUTTONDOWN:
				{
					try
					{
						_state[States.IgnoreSelects] = true;
						Focus();
					}
					finally
					{
						_state[States.IgnoreSelects] = false;
					}

					_state[States.MouseUpFired] = false;

					(int x, int y) = m.LParamToXY();
					TreemapViewHitTestInfo info = HitTest(x, y);

					_stateMouseDownNode = info.Node;

					// handle checkboxes
					if (info.Location.HasFlag(TreemapViewHitTestLocations.StateImage))
					{
						// raise mouse down, but ignore selection.
						_state[States.IgnoreSelects] = true;
						OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, x, y, 0));
						_state[States.IgnoreSelects] = false;

						if (CheckBoxes)
						{
							ToggleNodeChecked(_stateMouseDownNode, TreemapViewAction.ByMouse);
						}

						m.Result = IntPtr.Zero;
					}
					else if (info.Location.HasFlag(TreemapViewHitTestLocations.PlusMinus))
					{
						if (ShowPlusMinus)
						{
							_stateMouseDownNode?.Toggle();
						}
						else
						{
							throw new InvalidOperationException($"{nameof(HitTest)} returned {nameof(TreemapViewHitTestLocations.PlusMinus)} but they aren't enabled.");
						}
					}
					else
					{
						WmMouseDown(ref m, MouseButtons.Left, 1);
					}

					_stateDownButton = MouseButtons.Left;
					break;
				}
				case (int)WindowsMessages.LBUTTONUP:
				case (int)WindowsMessages.RBUTTONUP:
				{
					(int x, int y) = m.LParamToXY();
					TreemapViewHitTestInfo info = HitTest(x, y);
					if (info.Node is not null)
					{
						if (!_state[States.DoubleClickFired] && !_state[States.MouseUpFired])
						{
							if (info.Node == _stateMouseDownNode)
							{
								OnNodeMouseClick(new TreemapNodeMouseClickEventArgs(info.Node, _stateDownButton, 1, x, y));
							}

							OnClick(new MouseEventArgs(_stateDownButton, 1, x, y, 0));
							OnMouseClick(new MouseEventArgs(_stateDownButton, 1, x, y, 0));
						}

						if (_state[States.DoubleClickFired])
						{
							_state[States.DoubleClickFired] = false;
							OnNodeMouseDoubleClick(new TreemapNodeMouseClickEventArgs(info.Node, _stateDownButton, 2, x, y));
							OnDoubleClick(new MouseEventArgs(_stateDownButton, 2, x, y, 0));
							OnMouseDoubleClick(new MouseEventArgs(_stateDownButton, 2, x, y, 0));
						}
					}

					if (!_state[States.MouseUpFired])
					{
						OnMouseUp(new MouseEventArgs(_stateDownButton, 1, x, y, 0));
					}

					_state[States.DoubleClickFired] = false;
					_state[States.MouseUpFired] = false;
					Capture = false;
					_stateMouseDownNode = null;

					break;
				}
				case (int)WindowsMessages.MBUTTONDBLCLK:
					_state[States.MouseUpFired] = false;
					WmMouseDown(ref m, MouseButtons.Middle, 2);
					break;
				case (int)WindowsMessages.MBUTTONDOWN:
					_state[States.MouseUpFired] = false;
					WmMouseDown(ref m, MouseButtons.Middle, 1);
					_stateDownButton = MouseButtons.Middle;
					break;
				case (int)WindowsMessages.MOUSELEAVE:
					_stateHoveringNode = null;
					base.WndProc(ref m);
					break;
				case (int)WindowsMessages.RBUTTONDBLCLK:
					WmMouseDown(ref m, MouseButtons.Right, 2);
					_state[States.DoubleClickFired] = true;
					_state[States.MouseUpFired] = false;
					Capture = true;
					break;
				case (int)WindowsMessages.RBUTTONDOWN:
					_state[States.MouseUpFired] = false;
					_stateMouseDownNode = HitTest(m.LParamToPoint())?.Node;
					WmMouseDown(ref m, MouseButtons.Right, 1);
					_stateDownButton = MouseButtons.Right;
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

		private void ToggleNodeChecked(TreemapNode? nodeToToggle, TreemapViewAction source)
		{
			bool skipCheck = DoBeforeCheck(nodeToToggle, source);
			if (!skipCheck && nodeToToggle is not null)
			{
				nodeToToggle.CheckStateInternal = nodeToToggle.Checked
					? CheckState.Unchecked
					: CheckState.Checked;
				DoAfterCheck(nodeToToggle, source);
				this.Invalidate(nodeToToggle.CheckboxArea);
			}
		}

		/// <summary>
		/// Handles a WM_(L|R|M)MOSUEDOWN message.
		/// </summary>
		/// <param name="m">The <see cref="Message"/> to handle.</param>
		/// <param name="buttons">The pressed <see cref="MouseButtons"/>.</param>
		/// <param name="clicks">The number of clicks.</param>
		/// <remarks>
		/// This function based on <see cref="TreeView"/>'s source.
		/// Licensed under the MIT license.
		/// Copyright (c) 2021 Chris Marc Dailey (nitz)
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/main/src/System.Windows.Forms/src/System/Windows/Forms/TreeView.cs">TreeView.cs</see>
		/// </remarks>
		private void WmMouseDown(ref Message m, MouseButtons buttons, int clicks)
		{
			(int x, int y) = m.LParamToXY();
			OnMouseDown(new MouseEventArgs(buttons, clicks, x, y, 0));
		}

		/// <summary>
		/// Handles node events that should trigger a re-tile of the <see cref="TreemapView"/>.
		/// This can cause many re-tiles if called repeatedly. Consider using <see cref="Control.SuspendLayout()"/>
		/// and <see cref="Control.ResumeLayout()"/> to differ layout work if multiple changes are to be made.
		/// </summary>
		/// <param name="sender">The originator of this event.</param>
		/// <param name="e">The event arguments.</param>
		private void NodeEventRequiresRetile(object sender, TreemapNodeEventArgs e) => ShouldReTile();

		#endregion Event Handlers

		/// <summary>
		/// Gets a value from the given node, determined by the current <see cref="TreemapValueMode"/>
		/// set in <see cref="TreemapView.ValueMode"/>.
		/// </summary>
		/// <param name="node">The node to get the value for.</param>
		/// <returns>The calculated value, or 0 if the node is null.</returns>
		internal float GetNodeValueForCurrentMode(TreemapNode? node)
		{
			if (node is null)
			{
				return 0;
			}

			return ValueMode switch
			{
				TreemapValueMode.LeavesOnly => node.LeavesValue,
				TreemapValueMode.LeavesAndBranches => node.TotalValue,
				_ => node.GetValue(ValueMode),
			};
		}
	}
}
