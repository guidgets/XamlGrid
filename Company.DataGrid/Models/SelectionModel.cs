using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Company.DataGrid.Core;

namespace Company.DataGrid.Models
{
	/// <summary>
	/// Represents a <see cref="Model"/> that executes the appropriate selection logic requested by different commands.
	/// </summary>
	public class SelectionModel : Model
	{
		public new const string NAME = "selectionModel";

		private IList items;
		private SelectionMode selectionMode;
		private readonly RangeCollection ranges;

		/// <summary>
		/// Represents a <see cref="Model"/> that executes the appropriate selection logic requested by different commands.
		/// </summary>
		public SelectionModel() : base (NAME)
		{
			this.items = new List<object>(0);
			this.SelectedItems = new SelectedItemsCollection();
			this.selectionMode = SelectionMode.Extended;
			this.ranges = new RangeCollection();
		}

		/// <summary>
		/// Gets the selected items managed by the <see cref="SelectionModel"/>.
		/// </summary>
		/// <value>The selected items managed by the <see cref="SelectionModel"/>.</value>
		public SelectedItemsCollection SelectedItems
		{
			get;
			private set;
		}

		public SelectionMode SelectionMode
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
					this.SelectedItems.SelectionMode = this.selectionMode;
					this.SendNotification(Notifications.SELECTION_MODE_CHANGED, this.selectionMode);
				}
			}
		}

		/// <summary>
		/// Gets or sets the items that the <see cref="SelectionModel"/> selects and deselects.
		/// </summary>
		/// <value>The items to select and deselect.</value>
		public IList Items
		{
			get
			{
			    return this.items;
			}
			set
			{
				if (this.items != value)
				{
					this.items = value;
					this.SelectedItems.Clear();
				}
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();

			this.SelectedItems.CollectionChanged += this.SelectedItems_CollectionChanged;
			((INotifyPropertyChanged) this.SelectedItems).PropertyChanged += this.SelectionModel_PropertyChanged;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.SelectedItems.CollectionChanged -= this.SelectedItems_CollectionChanged;
			((INotifyPropertyChanged) this.SelectedItems).PropertyChanged -= this.SelectionModel_PropertyChanged;
		}

		/// <summary>
		/// Selects the range of the items located between the specified items.
		/// </summary>
		/// <param name="item">The first item of the range.</param>
		/// <param name="clearPreviousSelection">if set to <c>true</c> clear the previous selection.</param>
		public void SelectRange(object item, bool clearPreviousSelection)
		{
			this.SelectRange(this.items.IndexOf(item), clearPreviousSelection);
		}

		/// <summary>
		/// Selects the range of the items located between the specified indices.
		/// </summary>
		/// <param name="endIndex">The first index of the range.</param>
		/// <param name="clearPreviousSelection">if set to <c>true</c> clear the previous selection.</param>
		public void SelectRange(int endIndex, bool clearPreviousSelection)
		{
			int start = this.ranges.Count > 0 ? this.ranges.Last().Start : 0;
			int end = endIndex;
			if (clearPreviousSelection)
			{
				this.SelectedItems.Clear();
			}
			this.SelectRange(start, end);
		}

		public void SelectRange(int start, int end)
		{
			int step = start <= end ? 1 : -1;
			this.ranges.AddRange(start, end);
			List<object> selectedItems = new List<object>();
			for (int index = start; index != end + step; index += step)
			{
				selectedItems.Add(this.items[index]);
			}
			this.SelectedItems.AddRange(selectedItems);
		}

		/// <summary>
		/// Selects all items in the collection managed by the model.
		/// </summary>
		public void SelectAll()
		{
			List<Range> backup = new List<Range>(this.ranges);
			this.SelectRange(0, this.items.Count - 1);
			this.ranges.Clear();
			foreach (Range range in backup)
			{
				this.ranges.Add(range);
			}
		}

		private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					this.ProcessAddition(e);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (int indexOfItem in from object selectedItem in e.OldItems
												select this.items.IndexOf(selectedItem))
					{
						this.ranges.RemoveRange(indexOfItem, indexOfItem);
					}
					this.SendNotification(Notifications.DESELECTED_ITEMS, e.OldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					foreach (int indexOfItem in from object selectedItem in e.NewItems
												select this.items.IndexOf(selectedItem))
					{
						this.ranges.AddRange(indexOfItem, indexOfItem);
					}
					foreach (int indexOfItem in from object selectedItem in e.OldItems
												select this.items.IndexOf(selectedItem))
					{
						this.ranges.RemoveRange(indexOfItem, indexOfItem);
					}
					this.SendNotification(Notifications.DESELECTED_ITEMS, e.OldItems);
					this.SendNotification(Notifications.SELECTED_ITEMS, e.NewItems);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.ranges.Clear();
					this.SendNotification(Notifications.DESELECTED_ITEMS, this.SelectedItems);
					break;
			}
		}

		private void ProcessAddition(NotifyCollectionChangedEventArgs e)
		{
			List<object> selectedItems = new List<object>();
			foreach (object newItem in e.NewItems)
			{
				IEnumerable<object> enumerable = newItem as IEnumerable<object>;
				if (enumerable != null)
				{
					selectedItems.AddRange(enumerable);
				}
				else
				{
					int index = this.items.IndexOf(newItem);
					this.ranges.AddRange(index, index);						
				}
			}
			if (selectedItems.Count > 0)
			{
				int index = this.items.IndexOf(selectedItems[0]);
				this.ranges.AddRange(index, index + selectedItems.Count);
			}
			this.SendNotification(Notifications.SELECTED_ITEMS, selectedItems.Count > 0 ? selectedItems : e.NewItems);
		}

		private void SelectionModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectionMode")
			{
				this.SelectionMode = this.SelectedItems.SelectionMode;
			}
		}
	}
}
