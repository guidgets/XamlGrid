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
// File:	ItemPropertyChangedEventArgs.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;

namespace Guidgets.XamlGrid.Models
{
	/// <summary>
	/// Provides data for the <see cref="ObservableItemCollection{T}.ItemPropertyChanged"/> event.
	/// </summary>
	public class ItemPropertyChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Provides data for the <see cref="ObservableItemCollection{T}.ItemPropertyChanged"/> event.
		/// </summary>
		/// <param name="item">The item which had a property changed.</param>
		/// <param name="propertyPath">The property path in which the name of the changed property is located.</param>
		/// <param name="propertyName">The name of the property which value changed.</param>
		public ItemPropertyChangedEventArgs(object item, string propertyPath, string propertyName)
		{
			this.Item = item;
			this.PropertyPath = propertyPath;
			this.PropertyName = propertyName;
		}

		/// <summary>
		/// Gets the item which had a property changed.
		/// </summary>
		public virtual object Item
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets the property path in which the name of the changed property is located.
		/// </summary>
		/// <value>The property path.</value>
		public virtual string PropertyPath
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets the name of the property which value changed.
		/// </summary>
		/// <value>The name of the property which value changed.</value>
		public virtual string PropertyName
		{
			get; 
			private set;
		}
	}
}
