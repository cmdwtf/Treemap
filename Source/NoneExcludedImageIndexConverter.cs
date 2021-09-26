using System.Windows.Forms;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Provides a duplicate for the NoneExcludedImageIndexConverter that is
	/// internal to the System.Windows.Forms library.
	/// </summary>
	internal sealed class NoneExcludedImageIndexConverter : ImageIndexConverter
	{

		/// <summary>
		/// Provides the value false for including 'None' as a standard value.
		/// </summary>
		protected override bool IncludeNoneAsStandardValue
		{
			get
			{
				return false;
			}
		}
	}

}
