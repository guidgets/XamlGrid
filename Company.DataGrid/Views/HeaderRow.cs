using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	public class HeaderRow : ItemsControl
	{
		public HeaderRow()
		{
			this.DefaultStyleKey = typeof(HeaderRow);

			DataGridFacade.Instance.RegisterController(new HeaderRowController(this));
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new HeaderCell();
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			HeaderCell headerCell = (HeaderCell) element;
			Column column = (Column) item;
			headerCell.Column = column;
		}

		/// <summary>
		/// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			HeaderCell headerCell = (HeaderCell) element;
			headerCell.Column = null;
			DataGridFacade.Instance.RemoveController(headerCell.GetHashCode().ToString());
		}
	}
}
