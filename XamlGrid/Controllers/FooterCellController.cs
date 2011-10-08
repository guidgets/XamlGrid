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
// File:	FooterCellController.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System.Collections.Generic;
using System.Collections.Specialized;
using XamlGrid.Core;
using XamlGrid.Views;

namespace XamlGrid.Controllers
{
	public class FooterCellController : Controller
	{
		public const string FOOTER_PREFIX = "footer";


		public FooterCellController(object viewComponent) : base(FOOTER_PREFIX + viewComponent.GetHashCode(), viewComponent)
		{

		}

		public virtual Cell FooterCell
		{
			get
			{
				return (Cell) this.ViewComponent;
			}
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			((INotifyCollectionChanged) this.FooterCell.DataContext).CollectionChanged += this.FooterCellController_CollectionChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			((INotifyCollectionChanged) this.FooterCell.DataContext).CollectionChanged -= this.FooterCellController_CollectionChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.ItemPropertyChanged };
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
				case Notifications.ItemPropertyChanged:
					this.Update();
					break;
			}
		}

		private void FooterCellController_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.Update();
		}

		private void Update()
		{
			this.FooterCell.ClearValue(Cell.ValueProperty);
			if (this.FooterCell.Column.FooterBinding != null)
			{
				this.FooterCell.SetBinding(Cell.ValueProperty, this.FooterCell.Column.FooterBinding);
			}
		}
	}
}
