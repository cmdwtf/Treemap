namespace cmdwtf.Treemap
{
	/// <summary>
	/// Specifies the action that raised a <see cref="TreemapViewEventArgs"/> event.
	/// </summary>
	public enum TreemapViewAction
	{
		/// <summary>
		/// The action that caused the event is unknown.
		/// </summary>
		Unknown,

		/// <summary>
		/// The event was caused by a keystroke.
		/// </summary>
		ByKeyboard,

		/// <summary>
		/// The event was caused by a mouse operation.
		/// </summary>
		ByMouse,

		/// <summary>
		/// The event was caused by the <see cref="TreemapNode"/> collapsing.
		/// </summary>
		Collapse,

		/// <summary>
		/// The event was caused by the <see cref="TreemapNode"/> expanding.
		/// </summary>
		Expand,
	}
}
