namespace cmdwtf.Treemap
{
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
