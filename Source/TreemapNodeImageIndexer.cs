using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using static cmdwtf.Treemap.TreemapNodeImageIndexer;

namespace cmdwtf.Treemap
{
	public partial class TreemapNode
	{
		/// <summary>
		/// A helper class for juggling <see cref="ImageList"/> indexes.
		/// </summary>
		/// <remarks>
		/// Licensed under the MIT license.
		/// Copyright (c) .NET Foundation and Contributors
		/// <see href="https://github.com/dotnet/winforms/blob/58d96850c5c2d76c2789ea51fa7994514051937a/src/System.Windows.Forms/src/System/Windows/Forms/ImageList.Indexer.cs">ImageList.Indexer.cs</see>
		/// <see href="https://github.com/dotnet/winforms/blob/c18b171e51714d9de8d15e6e52d48a45c33c2d4c/src/System.Windows.Forms/src/System/Windows/Forms/TreeNode.TreeNodeImageIndexer.cs">TreeNode.TreeNodeImageIndexer.cs</see>
		/// </remarks>
		internal partial class TreemapNodeImageIndexer : ImageIndexer
		{
			private readonly TreemapNode _owner;
			private readonly ImageListType _imageListType;

			public override ImageList? ImageList
			{
				get
				{
					if (_owner.TreemapView is not null)
					{
						if (_imageListType == ImageListType.State)
						{
							return _owner.TreemapView.StateImageList;
						}
						else
						{
							return _owner.TreemapView.ImageList;
						}
					}
					else
					{
						return null;
					}
				}
				set { Debug.Assert(false, "We should never set the image list"); }
			}

			public Image? GetImage(ImageIndexer fallback)
			{
				if (Image is not null)
				{
					return Image;
				}

				return fallback.Image;
			}

			public TreemapNodeImageIndexer(TreemapNode node, ImageListType imageListType)
			{
				_owner = node;
				_imageListType = imageListType;
			}
		}
	}
}
