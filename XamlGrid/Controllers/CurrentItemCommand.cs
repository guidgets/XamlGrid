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
// File:	CurrentItemCommand.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.ComponentModel;
using XamlGrid.Core;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	public class CurrentItemCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			base.Execute(notification);
			CurrentItemModel currentItemModel = (CurrentItemModel) DataGridFacade.Instance.RetrieveModel(CurrentItemModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ItemsSourceChanged:
					currentItemModel.SetCollectionView(notification.Body as ICollectionView);
					break;
				case Notifications.CurrentItemUp:
					currentItemModel.MoveCurrentToPrevious();
					break;
				case Notifications.CurrentItemDown:
					currentItemModel.MoveCurrentToNext();
					break;
				case Notifications.CurrentItemToPosition:
					currentItemModel.MoveCurrentToPosition((int) notification.Body);
					break;
				case Notifications.CurrentItemFirst:
					currentItemModel.MoveCurrentToFirst();
					break;
				case Notifications.CurrentItemLast:
					currentItemModel.MoveCurrentToLast();
					break;
				case Notifications.CurrentItemChanging:
					currentItemModel.MoveCurrentTo(notification.Body);
					break;
				case Notifications.IsItemCurrent:
					this.SendNotification(Notifications.CurrentItemChanged, currentItemModel.CurrentItem);
					break;
			}
		}
	}
}
