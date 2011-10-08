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
// File:	RowController.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using XamlGrid.Core;
using XamlGrid.Models;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Row"/>.
	/// </summary>
	public class RowController : Controller
	{
		private static readonly CurrentItemModel currentItemModel =
			(CurrentItemModel) MainModel.Instance.RetrieveModel(CurrentItemModel.NAME);

		private static readonly SelectionModel selectionModel =
			(SelectionModel) MainModel.Instance.RetrieveModel(SelectionModel.NAME);


		/// <summary>
		/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Row"/>.
		/// </summary>
		/// <param name="row">The row for which functionality the <see cref="Controller"/> is responsible.</param>
		public RowController(Row row) : base(row.GetHashCode().ToString(), row)
		{

		}


		/// <summary>
		/// Gets the row for which functionality the <see cref="RowController"/> is responsible.
		/// </summary>
		public virtual Row Row
		{
			get { return (Row) this.ViewComponent; }
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();
			this.Row.DataContextChanged += this.Row_DataContextChanged;
			this.Row.HasFocusChanged += this.Row_HasFocusedChanged;
			this.Row.IsSelectedChanged += this.Row_IsSelectedChanged;
			this.Row.KeyDown += this.Row_KeyDown;
			this.Row.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Row_MouseLeftButtonDown), true);
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();
			this.Row.DataContextChanged -= this.Row_DataContextChanged;
			this.Row.HasFocusChanged -= this.Row_HasFocusedChanged;
			this.Row.IsSelectedChanged -= this.Row_IsSelectedChanged;
			this.Row.KeyDown -= this.Row_KeyDown;
			this.Row.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Row_MouseLeftButtonDown));
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int>
			       	{
			       		Notifications.CurrentItemChanged,
			       		Notifications.SelectedItems,
			       		Notifications.DeselectedItems,
			       		Notifications.ItemIsSelected
			       	};
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
			switch (notification.Code)
			{
				case Notifications.CurrentItemChanged:
					this.Row.IsFocused = this.Row.DataContext == notification.Body;
					break;
				case Notifications.SelectedItems:
					if (((IList<object>) notification.Body).Contains(this.Row.DataContext))
					{
						this.Row.IsSelected = true;
					}
					break;
				case Notifications.DeselectedItems:
					IList<object> list = (IList<object>) notification.Body;
					if (list.Contains(this.Row.DataContext) || list.Count == 0)
					{
						this.Row.IsSelected = false;
					}
					break;
				case Notifications.ItemIsSelected:
					if (this.Row.DataContext == notification.Body)
					{
						this.Row.IsSelected = bool.Parse(notification.Type);
					}
					break;
			}
		}

		private void Row_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.Row.IsFocused = this.Row.DataContext == currentItemModel.CurrentItem;
			this.Row.IsSelected = selectionModel.SelectedItems.Any(s => s.Item == this.Row.DataContext);
		}

		private void Row_HasFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Row.IsFocused)
			{
				this.SendNotification(Notifications.CurrentItemChanging, this.Row.DataContext);
			}
		}

		private void Row_IsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(this.Row.IsSelected ? Notifications.SelectingItems : Notifications.DeselectingItems,
			                      this.Row.DataContext);
		}

		private void Row_KeyDown(object sender, KeyEventArgs e)
		{
			this.SendNotification(Notifications.ItemKeyDown, e);
		}

		private void Row_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.SendNotification(Notifications.ItemClicked, this.Row.DataContext);
		}
	}
}
