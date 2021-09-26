using System;
using System.Runtime.InteropServices;

namespace cmdwtf.Treemap
{
	/// <summary>Defines constants that represent areas of a <see cref="TreemapView"/> or <see cref="TreemapView"/>.</summary>
	[Flags]
	[ComVisible(true)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "'Label' is the traditional 'Meat' of the TreeNode, so we are leaving that value for compatibility.")]
	public enum TreemapViewHitTestLocations
	{
		/// <summary>An unknown location, with no flags set.</summary>
		Unknown = 0,
		/// <summary>A position in the client area of the <see cref="TreemapView"/> control, but not on a node or a portion of a node.</summary>
		None = 1,
		/// <summary>A position within the bounds of an image contained on a <see cref="TreemapView"/> or <see cref="TreemapNode"/>.</summary>
		Image = 2,
		/// <summary>A position on the text portion of a <see cref="TreemapNode"/>.</summary>
		Label = 4,
		/// <summary>A position on the client portion of a <see cref="TreemapNode"/>. This is the same as <see cref="Label"/></summary>
		Node = 4,
		/// <summary>A position on the client portion of a <see cref="TreemapNode"/>. This is the same as <see cref="Image"/>, <see cref="Label"/>, and <see cref="StateImage"/></summary>
		OnItem = Image | Label | StateImage,
		/// <summary>A position in the indentation area for a <see cref="TreemapNode"/>.</summary>
		Indent = 8,
		/// <summary>A position above the client portion of a <see cref="TreemapNode"/> control.</summary>
		AboveClientArea = 256,
		/// <summary>A position below the client portion of a <see cref="TreemapNode"/> control.</summary>
		BelowClientArea = 512,
		/// <summary>A position to the left of the client area of a <see cref="TreemapNode"/> control.</summary>
		LeftOfClientArea = 2048,
		/// <summary>A position to the right of the client area of the <see cref="TreemapNode"/> control.</summary>
		RightOfClientArea = 1024,
		/// <summary>A position to the right of the text area of a <see cref="TreemapNode"/>.</summary>
		RightOfLabel = 32,
		/// <summary>A position within the bounds of a state image for a <see cref="TreemapNode"/>.</summary>
		StateImage = 64,
		/// <summary>A position on the plus/minus area of a <see cref="TreemapNode"/>.</summary>
		PlusMinus = 16,

	}
}