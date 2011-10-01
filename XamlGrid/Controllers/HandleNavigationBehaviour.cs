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
// File:	HandleNavigationBehaviour.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Marks events caused by the some navigation keys as handled.
	/// </summary>
	public class HandleNavigationBehaviour : Behavior<UIElement>
	{
		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.KeyDown += AssociatedObject_KeyDown;
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
		}

		private static void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
				case Key.Right:
					if ((Keyboard.Modifiers & KeyHelper.CommandModifier) != KeyHelper.CommandModifier)
					{
						e.Handled = true;
					}
					break;
				case Key.Up:
				case Key.Down:
				case Key.Home:
				case Key.End:
					e.Handled = true;
					break;
			}
		}
	}
}
