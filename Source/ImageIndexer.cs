using System.Drawing;
using System.Windows.Forms;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// A helper class for juggling <see cref="ImageList"/> indexes.
	/// </summary>
	/// <remarks>
	/// Licensed under the MIT license.
	/// Copyright (c) .NET Foundation and Contributors
	/// <see href="https://github.com/dotnet/winforms/blob/58d96850c5c2d76c2789ea51fa7994514051937a/src/System.Windows.Forms/src/System/Windows/Forms/ImageList.Indexer.cs">ImageList.Indexer.cs</see>
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Uniformity with WinForms API")]
	internal class ImageIndexer
	{
		// Used by TreeViewImageIndexConverter to show "(none)"
		internal const int NoneIndex = -2;
		internal const string NoneKey = "(none)";

		// Used by generally across the board to indicate unset image
		internal const string DefaultKey = "";
		internal const int DefaultIndex = -1;

		private string _key = DefaultKey;
		private int _index = DefaultIndex;
		private bool _useIntegerIndex = true;

		public virtual ImageList? ImageList { get; set; }

		public virtual Image? Image
			=> ActualIndex >= 0
			? ImageList?.Images[ActualIndex]
			: null;

		public virtual string Key
		{
			get => _key;
			set
			{
				_index = DefaultIndex;
				_key = value ?? DefaultKey;
				_useIntegerIndex = false;
			}
		}

		public virtual int Index
		{
			get => _index;
			set
			{
				_key = DefaultKey;
				_index = value;
				_useIntegerIndex = true;
			}
		}

		public virtual int ActualIndex
		{
			get
			{
				if (_useIntegerIndex)
				{
					return Index;
				}

				if (ImageList != null)
				{
					return ImageList.Images.IndexOfKey(Key);
				}

				return DefaultIndex;
			}
		}
	}
}
