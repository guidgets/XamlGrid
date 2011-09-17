using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using XamlGrid.Aspects;

namespace XamlGrid.Models
{
	/// <summary>
	/// Represents a <see cref="ObservableCollection{T}"/> of items that are selected.
	/// </summary>
	public class SelectedItemsCollection : ObservableCollection<SelectedItem>
	{
		private SelectionMode selectionMode;
		private bool suspendNotifications;


		/// <summary>
		/// Represents a <see cref="ObservableCollection{T}"/> of items that are selected.
		/// </summary>
		public SelectedItemsCollection() : this(SelectionMode.Extended)
		{

		}

		/// <summary>
		/// Represents a <see cref="ObservableCollection{T}"/> of items that are selected.
		/// </summary>
		/// <param name="selectionMode">The selection mode of the <see cref="SelectedItemsCollection"/>.</param>
		public SelectedItemsCollection(SelectionMode selectionMode)
		{
			this.selectionMode = selectionMode;
		}


		/// <summary>
		/// Gets or sets the selection mode used by the <see cref="SelectedItemsCollection"/> when it manages its items.
		/// </summary>
		/// <value>The selection mode.</value>
		public virtual SelectionMode SelectionMode
		{
			get
			{
				return this.selectionMode;
			}
			set
			{
				if (this.selectionMode != value)
				{
					this.selectionMode = value;
					if (this.selectionMode == SelectionMode.Single)
					{
						for (int index = this.Count - 2; index >= 0; index--)
						{
							this.RemoveAt(index);
						}
					}
					this.OnPropertyChanged(new PropertyChangedEventArgs("SelectionMode"));
				}
			}
		}


		/// <summary>
		/// Inserts an item into the collection at the specified index; 
		/// if the <see cref="SelectionMode"/> of the <see cref="SelectedItemsCollection"/> is <see cref="System.Windows.Controls.SelectionMode.Single"/>, 
		/// all items except the specified object parameter are cleared.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected override void InsertItem(int index, SelectedItem item)
		{
			if (!this.Contains(item))
			{
				if (this.SelectionMode == SelectionMode.Single)
				{
					this.Clear();
					base.InsertItem(0, item);
				}
				else
				{
					base.InsertItem(index, item);
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="ObservableCollection{T}.CollectionChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (!suspendNotifications)
			{
				base.OnCollectionChanged(e);				
			}
		}


		/// <summary>
		/// Adds the elements of the specified range to the end of this <see cref="SelectedItemsCollection"/>.
		/// </summary>
		/// <param name="range">The range which elements should be added to the end of this <see cref="SelectedItemsCollection"/>.</param>
		[Validate]
		public virtual void AddRange([NotNull] IEnumerable<SelectedItem> range)
		{
			List<SelectedItem> rangeList = range.ToList();
			this.suspendNotifications = true;
			int firstIndex = -1;
			foreach (SelectedItem selectedItem in rangeList)
			{
				if (firstIndex < 0)
				{
					firstIndex = this.Count;
				}
				this.Add(selectedItem);
			}
			if (firstIndex < 0)
			{
				return;
			}
			this.suspendNotifications = false;
			const NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add;
			List<SelectedItem> selection = new List<SelectedItem>(rangeList);
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, selection, firstIndex));
		}

		/// <summary>
		/// Determines whether the specified item is selected.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if the specified item is selected; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSelected(object item)
		{
			return this.Any(selectedItem => selectedItem.Item == item);
		}

		/// <summary>
		/// Deselects the specified item.
		/// </summary>
		/// <param name="item">The item to deselect.</param>
		/// <returns></returns>
		public bool Deselect(object item)
		{
			IList<SelectedItem> existingItems = this.Where(selectedItem => selectedItem.Item == item).ToList();
			if (existingItems.Any())
			{
				for (int index = existingItems.Count - 1; index >= 0; index--)
				{
					this.Remove(existingItems[index]);
				}
				return true;
			}
			return false;
		}
	}
}
