using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Company.DataGrid.Models
{
	/// <summary>
	/// Represents a <see cref="ObservableCollection{T}"/> of items that are selected.
	/// </summary>
	public class SelectedItemsCollection : ObservableCollection<object>
	{
		public SelectedItemsCollection() : this(SelectionMode.Single)
		{

		}

		public SelectedItemsCollection(SelectionMode selectionMode)
		{
			this.SelectionMode = selectionMode;
		}

		public SelectionMode SelectionMode
		{
			get; 
			set;
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
