using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Company.DataGrid.Models
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
		public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

		private IEnumerable<T> enumerable;
		private readonly List<string> propertyPaths;

		public ObservableItemCollection()
		{
			this.propertyPaths = new List<string>();

			this.ThrowExceptionOnInvalidPath = true;
		}

		public bool ThrowExceptionOnInvalidPath
		{
			get; 
			set;
		}

		public void SetEnumerable(IEnumerable<T> newEnumerable)
		{
			if (this.enumerable == newEnumerable)
			{
				return;
			}
			if (this.enumerable is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged) this.enumerable).CollectionChanged -= this.Enumerable_CollectionChanged;
			}
			this.Clear();
			this.enumerable = newEnumerable;
			if (this.enumerable is INotifyCollectionChanged)
			{
				((INotifyCollectionChanged) this.enumerable).CollectionChanged += this.Enumerable_CollectionChanged;
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

		public void AddPropertyPaths(IEnumerable<string> newPropertyPaths)
		{
			this.TraversePropertyPaths(newPropertyPaths, true);
			this.propertyPaths.AddRange(newPropertyPaths);
		}

		public void RemovePropertyPaths(IEnumerable<string> newPropertyPaths)
		{
			this.TraversePropertyPaths(newPropertyPaths, false);
			newPropertyPaths.Select(propertyPath => this.propertyPaths.Remove(propertyPath));
		}

		public void ReplacePropertyPaths(IEnumerable<string> newPropertyPaths, IEnumerable<string> oldPropertyPaths)
		{
			this.TraversePropertyPaths(oldPropertyPaths, false);
			oldPropertyPaths.Select(propertyPath => this.propertyPaths.Remove(propertyPath));
			this.TraversePropertyPaths(newPropertyPaths, true);
			this.propertyPaths.AddRange(newPropertyPaths);
		}

		public void ClearPropertyPaths()
		{
			this.TraversePropertyPaths(this.propertyPaths, false);
			this.propertyPaths.Clear();
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
			this.AddRemoveHandler(item, addHandler);
			foreach (string propertyPath in this.propertyPaths)
			{
				object value = item;
				this.TraversePropertyPath(addHandler, propertyPath, value);
			}
		}

		private void TraversePropertyPaths(IEnumerable changedPropertyPaths, bool addedColumns)
		{
			foreach (string propertyPath in changedPropertyPaths)
			{
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
			ReadOnlyCollection<Property> properties = PropertyPathParser.GetPropertyNames(propertyPath, false);
			for (int index = 0; index < properties.Count - 1; index++)
			{
				Type type = value.GetType();
				PropertyInfo property = type.GetProperty(properties[index].Name);
				if (property == null)
				{
					if (this.ThrowExceptionOnInvalidPath)
					{
						throw new InvalidOperationException(string.Format("The property {0} contained in the property path {1} " +
																		  "does not exist on the type {2}.",
																		  properties[index].Name, propertyPath, type.FullName));	
					}
					return;
				}
				value = property.GetValue(value, properties[index].Arguments);
				if (value == null)
				{
					break;
				}
				this.AddRemoveHandler(value, addHandler);
			}
		}

		private void AddRemoveHandler(object item, bool add)
		{
			if (!(item is INotifyPropertyChanged))
			{
				return;
			}
			if (add)
			{
				((INotifyPropertyChanged) item).PropertyChanged += this.ObservableItemCollection_PropertyChanged;
			}
			else
			{
				((INotifyPropertyChanged) item).PropertyChanged -= this.ObservableItemCollection_PropertyChanged;
			}
		}

		private void ObservableItemCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			EventHandler<ItemPropertyChangedEventArgs> handler = this.ItemPropertyChanged;
			if (handler != null)
			{
				handler(this, new ItemPropertyChangedEventArgs(sender, e.PropertyName));
			}
		}
	}
}
