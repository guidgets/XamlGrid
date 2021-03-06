﻿// This library is free software; you can redistribute it and/or
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
// File:	ControllerCollection.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.ObjectModel;
using System.Windows;
using Guidgets.XamlGrid.Core;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a collection of controllers associated with the same <see cref="DependencyObject"/>.
	/// </summary>
	public class ControllerCollection : ObservableCollection<IController>
	{
		private readonly DependencyObject associatedObject;

		/// <summary>
		/// Represents a collection of controllers associated with the same <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="o"></param>
		public ControllerCollection(DependencyObject o)
		{
			this.associatedObject = o;
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert.</param>
		protected override void InsertItem(int index, IController item)
		{
			base.InsertItem(index, item);

			item.View = this.associatedObject;
			item.Name = this.associatedObject.GetHashCode().ToString();

			DataGridFacade.Instance.RegisterController(item);
		}

		/// <summary>
		/// Removes the item at the specified index from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		protected override void RemoveItem(int index)
		{
			IController controller = this[index];

			controller.View = null;

			base.RemoveItem(index);

			DataGridFacade.Instance.RemoveController(controller.Name);
		}
	}
}
