using System.Windows;
using System.Windows.Controls;
using Company.Widgets.Controllers;

namespace Company.Widgets.Views
{
	public class FooterRow : Row
	{
		public FooterRow()
		{
			this.DefaultStyleKey = typeof(FooterRow);

			DataGridFacade.Instance.RemoveController(this.GetHashCode().ToString()); 
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			Cell cell = (Cell) element;
			cell.ClearValue(Cell.ValueProperty);
			cell.IsEditable = false;

			if (cell.Column.FooterBinding != null)
			{
				cell.SetBinding(Cell.ValueProperty, cell.Column.FooterBinding);
			}
			else
			{
				cell.DataType = typeof(object);
			}
			DataGridFacade.Instance.RegisterController(new FooterCellController(cell));
		}

		/// <summary>
		/// Undoes the effects of the <see cref="ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			DataGridFacade.Instance.RemoveController(FooterCellController.FOOTER_PREFIX + element.GetHashCode());
		}
	}
}
