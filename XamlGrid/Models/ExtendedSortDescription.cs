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
// File:	ExtendedSortDescription.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.ComponentModel;

namespace XamlGrid.Models
{
	/// <summary>
	/// Contains the property and the direction to use when sorting a collection.
	/// </summary>
	public struct ExtendedSortDescription
	{
		public ExtendedSortDescription(string property, ListSortDirection? sortDirection) : this(property, sortDirection, false)
		{
			this.Property = property;
			this.SortDirection = sortDirection;
		}

		public ExtendedSortDescription(string property, ListSortDirection? sortDirection, bool clearPreviousSorting) : this()
		{
			this.Property = property;
			this.SortDirection = sortDirection;
			this.ClearPreviousSorting = clearPreviousSorting;
		}

		/// <summary>
		/// Gets or sets the property by which to sort.
		/// </summary>
		/// <value>The property by which to sort.</value>
		public string Property
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the direction in which to sort.
		/// </summary>
		/// <value>The direction in which to sort.</value>
		public ListSortDirection? SortDirection
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether all previous sorting has to be cleared.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if all previous sorting has to be cleared; otherwise, <c>false</c>.
		/// </value>
		public bool ClearPreviousSorting
		{
			get;
			set;
		}
	}
}
