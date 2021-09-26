
namespace cmdwtf.Treemap
{
	/// <summary>
	/// Defines constants that represent the ways a <see cref="TreemapView"/> can
	/// be drawn.
	/// </summary>
	public enum TreemapViewDrawMode
	{
		/// <summary>
		/// The <see cref="TreemapView"/> is drawn by the control itself.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// The label portion of the <see cref="TreemapView"/> nodes are drawn manually.
		/// </summary>
		OwnerDrawText = 1,
		/// <summary>
		/// All elements of a <see cref="TreemapView"/> node are drawn manually.
		/// </summary>
		OwnerDrawAll = 2
	}
}
