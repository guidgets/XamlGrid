using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Represents a UI element that displays a data object.
	/// </summary>
	public class Row : ItemsControl
	{
		public Row()
		{
			this.DefaultStyleKey = typeof(Row);
		}

		/// <summary>
		/// Creates or identifies the element used to display a specified item.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Windows.Controls.ListBoxItem"/> corresponding to a specified item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Cell();
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
			return item is Cell;
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			// TODO: this may be better; consider it carefully
			Cell cell = (Cell) element;
			Column column = (Column) item;
			cell.Column = column;
			cell.Width = column.ActualWidth;
			cell.DataContext = this.DataContext;
			cell.SetBinding(Cell.ValueProperty, column.DataBinding);
		}
	}
}
