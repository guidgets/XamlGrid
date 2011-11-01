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
// File:	HeaderCellController.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Models;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="HeaderCell"/>.
	/// </summary>
	public class HeaderCellController : Controller<HeaderCell>
	{
		/// <summary>
		/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="HeaderCell"/>.
		/// </summary>
		/// <param name="headerCell">The header cell for which functionality the <see cref="Controller{T}"/> is responsible.</param>
		public HeaderCellController(HeaderCell headerCell) : base(headerCell.GetHashCode().ToString(), headerCell)
		{

		}


		/// <summary>
		/// Called by the Controller when the Controller is registered
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.Loaded += this.HeaderCell_Loaded;
			this.View.SortDirectionChanged += this.HeaderCell_SortDirectionChanged;
		}

		/// <summary>
		/// Called by the Controller when the Controller is removed
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.Loaded -= this.HeaderCell_Loaded;
			this.View.SortDirectionChanged -= this.HeaderCell_SortDirectionChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.Sorted };
		}

		/// <summary>
		/// Handle <c>INotification</c>s
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			base.HandleNotification(notification);
			SortDescription sortDescription = (SortDescription) notification.Body;
			if (!string.IsNullOrEmpty(sortDescription.PropertyName) && sortDescription.PropertyName != this.View.Column.Binding.Path.Path)
			{
				return;
			}
			switch (notification.Code)
			{
				case Notifications.Sorted:
					if (notification.Type == NotificationTypes.NoSorting)
					{
						this.View.SortDirection = null;
					}
					else
					{
						this.View.SortDirection = sortDescription.Direction;
					}
					break;
			}
		}


		private void HeaderCell_Loaded(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.SortingState, this.View.Column.Binding.Path.Path);
		}

		private void HeaderCell_SortDirectionChanged(object sender, SortDirectionEventArgs e)
		{
			ExtendedSortDescription sortDescription = new ExtendedSortDescription();
			sortDescription.Property = this.View.Column.Binding.Path.Path;
			sortDescription.ClearPreviousSorting = (Keyboard.Modifiers & ModifierKeys.Shift) == 0;
			sortDescription.SortDirection = e.SortDirection;
			this.SendNotification(Notifications.SortingRequested, sortDescription);
		}
	}
}
