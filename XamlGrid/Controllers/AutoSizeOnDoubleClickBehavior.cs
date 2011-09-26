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
// File:	AutoSizeOnDoubleClickBehavior.cs
// Authors:	Dimitar Dobrev

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Behavior{T}"/> for a <see cref="Thumb"/> which enables the thumb to auto-size a <see cref="Column"/> of a <see cref="DataGrid"/>.
	/// </summary>
	public class AutoSizeOnDoubleClickBehavior : Behavior<Thumb>
	{
		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent,
			                                 new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown), true);
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent,
			                                    new MouseButtonEventHandler(this.AssociatedObject_MouseLeftButtonDown));
			base.OnDetaching();
		}

		private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				Column columnToResize = this.AssociatedObject.Tag as Column;
				if (columnToResize != null && columnToResize.IsResizable)
				{
					columnToResize.AutoSize();
				}
			}
		}
	}
}
