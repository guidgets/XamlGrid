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
// File:	RowBase.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XamlGrid.Views
{
	public abstract class RowBase : ItemsControl
	{
		public event DependencyPropertyChangedEventHandler VisibilityChanged;

		private static readonly DependencyProperty visibilityListenerProperty =
			DependencyProperty.Register("visibilityListener", typeof(Visibility), typeof(RowBase),
										new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

		private static readonly Binding visibilityBinding = new Binding("Visibility")
		                                                    {
		                                                    	RelativeSource = new RelativeSource(RelativeSourceMode.Self),
		                                                    	Mode = BindingMode.OneWay
		                                                    };


		protected RowBase()
		{
			this.SetBinding(visibilityListenerProperty, visibilityBinding);
		}


		/// <summary>
		/// Undoes the effects of the <see cref="ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DataGridFacade.Instance.RemoveController(element.GetHashCode().ToString());
		}

		private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RowBase) d).OnVisibilityChanged(e);
		}

		protected virtual void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.VisibilityChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
