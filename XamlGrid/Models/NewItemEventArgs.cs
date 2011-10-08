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
// File:	NewItemEventArgs.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;

namespace XamlGrid.Models
{
	/// <summary>
	/// Contains event data about events raised when an attempt is made to create a new item to be added to a list.
	/// </summary>
	public class NewItemEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the new item to add.
		/// </summary>
		/// <value>The new item to add.</value>
		public object NewItem
		{
			get; 
			set;
		}
	}
}
