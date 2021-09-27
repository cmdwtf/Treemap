using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

using Lazy;

using static cmdwtf.Toolkit.WinForms.Forms;

using TreemapNodeValueDataType = System.Single;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Represents a node of a <see cref="TreemapView"/>.
	/// </summary>
	[DefaultProperty(nameof(Text))]
	[TypeConverter(typeof(TreemapNodeConverter))]
	[Serializable]
	public partial class TreemapNode : MarshalByRefObject, ICloneable
#if TREEMAP_NODE_ISERIALIZABLE
		, ISerializable
#endif // TREEMAP_NODE_ISERIALIZABLE
	{
		/// <summary>
		/// The amount of in-treemap area this <see cref="TreemapNode"/> consumes.
		/// </summary>
		public float Area { get; private set; }

		/// <summary>
		/// A default value to be used if this node is a leaf, and doesn't have a value otherwise set.
		/// </summary>
		private const TreemapNodeValueDataType _defaultLeafValue = 1.0f;

		/// <summary>
		/// A default path separator to use if there's no eventual owning
		/// <see cref="TreemapView"/> at the top of the hierarchy.
		/// </summary>
		private const string _defaultPathSeparator = "/";

		/// <summary>
		/// The parent node of this node, if any.
		/// </summary>
		private TreemapNode? _parent = null;

		/// <summary>
		/// The parent <see cref="TreemapView"/>
		/// </summary>
		public TreemapView? _parentView = null;

		/// <summary>
		/// The collection of child nodes that belong to this node.
		/// The backing field for <see cref="Nodes"/>.
		/// </summary>
		private TreemapNodeCollection? _nodes = null;

		/// <summary>
		/// The backing field for <see cref="Value"/>.
		/// </summary>
		private TreemapNodeValueDataType? _value = null;

		/// <summary>
		/// The backing field for <see cref="ToolTipText"/>.
		/// </summary>
		private string _toolTipText = string.Empty;

		/// <summary>
		/// Constant bitmasks for state flags.
		/// </summary>
		[Flags]
		private enum States
		{
			Unknown = 0x0000_0000,

			Selected = 0x0000_0001,
			HotTracking = 0x0000_0002,

			MouseOver = 0x0000_0010,
			MouseOverCheckbox = 0x0000_0020,
			MouseOverPlusMinus = 0x0000_0040,
		}

		/// <summary>
		/// The bit flags for the <see cref="TreemapNode"/> states.
		/// </summary>
		private cmdwtf.Toolkit.StateFlags<States> _state = new(0);

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class.
		/// </summary>
		public TreemapNode()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class with the
		/// specified label text.
		/// </summary>
		/// <param name="text">
		/// The label <see cref="Text"/> of the new <see cref="TreemapNode"/>.
		/// </param>
		public TreemapNode(string text) : this()
		{
			Text = text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class with the
		/// specified label text and child <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="text">
		/// The label <see cref="TreemapNode"/>.Text of the new <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="children">
		/// An array of child <see cref="TreemapNode"/> objects.
		/// </param>
		public TreemapNode(string text, TreemapNode[] children) : this()
		{
			Text = text;
			Nodes.AddRange(children);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class with the
		/// specified label text and images to display when the <see cref="TreemapNode"/> is in a selected
		/// and unselected state.
		/// </summary>
		/// <param name="text">
		/// The label <see cref="TreemapNode"/>.Text of the new <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageIndex">
		/// The index value of System.Drawing.Image to display when the <see cref="TreemapNode"/> is unselected.
		/// </param>
		/// <param name="selectedImageIndex">
		/// The index value of System.Drawing.Image to display when the <see cref="TreemapNode"/> is selected.
		/// </param>
		public TreemapNode(string text, int imageIndex, int selectedImageIndex) : this()
		{
			Text = text;
			ImageIndex = imageIndex;
			SelectedImageIndex = selectedImageIndex;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class with the
		/// specified label text, child <see cref="TreemapNode"/>s, and images to display when the <see cref="TreemapNode"/>
		/// is in a selected and unselected state.
		/// </summary>
		/// <param name="text">
		/// The label <see cref="TreemapNode"/>.Text of the new <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageIndex">
		/// The index value of System.Drawing.Image to display when the <see cref="TreemapNode"/> is unselected.
		/// </param>
		/// <param name="selectedImageIndex">
		/// The index value of System.Drawing.Image to display when the <see cref="TreemapNode"/> is selected.
		/// </param>
		/// <param name="children">
		/// An array of child <see cref="TreemapNode"/> objects.
		/// </param>
		public TreemapNode(string text, int imageIndex, int selectedImageIndex, TreemapNode[] children) : this()
		{
			Text = text;
			ImageIndex = imageIndex;
			SelectedImageIndex = selectedImageIndex;
			Nodes.AddRange(children);
		}

		#endregion Constructors

		/// <summary>
		/// Is true when this node has no children.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Data)]
		public bool IsLeaf => Nodes.Count == 0;

		/// <summary>
		/// Is true when this node more than one child.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Data)]
		public bool IsBranch => Nodes.Count > 0;

		/// <summary>
		/// The value of this node.
		/// </summary>
		[Browsable(false)]
		[Category(Categories.Data)]
		public TreemapNodeValueDataType Value
		{
			get
			{
				if (_value.HasValue == false)
				{
					return _defaultLeafValue;
				}

				return _value.Value;
			}
			set
			{
				if (_value != value)
				{
					_value = value;
					ValueChanged?.Invoke(this, new TreemapNodeValueChangedEventArgs(this, value));
				}
			}
		}

		/// <summary>
		/// The <see cref="TreemapView"/> that this node belongs to. This property
		/// can not be set directly. Instead, add this node to <see cref="TreemapView.Nodes"/>,
		/// or as a child of a node that belongs to a <see cref="TreemapView"/> already.
		/// </summary>
		[Category(Categories.Data)]
		[Browsable(false)]
		public TreemapView? TreemapView
		{
			get
			{
				if (_parentView is null)
				{
					_parentView = FindTreemapView();
				}

				return _parentView;
			}
		}

		/// <summary>
		/// The value of this node, calculated with the <see cref="TreemapValueMode.LeavesOnly"/> mode.
		/// </summary>
		[Category(Categories.Data)]
		[Browsable(false)]
		public TreemapNodeValueDataType LeavesValue => GetValue(TreemapValueMode.LeavesOnly);

		/// <summary>
		/// The value of this node, calculated with the <see cref="TreemapValueMode.LeavesAndBranches"/> mode.
		/// </summary>
		[Category(Categories.Data)]
		[Browsable(false)]
		public TreemapNodeValueDataType TotalValue => GetValue(TreemapValueMode.LeavesAndBranches);

		/// <summary>
		/// Gets the position of the <see cref="TreemapNode"/> in the <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <returns>
		/// A zero-based index value that represents the position of the <see cref="TreemapNode"/> in the
		/// <see cref="Nodes"/> collection. Returns -1 if this node has no parent.
		/// </returns>
		[Category(Categories.Behavior)]
		public int Index => ParentInternal?.Nodes.IndexOf(this) ?? -1;

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is in an editable state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in editable state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		[Obsolete($"{nameof(IsEditing)} is not yet implemented.")] // #EDITING
		public bool IsEditing { get; internal set; } = false;

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is in the expanded state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in the expanded state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsExpanded { get; internal set; } = true;

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is in the collapsed state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in the collapsed state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsCollapsed => !IsExpanded;

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is in the selected state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in the selected state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsSelected
		{
			get => _state[States.Selected];
			internal set => _state[States.Selected] = value;
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is in the hot tracked state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in the hot tracked state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsHotTracking => _state[States.HotTracking];

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> has the mouse over it.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> has the cursor over it; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsMouseOver => _state[States.MouseOver];

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> has the mouse over its checkbox.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> has the cursor over its checkbox; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsMouseOverCheckbox => _state[States.MouseOverCheckbox];

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> has the mouse over its plus/minus.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> has the cursor over its plus/minus; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsMouseOverPlusMinus => _state[States.MouseOverPlusMinus];

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/>
		/// or any of it's ancestors is in the selected state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> or
		/// any of it's ancestors is in the selected state; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsSelectedInHirearchy
		{
			get
			{
				if (IsSelected)
				{
					return true;
				}

				if (!TreemapView?.FullBranchSelect ?? false)
				{
					return false;
				}

				TreemapNode? target = Parent;

				while (target != null)
				{
					if (target.IsSelected)
					{
						return true;
					}

					target = target.Parent;
				}

				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="TreemapNode"/> is visible or partially visible.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is visible or partially visible; otherwise, false.
		/// </returns>
		[Browsable(false)]
		public bool IsVisible => Bounds.IsEmpty == false;

		/// <summary>
		/// Gets the zero-based depth of the <see cref="TreemapNode"/> in the <see cref="TreemapView"/>
		/// control.
		/// </summary>
		/// <returns>
		/// The zero-based depth of the <see cref="TreemapNode"/> in the <see cref="TreemapView"/> control.
		/// </returns>
		[Browsable(false)]
		public int Level { get; private set; } = -1;

		/// <summary>
		/// How many layers deep this node is in the <see cref="TreemapView"/>. This value
		/// is the same as <see cref="Level"/> + 1.
		/// </summary>
		[Browsable(false)]
		internal int Depth => Level + 1;

		/// <summary>
		/// Gets the number of visible nodes in this node's collection.
		/// </summary>
		internal int VisibleCount
			=> Nodes.Where(n => n.IsVisible).Count() + Nodes.Sum(n => n.VisibleCount);

		#region Traversal

		/// <summary>
		/// Gets the parent <see cref="TreemapNode"/> of the current <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the parent of the current tree
		/// node, or null if there is no parent.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? Parent
		{
			// the public doesn't need to bother root nodes.
			get => _parent is not RootTreemapNode ? _parent : null;
			internal set
			{
				TreemapNode? previousParent = _parent;
				_parent = value;
				OnParentChanged(previousParent, _parent);
			}
		}

		/// <summary>
		/// Gets the parent <see cref="TreemapNode"/> of the current <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the parent of the current tree
		/// node, or null if there is no parent. This is similar to <see cref="Parent"/>,
		/// but will return the parent even if it is a <see cref="RootTreemapNode"/>, where
		/// as the regular <see cref="Parent"/> property will not.
		/// </returns>
		internal TreemapNode? ParentInternal
		{
			get => _parent;
		}

		/// <summary>
		/// Gets the first child <see cref="TreemapNode"/> in the <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <returns>
		/// The first child <see cref="TreemapNode"/> in the <see cref="Nodes"/>,
		/// or null if there is no first node.
		/// collection.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? FirstNode => Nodes.FirstOrDefault();

		/// <summary>
		/// Gets the last child <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the last child <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? LastNode => Nodes.LastOrDefault();

		/// <summary>
		/// Gets the next sibling <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the next sibling <see cref="TreemapNode"/>,
		/// or null if there is no next sibling.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? NextNode
		{
			get
			{
				if (ParentInternal == null)
				{
					return null;
				}

				int nextIndex = Index + 1;
				if (nextIndex < Nodes.Count)
				{
					return ParentInternal.Nodes[nextIndex];
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the previous sibling <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the previous sibling <see cref="TreemapNode"/>,
		/// or null if there is no previous sibling.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? PrevNode
		{
			get
			{
				if (ParentInternal == null)
				{
					return null;
				}

				// move previous one in the list if we can.
				int prevIndex = Index - 1;
				if (prevIndex >= 0)
				{
					return ParentInternal.Nodes[prevIndex];
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the next visible <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the next visible <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? NextVisibleNode
		{
			get
			{
				TreemapNode result = this;

				while (result.NextNode is not null)
				{
					result = result.NextNode;

					if (result.IsVisible)
					{
						return result;
					}
				}

				if (ParentInternal is not null)
				{
					return FirstVisibleParent?.NextVisibleNode;
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the previous visible <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the previous visible <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		public TreemapNode? PrevVisibleNode
		{
			get
			{
				TreemapNode? result = this;

				while (result.PrevNode is not null)
				{
					result = result.PrevNode;

					if (result.IsVisible)
					{
						return result;
					}
				}

				if (ParentInternal is not null)
				{
					result = FirstVisibleParent;

					// roots arent 'real' nodes, don't return them.
					if (result is RootTreemapNode)
					{
						return null;
					}

					return result;
				}

				return null;
			}
		}

		/// <summary>
		/// Traverses up the tree, looking for the first visible parent.
		/// </summary>
		private TreemapNode? FirstVisibleParent
		{
			get
			{
				if (ParentInternal == null)
				{
					return null;
				}

				TreemapNode? result = ParentInternal;

				while (result is not null && result.IsVisible == false)
				{
					result = result.ParentInternal;
				}

				return result;
			}
		}

		/// <summary>
		/// Attempts to traverse down the tree, finding the furthest deep
		/// <see cref="TreemapNode"/> at the given position.
		/// </summary>
		/// <param name="pt">The point to test.</param>
		/// <returns>The deepest node at the contained position, or null otherwise.</returns>
		internal TreemapNode? ChildNodeAt(Point pt)
			=> ChildNodeAt(pt.X, pt.Y);

		/// <summary>
		/// Attempts to traverse down the tree, finding the furthest deep
		/// <see cref="TreemapNode"/> at the given position.
		/// </summary>
		/// <param name="x">The x position.</param>
		/// <param name="y">The y position.</param>
		/// <returns>The deepest node at the contained position, or null otherwise.</returns>
		internal TreemapNode? ChildNodeAt(int x, int y)
		{
			if (Bounds.Contains(x, y) == false)
			{
				return null;
			}

			if (Nodes.Count == 0)
			{
				return this;
			}

			foreach (TreemapNode node in Nodes)
			{
				TreemapNode? result = node.ChildNodeAt(x, y);
				if (result is not null)
				{
					return result;
				}
			}

			// branches have bounds that can contain points, but won't have children that do.
			return IsBranch
				? this
				: null;
		}

		/// <summary>
		/// Gets a <see cref="TreemapViewHitTestLocations"/> from the given point.
		/// </summary>
		/// <param name="pt">The <see cref="Point"/> to test.</param>
		/// <returns>A <see cref="TreemapViewHitTestLocations"/> representing the result of the test.</returns>
		internal TreemapViewHitTestLocations GetHitTestLocation(Point pt)
			=> GetHitTestLocation(pt.X, pt.Y);


		/// <summary>
		/// Gets a <see cref="TreemapViewHitTestLocations"/> from the given point.
		/// </summary>
		/// <param name="x">The x coordinate to test.</param>
		/// <param name="y">The y coordiante to test.</param>
		/// <returns>A <see cref="TreemapViewHitTestLocations"/> representing the result of the test.</returns>
		internal virtual TreemapViewHitTestLocations GetHitTestLocation(int x, int y)
		{
			// are we in our client area at all?
			if (Bounds.Contains(x, y) == false)
			{
				return TreemapViewHitTestLocations.None;
			}

			if (TreemapView is null)
			{
				throw new InvalidOperationException($"{nameof(GetHitTestLocation)} cannot be called while the {nameof(TreemapNode)} has no parent view.");
			}

			// start with empty, and add as we go.
			TreemapViewHitTestLocations result = TreemapViewHitTestLocations.Unknown;

			RectangleF gridPaddedBounds = this.GetGridPaddedBoundsF(TreemapView);
			RectangleF paddedBounds = this.GetPaddedBoundsF(TreemapView);
			int gridThickness = this.GetGridThickness(TreemapView);

			if (gridPaddedBounds.Contains(x, y))
			{
				// we are in the client area proper?
				result |= TreemapViewHitTestLocations.Node;

				// check for checkbox,
				if (CheckboxArea.Contains(x, y))
				{
					result |= TreemapViewHitTestLocations.StateImage;
				}

				// check for [+]/[-]
				if (PlusMinusArea.Contains(x, y))
				{
					result |= TreemapViewHitTestLocations.PlusMinus;
				}

				// check for image,
				if (ImageArea.Contains(x, y))
				{
					result |= TreemapViewHitTestLocations.Image;
				}
			}
			else if (paddedBounds.Contains(x, y))
			{
				if (TreemapView.ShowGrid)
				{
					// we are in the grid. Lets find out which sides.

					if (Math.Abs(paddedBounds.Top - y) <= gridThickness)
					{
						result |= TreemapViewHitTestLocations.AboveClientArea;
					}
					if (Math.Abs(paddedBounds.Bottom - y) <= gridThickness)
					{
						result |= TreemapViewHitTestLocations.BelowClientArea;
					}
					if (Math.Abs(paddedBounds.Left - x) <= gridThickness)
					{
						result |= TreemapViewHitTestLocations.LeftOfClientArea;
					}
					if (Math.Abs(paddedBounds.Right - x) <= gridThickness)
					{
						result |= TreemapViewHitTestLocations.RightOfClientArea;
					}
				}
				else
				{
					result = TreemapViewHitTestLocations.Unknown;
				}
			}
			else
			{
				// we are in the the padding.
				result |= TreemapViewHitTestLocations.Indent;
			}

			return result;
		}

		#endregion Traversal

		#region ImageList

		/// <summary>
		/// An image indexer for the default image for a <see cref="TreemapNode"/>.
		/// </summary>
		[Lazy]
		internal TreemapNodeImageIndexer ImageIndexer
			=> new(this, Treemap.TreemapNodeImageIndexer.ImageListType.Default);

		/// <summary>
		/// An image indexer for the image for when the <see cref="TreemapNode"/> is selected.
		/// </summary>
		[Lazy]
		internal TreemapNodeImageIndexer SelectedImageIndexer
			=> new(this, Treemap.TreemapNodeImageIndexer.ImageListType.Default);

		/// <summary>
		/// An indexer for the image for a <see cref="TreemapNode"/> when the node is displaying it's checked state.
		/// </summary>
		[Lazy]
		internal TreemapNodeImageIndexer StateImageIndexer
			=> new(this, Treemap.TreemapNodeImageIndexer.ImageListType.State);

		/// <summary>
		/// Gets or sets the key for the image associated with this <see cref="TreemapNode"/> when the node
		/// is in an unselected state.
		/// </summary>
		/// <returns>
		/// The key for the image associated with this <see cref="TreemapNode"/> when the node is in an unselected
		/// state.
		/// </returns>
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreemapView.ImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		public string ImageKey
		{
			get => ImageIndexer.Key;
			set => ImageIndexer.Key = value;
		}

		/// <summary>
		/// Gets or sets the image list index value of the image displayed when the tree
		/// node is in the unselected state.
		/// </summary>
		/// <returns>
		/// A zero-based index value that represents the image position in the assigned <see cref="ImageList"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// .NET 5.0 and later: value is out of range.
		/// </exception>
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreemapView.ImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		public int ImageIndex
		{
			get => ImageIndexer.Index;
			set => ImageIndexer.Index = value;
		}

		/// <summary>
		/// Gets or sets the image list index value of the image that is displayed when the
		/// <see cref="TreemapNode"/> is in the selected state.
		/// </summary>
		/// <returns>
		/// A zero-based index value that represents the image position in an System.Windows.Forms.ImageList.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// .NET 5.0 and later: value is out of range.
		/// </exception>
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreemapView.ImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		public int SelectedImageIndex
		{
			get => SelectedImageIndexer.Index;
			set => SelectedImageIndexer.Index = value;
		}

		/// <summary>
		/// Gets or sets the key of the image displayed in the <see cref="TreemapNode"/> when it is in a
		/// selected state.
		/// </summary>
		/// <returns>
		/// The key of the image displayed when the <see cref="TreemapNode"/> is in a selected state.
		/// </returns>
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreemapView.ImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(TreeViewImageKeyConverter))]
		public string SelectedImageKey
		{
			get => SelectedImageIndexer.Key;
			set => SelectedImageIndexer.Key = value;
		}

		/// <summary>
		/// Gets or sets the key of the image that is used to indicate the state of the <see cref="TreemapNode"/>
		/// when the parent <see cref="TreemapView"/> has its <see cref="TreemapView.CheckBoxes"/>
		/// property set to false.
		/// </summary>
		/// <returns>
		/// The key of the image that is used to indicate the state of the <see cref="TreemapNode"/>.
		/// </returns>
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(ImageKeyConverter))]
		public string StateImageKey
		{
			get => StateImageIndexer.Key;
			set => StateImageIndexer.Key = value;
		}

		/// <summary>
		/// Gets or sets the index of the image that is used to indicate the state of the
		/// <see cref="TreemapNode"/> when the parent <see cref="TreemapView"/> has
		/// its <see cref="TreemapView.CheckBoxes"/> property set to false.
		/// </summary>
		/// <returns>
		/// The index of the image that is used to indicate the state of the <see cref="TreemapNode"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The specified index is less than -1 or greater than 14.
		/// </exception>
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("TreeView.StateImageList")]
		[Category(Categories.Behavior)]
		[TypeConverter(typeof(NoneExcludedImageIndexConverter))]
		public int StateImageIndex
		{
			get => StateImageIndexer.Index;
			set => StateImageIndexer.Index = value;
		}

		#endregion ImageList

		/// <summary>
		/// Gets or sets the object that contains data about the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="object"/> that contains data about the <see cref="TreemapNode"/>. The default is null.
		/// </returns>
		[Bindable(true)]
		[DefaultValue(null)]
		[Localizable(false)]
		[Category(Categories.Data)]
		[TypeConverter(typeof(StringConverter))]
		public object? Tag { get; set; } = null;

		/// <summary>
		/// Gets or sets the text displayed in the label of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The text displayed in the label of the <see cref="TreemapNode"/>.
		/// </returns>
		[Localizable(true)]
		[Category(Categories.Appearance)]
		public string Text { get; set; } = nameof(TreemapNode);

		/// <summary>
		/// Gets or sets the text that appears when the mouse pointer hovers over a <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// Gets the text that appears when the mouse pointer hovers over a <see cref="TreemapNode"/>.
		/// </returns>
		[DefaultValue("")]
		[Localizable(false)]
		[Category(Categories.Appearance)]
		public string ToolTipText
		{
			get => string.IsNullOrEmpty(_toolTipText) ? Text : _toolTipText;
			set => _toolTipText = value;
		}

		/// <summary>
		/// Gets the collection of <see cref="TreemapNode"/> objects assigned to the
		/// current <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="TreemapNodeCollection"/> that represents the <see cref="TreemapNode"/>s assigned
		/// to the current <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		[ListBindable(false)]
		public TreemapNodeCollection Nodes
		{
			get
			{
				if (_nodes == null)
				{
					_nodes = new TreemapNodeCollection(this);
				}

				return _nodes;
			}
		}

		/// <summary>
		/// Gets the path from the root <see cref="TreemapNode"/> to the current <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The path from the root <see cref="TreemapNode"/> to the current <see cref="TreemapNode"/>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// The node is not contained in a <see cref="TreemapView"/>.
		/// </exception>
		[Browsable(false)]
		public string FullPath { get; private set; } = string.Empty;

		/// <summary>
		/// Gets the handle of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="TreemapNode"/> handle.
		/// </returns>
		/// <exception cref="NotSupportedException">
		/// Handle is not currently supported in <see cref="TreemapNode"/>.
		/// </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"{nameof(Handle)} does not support the use of a handle, as it's not an actual control.", error: true)]
		public IntPtr Handle
			=> throw new NotSupportedException($"{nameof(TreemapNode)} doesn't have a handle, as it's not a full control.");

		/// <summary>
		/// Gets the bounds of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The rounded <see cref="Rectangle"/> that represents the bounds of the <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		public Rectangle Bounds => Rectangle.Round(BoundsF);

		/// <summary>
		/// Gets the bounds of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="RectangleF"/> that represents the bounds of the <see cref="TreemapNode"/>.
		/// </returns>
		[Browsable(false)]
		public RectangleF BoundsF { get; private set; } = RectangleF.Empty;

		/// <summary>
		/// Gets or sets the shortcut menu associated with this <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="ContextMenuStrip"/> associated with the <see cref="TreemapNode"/>.
		/// </returns>
		[DefaultValue(null)]
		[Category(Categories.Behavior)]
		public virtual ContextMenuStrip? ContextMenuStrip { get; set; } = null;

		/// <summary>
		/// Gets or sets the name of the <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents the name of the <see cref="TreemapNode"/>.
		/// </returns>
		[Category(Categories.Appearance)]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="TreemapNode"/> is in a checked state.
		/// </summary>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is in a checked state; otherwise, false.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(false)]
		public bool Checked
		{
			get => CheckState == CheckState.Checked;
			set => CheckState = value ? CheckState.Checked : CheckState.Unchecked;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="TreemapNode"/> is in a checked state.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Windows.Forms.CheckState"/> representing the current check state of this node.
		/// </returns>
		[Category(Categories.Appearance)]
		[DefaultValue(CheckState.Unchecked)]
		public CheckState CheckState
		{
			get => CheckStateInternal;
			set
			{
				bool shouldCancel = TreemapView?.DoBeforeCheck(this, TreemapViewAction.Unknown) ?? false;

				if (shouldCancel)
				{
					return;
				}

				CheckStateInternal = value;

				TreemapView?.DoAfterCheck(this, TreemapViewAction.Unknown);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="TreemapNode"/> is in a checked state.
		/// This is the backing value for <see cref="CheckState"/> and <see cref="Checked"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Windows.Forms.CheckState"/> representing the current check state of this node.
		/// </returns>
		internal CheckState CheckStateInternal { get; set; } = CheckState.Unchecked;

		#region Events

		/// <summary>
		/// Occurs after the <see cref="TreemapNode.Value"/> is changed.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeValueChangedEventHandler? ValueChanged;

		/// <summary>
		/// Occurs when the <see cref="TreemapNode"/> is added to a new parent.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeEventHandler? AddedToParent;

		/// <summary>
		/// Occurs when the <see cref="TreemapNode"/> is removed from it's parent.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeEventHandler? RemovedFromParent;

		/// <summary>
		/// Occurs when the <see cref="TreemapNode"/>'s parent is changed.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeEventHandler? ParentChanged;

		/// <summary>
		/// Occurs when the <see cref="TreemapNode"/> has been collapsed.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeEventHandler? Collapsed;

		/// <summary>
		/// Occurs when the <see cref="TreemapNode"/> has been expanded.
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		public event TreemapNodeEventHandler? Expanded;

		/// <summary>
		/// Occurs when there is some structural change in the decendants
		/// from this node. (E.g: A Child is added or removed.)
		/// </summary>
		[Category(Categories.Behavior)]
		[DefaultValue(null)]
		internal event TreemapNodeEventHandler? SubtreeChanged;

		#endregion Events

		/// <summary>
		/// Returns the <see cref="TreemapNode"/> with the specified handle and assigned to the specified
		/// <see cref="TreemapView"/>.
		/// </summary>
		/// <param name="tree">
		/// The <see cref="TreemapView"/> that contains the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="handle">
		/// The handle of the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// A <see cref="TreemapNode"/> that represents the <see cref="TreemapNode"/> assigned to the
		/// specified <see cref="TreemapView"/> control with the specified handle.
		/// </returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete($"{nameof(TreeNode)} does not support the use of a handle, as it's not an actual control.", error: true)]
		public static TreemapNode FromHandle(TreemapView tree, IntPtr handle)
			=> throw new NotSupportedException($"{nameof(TreemapNode)} doesn't have a handle, as it's not a full control.");

		/// <summary>
		/// Initiates the editing of the <see cref="TreemapNode"/> label.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <see cref="TreemapView.LabelEdit"/> is set to false.
		/// </exception>
		[Obsolete($"{nameof(BeginEdit)} is not yet implemented.")] // #EDITING
		public void BeginEdit() => throw new NotImplementedException();

		/// <summary>
		/// Ends the editing of the <see cref="TreemapNode"/> label.
		/// </summary>
		/// <param name="cancel">
		/// true if the editing of the <see cref="TreemapNode"/> label text was canceled without being saved;
		/// otherwise, false.
		/// </param>
		[Obsolete($"{nameof(EndEdit)} is not yet implemented.")] // #EDITING
		public void EndEdit(bool cancel) => throw new NotImplementedException();

		#region Collapsing

		/// <summary>
		/// Collapses the <see cref="TreemapNode"/> and optionally collapses its children.
		/// </summary>
		/// <param name="ignoreChildren">
		/// true to leave the child nodes in their current state; false to collapse the child
		/// nodes.
		/// </param>
		public void Collapse(bool ignoreChildren)
		{
			Collapse();

			if (ignoreChildren == false)
			{
				foreach (TreemapNode child in Nodes)
				{
					child.Collapse(ignoreChildren);
				}
			}
		}

		/// <summary>
		/// Collapses the <see cref="TreemapNode"/>.
		/// </summary>
		public void Collapse()
		{
			if (IsExpanded == false)
			{
				return;
			}

			TreemapViewCancelEventArgs beforeArgs = new(this, false, TreemapViewAction.Collapse);
			TreemapView?.OnBeforeCollapse(beforeArgs);

			if (beforeArgs.Cancel)
			{
				return;
			}

			IsExpanded = false;

			Collapsed?.Invoke(this, new TreemapNodeEventArgs(this));

			TreemapView?.OnAfterCollapse(new TreemapViewEventArgs(this, TreemapViewAction.Collapse));
		}

		/// <summary>
		/// Expands the <see cref="TreemapNode"/>.
		/// </summary>
		public void Expand()
		{
			if (IsExpanded)
			{
				return;
			}

			TreemapViewCancelEventArgs beforeArgs = new(this, false, TreemapViewAction.Expand);
			TreemapView?.OnBeforeExpand(beforeArgs);

			if (beforeArgs.Cancel)
			{
				return;
			}

			IsExpanded = true;

			Expanded?.Invoke(this, new TreemapNodeEventArgs(this));

			TreemapView?.OnAfterExpand(new TreemapViewEventArgs(this, TreemapViewAction.Expand));
		}

		/// <summary>
		/// Expands all the child <see cref="TreemapNode"/>s.
		/// </summary>
		public void ExpandAll()
		{
			Expand();
			foreach (TreemapNode child in Nodes)
			{
				child.ExpandAll();
			}
		}

		/// <summary>
		/// Ensures that the <see cref="TreemapNode"/> is visible, expanding <see cref="TreemapNode"/>s and scrolling the
		/// <see cref="TreemapView"/> as necessary.
		/// </summary>
		public void EnsureVisible()
		{
			Expand();
			ParentInternal?.EnsureVisible();
		}

		#endregion Collapsing

		/// <summary>
		/// Returns the number of child <see cref="TreemapNode"/>s.
		/// </summary>
		/// <param name="includeSubTrees">
		/// true if the resulting count includes all <see cref="TreemapNode"/>s indirectly rooted at this
		/// <see cref="TreemapNode"/>; otherwise, false.
		/// </param>
		/// <returns>
		/// The number of child <see cref="TreemapNode"/>s assigned to the <see cref="Nodes"/>
		/// collection.
		/// </returns>
		public int GetNodeCount(bool includeSubTrees)
		{
			int result = Nodes.Count;
			if (includeSubTrees)
			{
				foreach (TreemapNode child in Nodes)
				{
					result += child.GetNodeCount(includeSubTrees);
				}
			}

			return result;
		}

		/// <summary>
		/// Raises the <see cref="ParentChanged"/>, <see cref="AddedToParent"/>, and
		/// <see cref="RemovedFromParent"/> depending on the values passed.
		/// and recalculates internal values.
		/// </summary>
		/// <param name="previousParent">The previosu parent node, or null if there was no previous parent.</param>
		/// <param name="parent">The new parent node, or null if it has been removed from a parent.</param>
		protected virtual void OnParentChanged(TreemapNode? previousParent, TreemapNode? parent)
		{
			TreemapNodeEventArgs args = new(this);

			if (previousParent is not null && parent is null)
			{
				previousParent.OnChildRemoved(this);
				RemovedFromParent?.Invoke(this, args);
			}

			// check for being added to a new parent
			if (previousParent is null && parent is not null)
			{
				parent.OnChildAdded(this);
				AddedToParent?.Invoke(this, args);
			}

			RecalculateInternalValues();
			ParentChanged?.Invoke(this, args);
		}

		/// <summary>
		/// A helper handler that raises the <see cref="SubtreeChanged"/> event.
		/// </summary>
		/// <param name="removedChild">The child that was removed.</param>
		protected virtual void OnChildRemoved(TreemapNode removedChild) => OnSubtreeChanged(this, removedChild);

		/// <summary>
		/// A helper handler that raises the <see cref="SubtreeChanged"/> event.
		/// </summary>
		/// <param name="newChild">The child that was added.</param>
		protected virtual void OnChildAdded(TreemapNode newChild) => OnSubtreeChanged(this, newChild);

		/// <summary>
		/// A helper handler that raises the <see cref="SubtreeChanged"/> event.
		/// </summary>
		/// <param name="nodeEffected">The node that started the tree change propigation.</param>
		/// <param name="child">The child node that was added or removed.</param>
		protected virtual void OnSubtreeChanged(TreemapNode nodeEffected, TreemapNode child)
		{
			// raise the event
			TreemapNodeEventArgs args = new(nodeEffected);
			SubtreeChanged?.Invoke(this, args);

			// and propagate the event up the tree.
			ParentInternal?.OnSubtreeChanged(nodeEffected, child);
		}
		/// <summary>
		/// Handles the mouse cursor hovering over this node.
		/// This requires the cursor to stop moving momentarily.
		/// This will handle switching the node into the hot tracking state,
		/// if <see cref="TreemapView.HotTracking"/> is enabled.
		/// </summary>
		/// <param name="hit">The information about the test performed to determine the mouse was over this node.</param>
		internal virtual void OnMouseHover(TreemapViewHitTestInfo hit)
		{
			// i was doing hot tracking starting here,
			// but hover took too long to make it feel right.
			// instead, it's being handled in OnMouseOver now.
		}

		/// <summary>
		/// Handles the mouse cursor being over this node,
		/// raised most often from a mouse move event.
		/// </summary>
		/// <param name="hit">The information about the test performed to determine the mouse was over this node.</param>
		internal virtual void OnMouseOver(TreemapViewHitTestInfo hit)
		{
			if (TreemapView is null)
			{
				// nothing to do if we don't have a view.
				return;
			}

			// mouseover node in general
			if (!_state[States.MouseOver])
			{
				_state[States.MouseOver] = true;
			}

			// mouseover checkbox
			bool hittingCb = hit.Location.HasFlag(TreemapViewHitTestLocations.StateImage);
			if (!_state[States.MouseOverCheckbox] && hittingCb)
			{
				_state[States.MouseOverCheckbox] = true;
				TreemapView.Invalidate(CheckboxArea);
			}
			else if (_state[States.MouseOverCheckbox] && !hittingCb)
			{
				_state[States.MouseOverCheckbox] = false;
				TreemapView.Invalidate(CheckboxArea);
			}

			// mouseover [+]/[-]
			bool hittingPm = hit.Location.HasFlag(TreemapViewHitTestLocations.PlusMinus);
			if (!_state[States.MouseOverPlusMinus] && hittingPm)
			{
				_state[States.MouseOverPlusMinus] = true;
				TreemapView.Invalidate(PlusMinusArea);
			}
			else if (_state[States.MouseOverPlusMinus] && !hittingPm)
			{
				_state[States.MouseOverPlusMinus] = false;
				TreemapView.Invalidate(PlusMinusArea);
			}

			// hot tracking:
			// we need to not be hot tracking already,
			// as well as not over the the checkbox, or plusminus.
			// as well, hot tracking must be enabled at the view,
			// and checkboxes must be disabled. that is a limitation
			// of the System.Windows.Forms.TreeView control, and may
			// be something dropped as a requirement for this control later.
			if (!_state[States.HotTracking] &&
				!hittingCb &&
				!hittingPm &&
				TreemapView.HotTracking &&
				!TreemapView.CheckBoxes)
			{
				_state[States.HotTracking] = true;
				TreemapView.Invalidate(Bounds);
			}
			else if (_state[States.HotTracking])
			{
				// don't hot track if we're over a checkbox or [+]/[-]
				if (hittingCb || hittingPm)
				{
					_state[States.HotTracking] = false;
					TreemapView.Invalidate(Bounds);
				}
			}
		}

		/// <summary>
		/// Handles the mouse cursor leaving this node.
		/// </summary>
		internal virtual void OnMouseLeave()
		{
			if (_state[States.HotTracking])
			{
				TreemapView?.Invalidate(Bounds);
			}

			if (_state[States.MouseOverCheckbox])
			{
				TreemapView?.Invalidate(CheckboxArea);
			}

			if (_state[States.MouseOverPlusMinus])
			{
				TreemapView?.Invalidate(PlusMinusArea);
			}

			_state[States.MouseOver] = false;
			_state[States.MouseOverCheckbox] = false;
			_state[States.HotTracking] = false;
		}

		/// <summary>
		/// Shows the <see cref="ContextMenuStrip"/> if this <see cref="TreemapNode"/> has one.
		/// </summary>
		/// <param name="location">Where (in screen coordinates) to show the menu.</param>
		/// <returns>true if the menu was shown; otherwise false.</returns>
		internal bool ShowContextMenuStrip(Point location)
		{
			if (ContextMenuStrip is null)
			{
				return false;
			}

			ContextMenuStrip.Show(location);
			return true;
		}

		/// <summary>
		/// Recalculates internal values that depend
		/// on where in the hierarchy this node is.
		/// </summary>
		protected virtual void RecalculateInternalValues()
		{
			if (string.IsNullOrEmpty(Name))
			{
				Name = Text;
			}

			_parentView = null;

			Level = 0;

			string pathSep = _defaultPathSeparator;
			TreemapNode? target = this;
			Stack<string> steps = new();
			steps.Push(Name);

			while (target != null)
			{
				steps.Push(target.Name);
				target = target.ParentInternal;

				if (target is RootTreemapNode root)
				{
					_parentView = root.ParentView;
					pathSep = root.ParentView.PathSeparator;
					break;
				}

				if (target == null)
				{
					break;
				}

				Level++;
			}

			// build the full path from all the node steps collected.
			FullPath = $"{pathSep}{string.Join(pathSep, steps)}";

			// force each child to recalculate itself down the tree.
			foreach (TreemapNode child in Nodes)
			{
				child.RecalculateInternalValues();
			}
		}

		/// <summary>
		/// Removes the current <see cref="TreemapNode"/> from the <see cref="TreemapView"/>.
		/// </summary>
		public void Remove() => ParentInternal?.Nodes.Remove(this);

		/// <summary>
		/// Toggles the <see cref="TreemapNode"/> to either the expanded or collapsed state.
		/// </summary>
		public void Toggle()
		{
			if (IsExpanded)
			{
				Collapse();
			}
			else
			{
				Expand();
			}
		}

		/// <summary>
		/// Gets the value for this node, based on the given mode.
		/// </summary>
		/// <param name="mode">The mode to calculate the value by.</param>
		/// <returns>The node's value.</returns>
		internal TreemapNodeValueDataType GetValue(TreemapValueMode mode)
		{
			if (Nodes.Count == 0)
			{
				return Value;
			}

			TreemapNodeValueDataType result = mode switch
			{
				TreemapValueMode.LeavesOnly => 0,
				TreemapValueMode.LeavesAndBranches => Value,
				_ => throw new NotImplementedException()
			};

			foreach (TreemapNode child in Nodes)
			{
				result += mode switch
				{
					TreemapValueMode.LeavesOnly => child.GetValue(mode),
					TreemapValueMode.LeavesAndBranches => child.GetValue(mode),
					_ => throw new NotImplementedException()
				};
			}

			return result;
		}

		/// <summary>
		/// Crawls up the hierarchy and returns
		/// the top most node's parent view.
		/// </summary>
		/// <returns>The <see cref="TreemapView"/> this node belongs to.</returns>
		internal virtual TreemapView? FindTreemapView()
		{
			TreemapNode target = this;
			while (target.ParentInternal is not null)
			{
				target = target.ParentInternal;
			}

			return target._parentView;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A <see cref="string"/> that represents the current object.
		/// </returns>
		public override string ToString() => $"{Text} - ({Value}) '{Name}'";

#if TREEMAP_NODE_ISERIALIZABLE

		#region ISerializable Implementation

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNode"/> class using the
		/// specified serialization information and context.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo"/> that contains the data to
		/// deserialize the class.
		/// </param>
		/// <param name="context">
		/// The <see cref="StreamingContext"/> that contains the source and
		/// destination of the serialized stream.
		/// </param>
		protected TreemapNode(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Saves the state of the <see cref="TreemapNode"/> to the specified <see cref="SerializationInfo"/>.
		/// </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The state of the stream during serialization.</param>
		protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(Name), Name);
			info.AddValue(nameof(Text), Text);
			info.AddValue(nameof(Value), Value);
		}

		/// <summary>
		/// Saves the state of the <see cref="TreemapNode"/> to the specified <see cref="SerializationInfo"/>.
		/// </summary>
		/// <param name="info">The serialization information.</param>
		/// <param name="context">The state of the stream during serialization.</param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			GetObjectData(info, context);
		}

		#endregion ISerializable Implementation

#endif // TREEMAP_NODE_ISERIALIZABLE

		#region ICloneable Implementation

		/// <summary>
		/// Copies the <see cref="TreemapNode"/> and the entire subtree rooted at this <see cref="TreemapNode"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="object"/> that represents the cloned <see cref="TreemapNode"/>.
		/// </returns>
		public virtual object Clone()
		{
			// start with a memberwise clone.
			TreemapNode clone = MemberwiseClone() as TreemapNode
				?? throw new Exception($"Failed to clone {nameof(TreemapNode)} {Name}");

			// force the creation of a new nodes collection,
			// and remove our parent.
			clone._parent = null;
			clone._parentView = null;
			clone._nodes = null;

			// and now all the children.
			foreach (TreemapNode child in Nodes)
			{
				var childClone = (TreemapNode)child.Clone();
				clone.Nodes.Add(childClone);
			}

			clone.RecalculateInternalValues();

			return clone;
		}

		#endregion ICloneable Implementation
	}
}
