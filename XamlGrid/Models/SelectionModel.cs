// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	SelectionModel.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using XamlGrid.Core;

namespace XamlGrid.Models
{
	/// <summary>
	/// Represents a <see cref="Model"/> that executes the appropriate selection logic requested by different commands.
	/// </summary>
	public class SelectionModel : Model
	{
		public new static readonly string NAME = typeof(SelectionModel).Name;

		private IList<object> items;
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
		public virtual SelectedItemsCollection SelectedItems
		{
			get;
			private set;
		}

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
					this.SelectedItems.SelectionMode = this.selectionMode;
					this.SendNotification(Notifications.SelectionModeChanged, this.selectionMode);
				}
			}
		}

		/// <summary>
		/// Gets or sets the items that the <see cref="SelectionModel"/> selects and deselects.
		/// </summary>
		/// <value>The items to select and deselect.</value>
		public virtual IList<object> Items
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


		/// <summary>
		/// Called by the Model when the Model is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.SelectedItems.CollectionChanged += this.SelectedItems_CollectionChanged;
			((INotifyPropertyChanged) this.SelectedItems).PropertyChanged += this.SelectionModel_PropertyChanged;
		}

		/// <summary>
		/// Called by the Model when the Model is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.SelectedItems.CollectionChanged -= this.SelectedItems_CollectionChanged;
			((INotifyPropertyChanged) this.SelectedItems).PropertyChanged -= this.SelectionModel_PropertyChanged;
		}


		/// <summary>
		/// Selects the specified item.
		/// </summary>
		/// <param name="item">The item to select.</param>
		public virtual void Select(object item)
		{
			int index = this.items.IndexOf(item);
			if (index >= 0)
			{
				this.SelectedItems.Add(new SelectedItem(item, index));				
			}
		}

		/// <summary>
		/// Selects the range of the items located between the specified items.
		/// </summary>
		/// <param name="item">The first item of the range.</param>
		/// <param name="clearPreviousSelection">if set to <c>true</c> clear the previous selection.</param>
		public virtual void SelectRange(object item, bool clearPreviousSelection)
		{
			this.SelectRange(this.items.IndexOf(item), clearPreviousSelection);
		}

		public virtual void SelectRange(int endIndex, bool clearPreviousSelection)
		{
			int start = this.ranges.Count > 0 ? this.ranges.Last().Start : 0;
			int end = endIndex;
			if (clearPreviousSelection)
			{
				this.SelectedItems.Clear();
			}
			this.SelectRange(start, end);
		}

		/// <summary>
		/// Selects the range of the items located between the specified indices.
		/// </summary>
		/// <param name="start">The start index.</param>
		/// <param name="end">The end index.</param>
		public virtual void SelectRange(int start, int end)
		{
			int step = start <= end ? 1 : -1;
			this.ranges.AddRange(start, end);
			List<SelectedItem> selectedItems = new List<SelectedItem>();
			for (int index = start; index != end + step; index += step)
			{
				selectedItems.Add(new SelectedItem(this.items[index], index));
			}
			this.SelectedItems.AddRange(selectedItems);
		}

		/// <summary>
		/// Selects all items in the collection managed by the model.
		/// </summary>
		public virtual void SelectAll()
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
					this.ProcessRemoval(e);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.ProcessAddition(e);
					this.ProcessRemoval(e);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.ranges.Clear();
					this.SendNotification(Notifications.DeselectedItems, new List<object>(0));
					break;
			}
		}

		private void ProcessAddition(NotifyCollectionChangedEventArgs e)
		{
			List<object> selectedRanges = new List<object>();
			List<object> selectedItems = new List<object>();
			foreach (object newItem in e.NewItems)
			{
				IEnumerable<SelectedItem> enumerable = newItem as IEnumerable<SelectedItem>;
				if (enumerable != null)
				{
					selectedRanges.AddRange(enumerable.Select(selectedItem => selectedItem.Item));
				}
				else
				{
					SelectedItem selectedItem = (SelectedItem) newItem;
					selectedItems.Add(selectedItem.Item);
					int index = this.items.IndexOf(selectedItem.Item);
					this.ranges.AddRange(index, index);
				}
			}
			if (selectedRanges.Count > 0)
			{
				int index = this.items.IndexOf(selectedRanges[0]);
				this.ranges.AddRange(index, index + selectedRanges.Count);
			}
			this.SendNotification(Notifications.SelectedItems, selectedRanges.Count > 0 ? selectedRanges : selectedItems);
		}

		private void ProcessRemoval(NotifyCollectionChangedEventArgs e)
		{
			List<object> deselectedItems = new List<object>();
			foreach (SelectedItem selectedItem in e.OldItems)
			{
				deselectedItems.Add(selectedItem.Item);
				int index = this.items.IndexOf(selectedItem.Item);
				this.ranges.RemoveRange(index, index);
			}
			this.SendNotification(Notifications.DeselectedItems, deselectedItems);
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
