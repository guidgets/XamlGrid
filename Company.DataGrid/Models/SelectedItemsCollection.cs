using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace Company.DataGrid.Models
{
	/// <summary>
	/// Represents a <see cref="ObservableCollection{T}"/> of items that are selected.
	/// </summary>
	public class SelectedItemsCollection : ObservableCollection<object>
	{
		private SelectionMode selectionMode;

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
			this.SelectionMode = selectionMode;
		}

		/// <summary>
		/// Gets or sets the selection mode used by the <see cref="SelectedItemsCollection"/> when it manages its items.
		/// </summary>
		/// <value>The selection mode.</value>
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
		protected override void InsertItem(int index, object item)
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
	}
}
