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
// File:	SelectedItem.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>
// 
namespace Guidgets.XamlGrid.Models
{
	/// <summary>
	/// Represents a selected from a list item.
	/// </summary>
	public struct SelectedItem
	{
		/// <summary>
		/// Represents a selected from a list item.
		/// </summary>
		/// <param name="item">The selected item.</param>
		/// <param name="index">The index of the item in its list.</param>
		public SelectedItem(object item, int index) : this()
		{
			this.Item = item;
			this.Index = index;
		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		public object Item { get; private set; }

		/// <summary>
		/// Gets or sets the index of the item in its list.
		/// </summary>
		public int Index { get; private set; }
	}
}
