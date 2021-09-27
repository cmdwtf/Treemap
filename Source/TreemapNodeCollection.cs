using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace cmdwtf.Treemap
{
	/// <summary>
	/// Represents a collection of <see cref="TreemapNode"/> objects.
	/// </summary>
	//[Editor("System.Windows.Forms.Design.TreeNodeCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public class TreemapNodeCollection : IList, ICollection, IEnumerable, IQueryable<TreemapNode>
	{
		private readonly TreemapNode? _owner = null;
		private readonly List<TreemapNode> _nodes = new();

		/// <summary>
		/// Creats a new instance of a <see cref="TreemapNodeCollection"/>,
		/// that belongs to the specified <see cref="TreemapNode"/>.
		/// </summary>
		/// <param name="owner">The <see cref="TreemapNode"/> this collection belongs to.</param>
		public TreemapNodeCollection(TreemapNode owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets the <see cref="TreemapNode"/> with the specified key from the collection.
		/// </summary>
		/// <param name="key">The name of the <see cref="TreemapNode"/> to retrieve from the collection.</param>
		/// <returns>The <see cref="TreemapNode"/> with the specified key.</returns>
		public virtual TreemapNode this[string key]
		{
			get
			{
				TreemapNode? result = _nodes.FirstOrDefault(node => node.Name == key);

				return result is not null
					? result
					: throw new KeyNotFoundException($"The key {key} was not found in the node collection.");
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="TreemapNode"/> at the specified indexed location
		/// in the collection.
		/// </summary>
		/// <param name="index">The indexed location of the <see cref="TreemapNode"/> in the collection.</param>
		/// <returns>The <see cref="TreemapNode"/> at the specified indexed location in the collection.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// The index node is less than 0 or is greater than the number of <see cref="TreemapNode"/>s in the collection.
		/// </exception>
		public virtual TreemapNode this[int index]
		{
			get => _nodes[index];
			set => _nodes[index] = value;
		}

		/// <summary>
		/// Gets a node indicating whether the collection is read-only.
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets the total number of <see cref="TreemapNode"/> objects in the collection.
		/// </summary>
		[Browsable(false)]
		public int Count => _nodes.Count;

		/// <summary>
		/// Adds a previously created <see cref="TreemapNode"/> to the specified location
		/// in the node collection.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to add to the collection.</param>
		/// <param name="index">
		/// The indexed location within the collection to insert the <see cref="TreemapNode"/>.
		/// If index is &lt; 0 or larger than the number of nodes in the collection,
		/// the node will be appended to the end.
		/// </param>
		/// <returns>
		/// The zero-based index node of the <see cref="TreemapNode"/> added to the
		/// <see cref="TreemapNode"/> collection.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The node is currently assigned to another <see cref="TreemapView"/>
		/// </exception>
		private int AddNode(TreemapNode node, int index)
		{
			if (node is RootTreemapNode)
			{
				throw new ArgumentException($"{nameof(RootTreemapNode)}s cannot be added to a collection.", nameof(node));
			}

			if (node.ParentInternal != null && node.ParentInternal != _owner)
			{
				throw new ArgumentException($"The node is currently assigned to another {nameof(TreemapNodeCollection)}", nameof(node));
			}

			if (index < 0 || index >= _nodes.Count)
			{
				_nodes.Add(node);
				node.Parent = _owner;
				return _nodes.Count - 1;
			}
			else
			{
				_nodes.Insert(index, node);
				node.Parent = _owner;
				return index;
			}
		}

		/// <summary>
		/// Adds a previously created <see cref="TreemapNode"/> to the end of the <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <param name="node">The <see cref="TreemapNode"/> to add to the collection.</param>
		/// <returns>
		/// The zero-based index node of the <see cref="TreemapNode"/> added to the
		/// <see cref="TreemapNode"/> collection.
		/// </returns>
		public virtual int Add(TreemapNode node) => AddNode(node, -1);

		/// <summary>
		/// Adds a new <see cref="TreemapNode"/> with the specified label text to the end of the current
		/// <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <param name="text">The label text displayed by the <see cref="TreemapNode"/></param>
		/// <returns>
		/// TThe <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string text)
		{
			var node = new TreemapNode(text);
			Add(node);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and images, and adds it to
		/// the collection.
		/// </summary>
		/// <param name="key">The name of the <see cref="TreemapNode"/>.</param>
		/// <param name="text">The text to display in the <see cref="TreemapNode"/>.</param>
		/// <param name="imageIndex">The index of the image to display in the <see cref="TreemapNode"/>.</param>
		/// <param name="selectedImageIndex">The index of the image to be displayed in the <see cref="TreemapNode"/> when it is in a selected state.</param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string key, string text, int imageIndex, int selectedImageIndex)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageIndex = imageIndex,
				SelectedImageIndex = selectedImageIndex
			};
			Add(node);
			return node;
		}

		/// <summary>
		/// Creates a new <see cref="TreemapNode"/> with the specified key and text, and adds it to the collection.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string key, string text)
		{
			var node = new TreemapNode(text)
			{
				Name = key
			};
			Add(node);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and image, and adds it to the
		/// collection.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageKey">
		/// The image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string key, string text, string imageKey)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageKey = imageKey
			};
			Add(node);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and image, and adds it to the
		/// collection.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageIndex">
		/// The index of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string key, string text, int imageIndex)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageIndex = imageIndex
			};
			Add(node);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and images, and adds it to
		/// the collection.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageKey">
		/// The key of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="selectedImageKey">
		/// The key of the image to display when the node is in a selected state.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was added to the collection.
		/// </returns>
		public virtual TreemapNode Add(string key, string text, string imageKey, string selectedImageKey)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageKey = imageKey,
				SelectedImageKey = selectedImageKey
			};
			Add(node);
			return node;
		}

		/// <summary>
		/// Adds an array of previously created <see cref="TreemapNode"/>s to the collection.
		/// </summary>
		/// <param name="nodes">
		/// An array of <see cref="TreemapNode"/> objects representing the <see cref="TreemapNode"/>s
		/// to add to the collection.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// nodes is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// nodes is the child of another <see cref="TreemapView"/>.
		/// </exception>
		public virtual void AddRange(TreemapNode[] nodes)
		{
			foreach (TreemapNode node in nodes)
			{
				Add(node);
			}
		}

		/// <summary>
		/// Removes all <see cref="TreemapNode"/>s from the collection.
		/// </summary>
		public virtual void Clear()
		{
			List<TreemapNode> toRemove = _nodes.ToList();
			_nodes.Clear();

			foreach (TreemapNode node in toRemove)
			{
				node.Parent = null;
			}

		}

		/// <summary>
		/// Determines whether the specified <see cref="TreemapNode"/> is a member of the collection.
		/// </summary>
		/// <param name="node">
		/// The <see cref="TreemapNode"/> to locate in the collection.
		/// </param>
		/// <returns>
		/// true if the <see cref="TreemapNode"/> is a member of the collection; otherwise,
		/// false.
		/// </returns>
		public bool Contains(TreemapNode node) => _nodes.Contains(node);

		/// <summary>
		/// Determines whether the collection contains a <see cref="TreemapNode"/> with the specified key.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/> to search for.
		/// </param>
		/// <returns>
		/// true to indicate the collection contains a <see cref="TreemapNode"/> with
		/// the specified key; otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(string key) => this[key] != null;

		/// <summary>
		/// Copies the entire collection into an existing array at a specified location within
		/// the array.
		/// </summary>
		/// <param name="dest">
		/// The destination array.
		/// </param>
		/// <param name="index">
		/// The index in the destination array at which storing begins.
		/// </param>
		[Obsolete("This method transforms the internal list to an array before copying it to dest. There's a better way to copy the array than this function.")]
		public void CopyTo(Array dest, int index)
			=> Array.Copy(_nodes.ToArray(), 0, dest, index, dest.Length);

		/// <summary>
		/// Finds the <see cref="TreemapNode"/>s with specified key, optionally searching subnodes.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/> to search for.
		/// </param>
		/// <param name="searchAllChildren">
		/// true to search child nodes of <see cref="TreemapNode"/>s; otherwise, false.
		/// </param>
		/// <returns>
		/// An array of <see cref="TreemapNode"/> objects whose <see cref="TreemapNode"/>.Name
		/// property matches the specified key
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// .NET 5.0 and later: key is null or empty.
		/// </exception>
		public TreemapNode[] Find(string key, bool searchAllChildren)
		{
			var result = new List<TreemapNode>();
			result.AddRange(_nodes.Where(child => child.Name == key));

			if (searchAllChildren == false)
			{
				return result.ToArray();
			}

			foreach (TreemapNode child in _nodes)
			{
				result.AddRange(child.Nodes.Find(key, searchAllChildren));
			}

			return result.ToArray();
		}

		/// <summary>
		/// Returns an enumerator that can be used to iterate through the <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <returns>
		/// An System.Collections.IEnumerator that represents the <see cref="TreemapNode"/> collection.
		/// </returns>
		public IEnumerator GetEnumerator() => _nodes.GetEnumerator();

		/// <summary>
		/// Returns the index of the specified <see cref="TreemapNode"/> in the collection.
		/// </summary>
		/// <param name="node">
		/// The <see cref="TreemapNode"/> to locate in the collection.
		/// </param>
		/// <returns>
		/// The zero-based index of the item found in the <see cref="TreemapNode"/> collection; otherwise,
		/// -1.
		/// </returns>
		public int IndexOf(TreemapNode node) => _nodes.IndexOf(node);

		/// <summary>
		/// Returns the index of the first occurrence of a <see cref="TreemapNode"/> with the specified key.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/> to search for.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of a <see cref="TreemapNode"/> with the specified
		/// key, if found; otherwise, -1.
		/// </returns>
		public virtual int IndexOfKey(string key)
			=> _nodes.Where(node => node.Name == key).FirstOrDefault()?.Index ?? -1;

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and image, and inserts it into
		/// the collection at the specified index.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageKey">
		/// The key of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string key, string text, string imageKey)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageKey = imageKey
			};
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and images, and inserts it
		/// into the collection at the specified index.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageIndex">
		/// The index of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="selectedImageIndex">
		/// The index of the image to display in the <see cref="TreemapNode"/> when it is in a selected state.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageIndex = imageIndex,
				SelectedImageIndex = selectedImageIndex
			};
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and image, and inserts it into
		/// the collection at the specified index.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageIndex">
		/// The index of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string key, string text, int imageIndex)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageIndex = imageIndex
			};
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified text and key, and inserts it into the
		/// collection.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string key, string text)
		{
			var node = new TreemapNode(text)
			{
				Name = key
			};
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Inserts an existing <see cref="TreemapNode"/> into the <see cref="TreemapNode"/> collection at the specified
		/// location.
		/// </summary>
		/// <param name="index">
		/// The indexed location within the collection to insert the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="node">
		/// The <see cref="TreemapNode"/> to insert into the collection.
		/// </param>
		/// <exception cref="ArgumentException">
		/// The node is currently assigned to another <see cref="TreemapView"/>.
		/// </exception>
		public virtual void Insert(int index, TreemapNode node)
		{
			if (node.ParentInternal != null)
			{
				throw new ArgumentException($"The given {nameof(TreemapNode)} already belongs to another collection. Remove it from that collection before adding it to a new one.", nameof(node));
			}

			AddNode(node, index);
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified key, text, and images, and inserts it
		/// into the collection at the specified index.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="imageKey">
		/// The key of the image to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <param name="selectedImageKey">
		/// The key of the image to display in the <see cref="TreemapNode"/> when it is in a selected state.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
		{
			var node = new TreemapNode(text)
			{
				Name = key,
				ImageKey = imageKey,
				SelectedImageKey = selectedImageKey
			};
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Creates a <see cref="TreemapNode"/> with the specified text and inserts it at the specified index.
		/// </summary>
		/// <param name="index">
		/// The location within the collection to insert the node.
		/// </param>
		/// <param name="text">
		/// The text to display in the <see cref="TreemapNode"/>.
		/// </param>
		/// <returns>
		/// The <see cref="TreemapNode"/> that was inserted in the collection.
		/// </returns>
		public virtual TreemapNode Insert(int index, string text)
		{
			var node = new TreemapNode(text);
			AddNode(node, index);
			return node;
		}

		/// <summary>
		/// Removes the specified <see cref="TreemapNode"/> from the <see cref="TreemapNode"/> collection.
		/// </summary>
		/// <param name="node">
		/// The <see cref="TreemapNode"/> to remove.
		/// </param>
		public void Remove(TreemapNode node)
		{
			if (node.ParentInternal == _owner && _nodes.Remove(node))
			{
				node.Parent = null;
			}
		}

		/// <summary>
		/// Removes a <see cref="TreemapNode"/> from the <see cref="TreemapNode"/> collection at a specified index.
		/// </summary>
		/// <param name="index">
		/// The index of the <see cref="TreemapNode"/> to remove.
		/// </param>
		public virtual void RemoveAt(int index)
		{
			TreemapNode node = _nodes[index];
			_nodes.RemoveAt(index);
			node.Parent = null;
		}

		/// <summary>
		/// Removes the <see cref="TreemapNode"/> with the specified key from the collection.
		/// </summary>
		/// <param name="key">
		/// The name of the <see cref="TreemapNode"/> to remove from the collection.
		/// </param>
		public virtual void RemoveByKey(string key)
		{
			TreemapNode node = this[key];
			if (node != null && node.ParentInternal == _owner && _nodes.Remove(node))
			{
				node.Parent = null;
			}
		}

		/// <summary>
		/// Sorts the <see cref="TreemapNodeCollection"/> via the given <see cref="IComparer{T}"/>.
		/// </summary>
		/// <param name="treemapViewNodeSorter">The sorter to use.</param>
		/// <param name="sortAllChildren">If true, will sort children nodes as well.</param>
		internal void Sort(IComparer<TreemapNode>? treemapViewNodeSorter, bool sortAllChildren = true)
		{
			// sort our nodes,
			if (treemapViewNodeSorter is not null)
			{
				_nodes.Sort(treemapViewNodeSorter);
			}
			else
			{
				_nodes.Sort();
			}

			// and our children, if they said to.
			if (sortAllChildren)
			{
				foreach (TreemapNode child in _nodes)
				{
					child.Nodes.Sort(treemapViewNodeSorter, sortAllChildren);
				}
			}
		}


		#region Explicit Interface Implementations

		#region IList

		int IList.Add(object? node)
			=> Add(node as TreemapNode ?? throw new InvalidOperationException());
		bool IList.Contains(object? node)
			=> Contains(node as TreemapNode ?? throw new InvalidOperationException());
		int IList.IndexOf(object? node)
			=> IndexOf(node as TreemapNode ?? throw new InvalidOperationException());
		void IList.Insert(int index, object? node)
			=> Insert(index, node as TreemapNode ?? throw new InvalidOperationException());
		void IList.Remove(object? node)
			=> Remove(node as TreemapNode ?? throw new InvalidOperationException());
		object? IList.this[int index]
		{
			get => this[index];
			set => this[index] = value as TreemapNode ?? throw new InvalidOperationException();
		}
		bool IList.IsFixedSize
			=> false;

		#endregion IList

		#region ICollection

		bool ICollection.IsSynchronized
			=> false;
		object ICollection.SyncRoot { get; } = new object();

		#endregion ICollection

		#region IQueryable

		Type IQueryable.ElementType
			=> _nodes.AsQueryable().ElementType;
		Expression IQueryable.Expression
			=> _nodes.AsQueryable().Expression;
		IQueryProvider IQueryable.Provider
			=> _nodes.AsQueryable().Provider;

		#endregion IQueryable

		#region IEnumerable<TreemapNode>

		IEnumerator<TreemapNode> IEnumerable<TreemapNode>.GetEnumerator() => _nodes.GetEnumerator();

		#endregion IEnumerable<TreemapNode>

		#endregion Explicit Interface Implementations
	}
}
