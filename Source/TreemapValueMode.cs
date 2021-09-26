
namespace cmdwtf.Treemap
{
	/// <summary>
	/// Describes the ways the <see cref="TreemapView"/> can calculate branch values.
	/// </summary>
	public enum TreemapValueMode
	{
		/// <summary>
		/// The <see cref="TreemapView"/> will calculate it's values based on the
		/// values in the leaves (nodes with no children) of the tree, ignoring
		/// any values on branch nodes. This is the standard behavior.
		/// </summary>
		LeavesOnly = 0,
		/// <summary>
		/// The <see cref="TreemapView"/> will calculate it's values where each node,
		/// adds it's values to that of the children. This mode will allow branch
		/// nodes to have non-header, owned area that is not consumed by child nodes.
		/// </summary>
		LeavesAndBranches
	}
}
