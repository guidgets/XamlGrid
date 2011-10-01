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
// File:	SelectionCommand.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Collections.Generic;
using System.Windows.Controls;
using XamlGrid.Core;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	public class SelectionCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			base.Execute(notification);
			SelectionModel selectionModel = (SelectionModel) DataGridFacade.Instance.RetrieveModel(SelectionModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ItemsChanged:
					selectionModel.Items = notification.Body as IList<object>;
					break;
				case Notifications.SelectingItems:
					if (notification.Type == NotificationTypes.ClearSelection)
					{
						selectionModel.SelectedItems.Clear();
					}
					selectionModel.Select(notification.Body);
					break;
				case Notifications.SelectAll:
					selectionModel.SelectAll();
					break;
				case Notifications.SelectRange:
					selectionModel.SelectRange(notification.Body, notification.Type == NotificationTypes.ClearSelection);
					break;
				case Notifications.DeselectingItems:
					selectionModel.SelectedItems.Deselect(notification.Body);
					break;
				case Notifications.IsItemSelected:
					selectionModel.SendNotification(Notifications.ItemIsSelected, notification.Body,
					                                selectionModel.SelectedItems.IsSelected(notification.Body).ToString());
					break;
				case Notifications.SelectionModeChanging:
					selectionModel.SelectionMode = (SelectionMode) notification.Body;
					break;
			}
		}
	}
}
