using System.Windows.Forms.VisualStyles;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Some custom <see cref="VisualStyleElement"/>s.
	/// </summary>
	internal class CustomVisualStyleElement
	{
#if !USE_CLASSIC_PLUSMINUS_GLYPHS
		/// <summary>
		/// Thanks to <see href="https://stackoverflow.com/a/3026207">this StackOverflow user</see>
		/// for tracking down how to get the hover glyphs.
		/// </summary>
		public static class ExplorerTreeView
		{
			private const string _className = "Explorer::TreeView";

			public static class Glyph
			{

				private const int _stateOpened = 2;
				private const int _stateClosed = 1;

				public static class Normal
				{
					private const int _part = 2;

					public static VisualStyleElement Closed { get; } =
						VisualStyleElement.CreateElement(
							_className,
							_part,
							_stateOpened);

					public static VisualStyleElement Opened { get; } =
						VisualStyleElement.CreateElement(
							_className,
							_part,
							_stateClosed);
				}

				public static class Hover
				{
					private const int _part = 4;

					public static VisualStyleElement Closed { get; } =
						VisualStyleElement.CreateElement(
							_className,
							_part,
							_stateOpened);

					public static VisualStyleElement Opened { get; } =
						VisualStyleElement.CreateElement(
							_className,
							_part,
							_stateClosed);
				}
			}
		}
#endif // !USE_CLASSIC_PLUSMINUS_GLYPHS
	}
}
