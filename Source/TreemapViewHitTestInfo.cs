
using System.Drawing;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A record representing a <see cref="TreemapView"/> hit test result.
	/// </summary>
	public record TreemapViewHitTestInfo
	{
		/// <summary>
		/// The node hit, if any.
		/// </summary>
		public TreemapNode? Node { get; init; }

		/// <summary>
		/// The location of the <see cref="TreemapNode"/> hit.
		/// </summary>
		public TreemapViewHitTestLocations Location { get; init; }

		/// <summary>
		/// The client x coordinate tested.
		/// </summary>
		public int X => TestPoint.X;

		/// <summary>
		/// The client y coordinate tested.
		/// </summary>
		public int Y => TestPoint.Y;

		/// <summary>
		/// The client point tested.
		/// </summary>
		public Point TestPoint { get; init; }

		/// <summary>
		/// Gets a value that returns true if this <see cref="TreemapViewHitTestInfo"/>
		/// didn't hit anything.
		/// </summary>
		public bool IsMiss => Node is null && Location == TreemapViewHitTestLocations.None;

		/// <summary>
		/// Creates a new instance of the <see cref="TreemapViewHitTestInfo"/> record.
		/// </summary>
		/// <param name="hitNode">The node hit, if any.</param>
		/// <param name="hitLocation">The location hit.</param>
		/// <param name="testPoint">The client location tested.</param>
		public TreemapViewHitTestInfo(TreemapNode? hitNode, TreemapViewHitTestLocations hitLocation, Point? testPoint = null)
		{
			Node = hitNode;
			Location = hitLocation;
			TestPoint = testPoint ?? Point.Empty;
		}

		/// <summary>
		/// A static <see cref="TreemapViewHitTestInfo"/> that represents a missed hit test.
		/// </summary>
		public static TreemapViewHitTestInfo Miss { get; } = new(null, TreemapViewHitTestLocations.None);
	}
}