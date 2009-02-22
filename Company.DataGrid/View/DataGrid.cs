using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Represents a control for displaying and manipulating data with a default tabular view.
	/// </summary>
	public class DataGrid : ItemsControl
	{
		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);

			// TODO: carefully review this line: it may be more appropriate to initialize the columns elsewhere
			// TODO: or use another collection
			this.Columns = new ObservableCollection<Column>();
		}

		public ObservableCollection<Column> Columns
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates or identifies the element used to display a specified item.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Windows.Controls.ListBoxItem"/> corresponding to a specified item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			// TODO: think carefully about this (what the container for the data items should be)
			return new RowPresenter();
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own item container.
		/// </summary>
		/// <param name="item">The specified item.</param>
		/// <returns>
		/// true if the item is its own item container; otherwise, false.
		/// </returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is RowPresenter;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			// TODO: this isn't quite correct (even with a type check); needs more thinking
			((RowPresenter) element).Columns = this.Columns;
		}
	}
}