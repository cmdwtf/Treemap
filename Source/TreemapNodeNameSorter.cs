namespace cmdwtf.Treemap
{
	/// <summary>
	/// A <see cref="TreemapNode"/> sorter that sorts by
	/// <see cref="TreemapNode.Name"/>.
	/// </summary>
	public class TreemapNodeNameSorter : TreemapNodeSorter
	{
		/// <summary>
		/// Creates a new instance of the <see cref="TreemapNodeNameSorter"/> class.
		/// </summary>
		public TreemapNodeNameSorter() { }

		/// <inheritdoc/>
		protected override float CompareNodes(TreemapNode? lhs, TreemapNode? rhs)
			=> lhs?.Name.CompareTo(rhs?.Name) ?? 0;
	}
}
