using System;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// An enumeration describing which step of the node should be drawing.
	/// </summary>
	[Flags]
	public enum TreemapNodeDrawStep
	{
		/// <summary>
		/// An unknown step. This value shouldn't be used.
		/// </summary>
		Unknown = 0x0000_0000,
		/// <summary>
		/// The node should be drawing it's background.
		/// </summary>
		Background = 0x0000_0001,
		/// <summary>
		/// The node should be drawing it's foreground and text.
		/// </summary>
		Foreground = 0x0000_0002,
		/// <summary>
		/// The node should be drawing it's grid.
		/// </summary>
		Grid = 0x0000_0004,
		/// <summary>
		/// The node should be drawing any overlays, including
		/// state (checkbox) and plus/minus glyphs.
		/// </summary>
		Overlay = 0x0000_0008,
	}
}
