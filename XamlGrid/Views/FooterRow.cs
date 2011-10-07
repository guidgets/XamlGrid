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
// File:	FooterRow.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Windows;
using System.Windows.Controls;
using XamlGrid.Controllers;

namespace XamlGrid.Views
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
			cell.IsReadOnly = true;

			if (cell.Column.FooterBinding != null)
			{
				cell.SetBinding(Cell.ValueProperty, cell.Column.FooterBinding);
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
