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
// File:	ItemsControlExtensions.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace XamlGrid.Views
{
	/// <summary>
	/// Extends the functionality of an <see cref="ItemsControl"/>.
	/// </summary>
	public static class ItemsControlExtensions
	{
		/// <summary>
		/// Gets the <see cref="ScrollViewer"/> that contains the items of the specified <see cref="ItemsControl"/>.
		/// </summary>
		/// <param name="itemsControl">The <see cref="ItemsControl"/> to get the <see cref="ScrollViewer"/> of.</param>
		/// <returns>The <see cref="ScrollViewer"/> of the specified <see cref="ItemsControl"/>.</returns>
		public static ScrollViewer GetScroll(this ItemsControl itemsControl)
		{
			DependencyObject child = GetItemsPresenter(itemsControl);
			while (child != null && !(child is ScrollViewer))
			{
				child = child.GetParent();
			}
			return child as ScrollViewer;
		}

		/// <summary>
		/// Gets the <see cref="ItemsPresenter"/>, if any, of the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="dependencyObject">The <see cref="DependencyObject"/> to get the <see cref="ItemsPresenter"/> of.</param>
		/// <returns>The <see cref="ItemsPresenter"/>, if any, of the specified <see cref="DependencyObject"/></returns>
		private static ItemsPresenter GetItemsPresenter(this DependencyObject dependencyObject)
		{
			ItemsPresenter presenter = dependencyObject as ItemsPresenter;
			if (presenter != null)
			{
				return presenter;
			}
			DependencyObject content = null;
			ContentControl contentControl = dependencyObject as ContentControl;
			if (contentControl != null)
			{
				content = contentControl.Content as DependencyObject;
			}
			ContentPresenter contentPresenter = dependencyObject as ContentPresenter;
			if (contentPresenter != null)
			{
				content = contentPresenter.Content as DependencyObject;
			}
			if (content != null)
			{
				ItemsPresenter itemsPresenter = GetItemsPresenter(content);
				if (itemsPresenter != null)
				{
					return itemsPresenter;
				}
			}
			return (from descendant in dependencyObject.GetVisualDescendants()
					where !(descendant is ItemsControl)
					let itemsPresenter = GetItemsPresenter(descendant)
					where itemsPresenter != null
					select itemsPresenter).FirstOrDefault();
		}
	}
}
