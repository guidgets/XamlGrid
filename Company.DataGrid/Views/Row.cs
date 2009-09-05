using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a UI element that displays a data object.
	/// </summary>
	public class Row : ItemsControl
	{
		private readonly Binding dataBinding;

		/// <summary>
		/// Represents a UI element that displays a data object.
		/// </summary>
		public Row()
		{
			this.DefaultStyleKey = typeof(Row);

			this.dataBinding = new Binding { Source = this.DataContext, Mode = BindingMode.OneTime };
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Cell();
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		/// true if the item is (or is eligible to be) its own container; otherwise, false.
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
        	Cell cell = (Cell) element;
        	Column column = (Column) item;
			cell.Column = column;
        	cell.DataType = column.DataType;
        	cell.Style = column.CellStyle;
        	cell.SetBinding(DataContextProperty, this.dataBinding);
        	cell.SetBinding(Cell.ValueProperty, column.Binding);
        }

		/// <summary>
		/// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			Cell cell = (Cell) element;
			cell.Column = null;
			cell.ClearValue(DataContextProperty);
			cell.DataContext = null;
			cell.ClearValue(Cell.ValueProperty);
			cell.ClearValue(Cell.EditorValueProperty);
			cell.ClearValue(WidthProperty);
		}
	}
}
