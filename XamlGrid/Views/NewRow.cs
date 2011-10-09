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
// File:	NewRow.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using XamlGrid.Controllers;

namespace XamlGrid.Views
{
	/// <summary>
	/// Represents an empty <see cref="Row"/> that serves to fill in and enter new items to the set of <see cref="Row"/>s.
	/// </summary>
	public class NewRow : Row
	{
		/// <summary>
		/// Represents an empty <see cref="Row"/> that serves to fill in and enter new items to the set of <see cref="Row"/>s.
		/// </summary>
		public NewRow()
		{
			this.DefaultStyleKey = typeof(NewRow);

			DataGridFacade.Instance.RemoveController(this.GetHashCode().ToString());
			DataGridFacade.Instance.RegisterController(new NewRowController(this));
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(System.Windows.DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			Cell cell = (Cell) element;
			DataGridFacade.Instance.RemoveController(cell.GetHashCode().ToString());
			cell.IsInEditMode = true;
		}
	}
}
