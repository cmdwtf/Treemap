namespace cmdwtf.Treemap
{
	/// <summary>
	/// An enum describing the various styes a <see cref="TreemapNode"/>
	/// can draw itself in.
	/// </summary>
	public enum TreemapNodeDrawStyle
	{
		/// <summary>
		/// A flat, single color.
		/// </summary>
		Flat,

		/// <summary>
		/// A circular color gradient that gives a bit of depth.
		/// </summary>
		Gradient,

		/// <summary>
		/// A horizontal color gradient that gives a bit of depth.
		/// </summary>
		GradientHorizontal,

		/// <summary>
		/// A vertical color gradient that gives a bit of depth.
		/// </summary>
		GradientVertical,
	}
}
