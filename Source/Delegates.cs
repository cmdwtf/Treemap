
namespace cmdwtf.Treemap
{
	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.BeforeLabelEdit"/>
	/// and <see cref="TreemapView.AfterLabelEdit"/> events of a <see cref="TreemapView"/>
	/// control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e"><see cref="TreemapNodeLabelEditEventArgs"/> that contains the event data.</param>
	public delegate void TreemapNodeLabelEditEventHandler(object sender, TreemapNodeLabelEditEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.BeforeCheck"/>,
	/// <see cref="TreemapView.BeforeCollapse"/>, <see cref="TreemapView.BeforeExpand"/>,
	/// or <see cref="TreemapView.BeforeSelect"/> event of a <see cref="TreemapView"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapViewCancelEventArgs"/> that contains the event data.</param>
	public delegate void TreemapViewCancelEventHandler(object sender, TreemapViewCancelEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.AfterCheck"/>,
	/// <see cref="TreemapView.AfterCollapse"/>, <see cref="TreemapView.AfterExpand"/>,
	/// or <see cref="TreemapView.AfterSelect"/> event of a <see cref="TreemapView"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapViewEventArgs"/> that contains the event data.</param>
	public delegate void TreemapViewEventHandler(object sender, TreemapViewEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.DrawNode"/>
	/// and <see cref="TreemapView.DrawBranchNode"/> event of a <see cref="TreemapView"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="DrawTreemapNodeEventArgs"/> that contains the event data.</param>
	public delegate void DrawTreemapNodeEventHandler(object sender, DrawTreemapNodeEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.NodeMouseHover"/>
	/// event of a <see cref="TreemapView"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapNodeMouseHoverEventArgs"/> that contains the event data.</param>
	public delegate void TreemapNodeMouseHoverEventHandler(object sender, TreemapNodeMouseHoverEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapView.NodeMouseClick"/>
	/// and <see cref="TreemapView.NodeMouseDoubleClick"/> events of a <see cref="TreemapView"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapNodeMouseClickEventArgs"/> that contains the event data.</param>
	public delegate void TreemapNodeMouseClickEventHandler(object sender, TreemapNodeMouseClickEventArgs e);

	/// <summary>
	/// Represents the method that will handle the <see cref="TreemapNode.ValueChanged"/>
	/// events of a <see cref="TreemapNode"/>.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapNodeValueChangedEventArgs"/> that contains the event data.</param>
	public delegate void TreemapNodeValueChangedEventHandler(object sender, TreemapNodeValueChangedEventArgs e);

	/// <summary>
	/// Represents the method that will handle generic <see cref="TreemapNode"/> events.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">A <see cref="TreemapNodeEventArgs"/> that contains the event data.</param>
	public delegate void TreemapNodeEventHandler(object sender, TreemapNodeEventArgs e);
}
