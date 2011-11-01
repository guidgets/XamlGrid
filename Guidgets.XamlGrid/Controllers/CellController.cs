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
// File:	CellController.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="Views.Cell"/>.
	/// </summary>
	public class CellController : Controller<Cell>
	{
		/// <summary>
		/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="Views.Cell"/>.
		/// </summary>
		/// <param name="cell">The cell for which functionality the <see cref="Controller{T}"/> is responsible.</param>
		public CellController(Cell cell) : base(cell.GetHashCode().ToString(), cell)
		{

		}


		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonDown), true);
			this.View.GotFocus += this.Cell_GotFocus;
            this.View.LostFocus += this.Cell_LostFocus;
			this.View.KeyDown += this.Cell_KeyDown;
			this.View.IsInEditModeChanged += this.Cell_IsInEditModeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonDown));
			this.View.GotFocus -= this.Cell_GotFocus;
		    this.View.LostFocus -= this.Cell_LostFocus;
			this.View.KeyDown -= this.Cell_KeyDown;
			this.View.IsInEditModeChanged -= this.Cell_IsInEditModeChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.FocusCell };
		}

		/// <summary>
		/// Handle <c>INotification</c>s.
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			base.HandleNotification(notification);
			switch (notification.Code)
			{
				case Notifications.FocusCell:
					object[] data = (object[]) notification.Body;
					if (this.View.DataContext == data[0] && this.View.Column == data[1] && !this.View.HasFocus)
					{
						this.View.Focus();
					}
					break;
			}
		}


		private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				this.View.IsInEditMode = true;
			}
		}

		private void Cell_GotFocus(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.CellFocused, this.View);
		}

		private void Cell_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!this.View.HasFocus)
			{
				this.View.IsInEditMode = false;
			}
		}

		private void Cell_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.F2:
					this.View.IsInEditMode = true;
					break;
				case Key.Enter:
					if (!EditorController.SentFromMultilineTextBox(e))
					{
						this.View.IsInEditMode = !this.View.IsInEditMode;
						if (!this.View.IsInEditMode || this.View.AlwaysInEditMode)
						{
							this.View.FocusHorizontalNeighbour(true);
						}
					}
					break;
				case Key.Escape:
					if (this.View.IsInEditMode)
					{
						this.View.IsInEditMode = false;
					}
					break;
				case Key.Left:
				case Key.Right:
					if ((Keyboard.Modifiers & KeyHelper.CommandModifier) == ModifierKeys.None)
					{
						this.View.FocusHorizontalNeighbour(e.Key == Key.Right);
					}
					break;
			}
		}

		private void Cell_IsInEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CellEditModeChanged, this.View.IsInEditMode);
		}
	}
}
