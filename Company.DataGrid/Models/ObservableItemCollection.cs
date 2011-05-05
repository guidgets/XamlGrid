using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Company.Widgets.Models
{
	/// <summary>
	/// Represents an <see cref="ObservableCollection{T}"/> which fires an event when a 
	/// contained item implementing <see cref="INotifyPropertyChanged"/> has a property changed.
	/// </summary>
	/// <typeparam name="T">The type of the items contained in the collection.</typeparam>
	public class ObservableItemCollection<T> : ObservableCollection<T>
	{
		/// <summary>
		/// Occurs when a contained item implementing <see cref="INotifyPropertyChanged"/> has a property changed.
		/// </summary>
		public virtual event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;


		private IEnumerable<T> enumerable;
		private readonly Dictionary<string, List<object>> propertyPathsItems;
		private bool throwExceptionOnInvalidPath;


		/// <summary>
		/// Represents an <see cref="ObservableCollection{T}"/> which fires an event when a 
		/// contained item implementing <see cref="INotifyPropertyChanged"/> has a property changed.
		/// </summary>
		public ObservableItemCollection()
		{
			this.propertyPathsItems = new Dictionary<string, List<object>>();

			this.throwExceptionOnInvalidPath = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ObservableItemCollection{T}"/> 
		/// should throw an exception if it encounters an invalid property path.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="ObservableItemCollection{T}"/> should throw an exception 
		/// if it encounters an invalid property path; otherwise, <c>false</c>.
		/// </value>
		public virtual bool ThrowExceptionOnInvalidPath
		{
			get
			{
				return this.throwExceptionOnInvalidPath;
			}
			set
			{
				this.throwExceptionOnInvalidPath = value;
			}
		}

		public virtual void SetSource(IEnumerable<T> newEnumerable)
		{
			if (this.enumerable == newEnumerable)
			{
				return;
			}
			INotifyCollectionChanged oldNotifyCollectionChanged = this.enumerable as INotifyCollectionChanged;
			if (oldNotifyCollectionChanged != null)
			{
				oldNotifyCollectionChanged.CollectionChanged -= this.Enumerable_CollectionChanged;
			}
			this.Clear();
			this.enumerable = newEnumerable;
			INotifyCollectionChanged newNotifyCollectionChanged = this.enumerable as INotifyCollectionChanged;
			if (newNotifyCollectionChanged != null)
			{
				newNotifyCollectionChanged.CollectionChanged += this.Enumerable_CollectionChanged;
			}
			if (this.enumerable != null)
			{
				foreach (T item in this.enumerable)
				{
					this.Add(item);
				}
			}
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected override void InsertItem(int index, T item)
		{
			base.InsertItem(index, item);
			this.ManagePropertyChangedHandler(item, true);
		}

		/// <summary>
		/// Replaces the element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index.</param>
		protected override void SetItem(int index, T item)
		{
			this.ManagePropertyChangedHandler(this[index], false);
			base.SetItem(index, item);
			this.ManagePropertyChangedHandler(item, true);
		}

		/// <summary>
		/// Removes the item at the specified index of the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		protected override void RemoveItem(int index)
		{
			this.ManagePropertyChangedHandler(this[index], false);
			base.RemoveItem(index);
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (T item in this)
			{
				this.ManagePropertyChangedHandler(item, false);
			}
			base.ClearItems();
		}

		/// <summary>
		/// Adds the specified property paths, at which the items in 
		/// this <see cref="ObservableItemCollection{T}"/> will be listened to for changes.
		/// </summary>
		/// <param name="newPropertyPaths">The property paths, at which the items in 
		/// this <see cref="ObservableItemCollection{T}"/> will be listened to for changes.</param>
		public virtual void AddPropertyPaths(IEnumerable<string> newPropertyPaths)
		{
			this.TraversePropertyPaths(newPropertyPaths, true);
		}

		/// <summary>
		/// Removes the specified property paths, at which the items in
		/// this <see cref="ObservableItemCollection{T}"/> will no longer be listened to for changes.
		/// </summary>
		/// <param name="oldPropertyPaths">The property paths, at which the items in 
		/// this <see cref="ObservableItemCollection{T}"/> will no longer be listened to for changes.</param>
		public virtual void RemovePropertyPaths(IEnumerable<string> oldPropertyPaths)
		{
			this.TraversePropertyPaths(oldPropertyPaths, false);
		}

		public virtual void ReplacePropertyPaths(IEnumerable<string> newPropertyPaths, IEnumerable<string> oldPropertyPaths)
		{
			this.TraversePropertyPaths(oldPropertyPaths, false);
			this.TraversePropertyPaths(newPropertyPaths, true);
		}

		/// <summary>
		/// Clears all property paths at which the items in
		/// this <see cref="ObservableItemCollection{T}"/> are listened to for changes.
		/// </summary>
		public virtual void ClearPropertyPaths()
		{
			this.TraversePropertyPaths(new List<string>(this.propertyPathsItems.Keys), false);
		}

		private void Enumerable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (T item in e.NewItems)
					{
						this.Add(item);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (T item in e.OldItems)
					{
						this.Remove(item);
					}
					break;
				case NotifyCollectionChangedAction.Replace:
					this[e.OldStartingIndex] = (T) e.NewItems[0];
					break;
				case NotifyCollectionChangedAction.Reset:
					this.Clear();
					break;
			}
		}

		private void ManagePropertyChangedHandler(T item, bool addHandler)
		{
			this.AddRemoveHandler(item, string.Empty, addHandler);
			foreach (string propertyPath in new List<string>(this.propertyPathsItems.Keys))
			{
				object value = item;
				this.TraversePropertyPath(addHandler, propertyPath, value);
			}
		}

		private void TraversePropertyPaths(IEnumerable changedPropertyPaths, bool addedColumns)
		{
			foreach (string propertyPath in changedPropertyPaths)
			{
				if (addedColumns)
				{
					if (this.propertyPathsItems.ContainsKey(propertyPath))
					{
						continue;
					}
					this.propertyPathsItems.Add(propertyPath, new List<object>());
				}
				else
				{
					this.propertyPathsItems.Remove(propertyPath);
				}
				foreach (T item in this)
				{
					this.TraversePropertyPath(addedColumns, propertyPath, item);
				}
			}
		}

		private void TraversePropertyPath(bool addHandler, string propertyPath, object value)
		{
			if (value == null)
			{
				return;
			}
			PropertyPathWalker propertyPathWalker = new PropertyPathWalker(propertyPath);
			propertyPathWalker.Update(value);
			if (propertyPathWalker.IsPathBroken)
			{
				if (this.ThrowExceptionOnInvalidPath)
				{
					throw new ArgumentException(string.Format("The property path {0} is broken", propertyPath));
				}
				return;
			}

			IPropertyPathNode currentNode = propertyPathWalker.Node;
			while (currentNode != null)
			{
				value = currentNode.Value;
				if (value == null)
				{
					break;
				}
				this.AddRemoveHandler(value, propertyPath, addHandler);
				currentNode = currentNode.Next;
			}
			propertyPathWalker.Update(null);
		}

		private void AddRemoveHandler(object item, string propertyPath, bool add)
		{
			INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
			if (notifyPropertyChanged == null)
			{
				return;
			}
			if (add)
			{
				if (!string.IsNullOrEmpty(propertyPath))
				{
					List<object> items = this.propertyPathsItems[propertyPath];
					if (!items.Contains(item))
					{
						items.Add(item);
					}
				}
				notifyPropertyChanged.PropertyChanged += this.ObservableItemCollection_PropertyChanged;
			}
			else
			{
				this.propertyPathsItems.Remove(propertyPath);
				notifyPropertyChanged.PropertyChanged -= this.ObservableItemCollection_PropertyChanged;
			}
		}

		private void ObservableItemCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			EventHandler<ItemPropertyChangedEventArgs> handler = this.ItemPropertyChanged;
			if (handler != null)
			{
				string propertyPath = (from propertyPathsItem in this.propertyPathsItems
				                       where propertyPathsItem.Value.Contains(sender)
				                       select propertyPathsItem.Key).FirstOrDefault();
				handler(this, new ItemPropertyChangedEventArgs(sender, propertyPath ?? e.PropertyName, e.PropertyName));
			}
		}
	}
}
