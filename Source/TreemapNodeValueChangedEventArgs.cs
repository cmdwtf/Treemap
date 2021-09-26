using TreemapNodeValueDataType = System.Single;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Arguments provided for a <see cref="TreemapNode.Value"/> being changed.
	/// </summary>
	public class TreemapNodeValueChangedEventArgs : TreemapNodeEventArgs
	{
		/// <summary>
		/// The new value of the node.
		/// </summary>
		public TreemapNodeValueDataType Value { get; init; }


		/// <summary>
		/// Creates a new instance of the <see cref="TreemapNodeEventArgs"/> type.
		/// </summary>
		/// <param name="node">The node the event occured on.</param>
		/// <param name="value">The new value.</param>
		public TreemapNodeValueChangedEventArgs(TreemapNode node, TreemapNodeValueDataType value) : base(node)
		{

			Value = value;
		}

	}
}
