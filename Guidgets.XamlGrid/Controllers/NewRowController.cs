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
// File:	NewRowController.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	public class NewRowController : Controller<NewRow>
	{
		public NewRowController(NewRow view) : base(view.GetHashCode().ToString(), view)
		{

		}


		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.Loaded += this.NewRow_Loaded;
			this.View.VisibilityChanged += this.NewRow_VisibilityChanged;
			this.View.KeyDown += this.NewRow_KeyDown;
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.Loaded -= this.NewRow_Loaded;
			this.View.VisibilityChanged -= this.NewRow_VisibilityChanged;
			this.View.KeyDown -= this.NewRow_KeyDown;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.NewItemAdded, Notifications.ItemsSourceChanged };
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
				case Notifications.NewItemAdded:
					this.View.DataContext = notification.Body;
					break;
				case Notifications.ItemsSourceChanged:
					this.EnsureNewItem();
					break;
			}
		}


		private void EnsureNewItem()
		{
			if (this.View.Visibility == Visibility.Visible)
			{
				this.SendNotification(Notifications.NewItemAdd);
			}
		}


		private void NewRow_Loaded(object sender, RoutedEventArgs e)
		{
			this.EnsureNewItem();
		}

		private void NewRow_VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.EnsureNewItem();
		}

		private void NewRow_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					this.SendNotification(Notifications.NewItemCommit);
					break;
				case Key.Escape:
					this.View.FocusHorizontalNeighbour(true);
					break;
			}
		}
	}
}
