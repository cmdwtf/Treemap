using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides data for the <see cref="TreemapView"/>.BeforeLabelEdit and
	/// <see cref="TreemapView"/>.AfterLabelEdit events.
	/// </summary>
	public class TreemapNodeLabelEditEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNodeLabelEditEventArgs"/>
		/// class for the specified System.Windows.Forms.TreeNode.
		/// </summary>
		/// <param name="node">The treemap node containing the text to edit.</param>
		public TreemapNodeLabelEditEventArgs(TreemapNode node)
		{
			Node = node;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TreemapNodeLabelEditEventArgs"/>
		/// class for the specified <see cref="TreemapNode"/> and the specified text
		/// with which to update the treemap node label.
		/// </summary>
		/// <param name="node">The treemap node containing the text to edit.</param>
		/// <param name="label">The new text to associate with the treemap node.</param>
		public TreemapNodeLabelEditEventArgs(TreemapNode node, string label) : this(node)
		{
			Label = label;
		}

		/// <summary>
		/// Gets the treemap node containing the text to edit.
		/// </summary>
		public TreemapNode Node { get; }

		/// <summary>
		/// Gets the new text to associate with the treemap node, or null if the user cancels the edit.
		/// </summary>
		public string? Label { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the edit has been canceled.
		/// </summary>
		public bool CancelEdit { get; set; }
	}
}