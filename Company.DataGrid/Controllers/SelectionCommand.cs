using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public class SelectionCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			SelectionModel selectionModel = (SelectionModel) DataGridFacade.Instance.RetrieveModel(SelectionModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ITEMS_CHANGED:
					selectionModel.Items = notification.Body as IList<object>;
					break;
				case Notifications.SELECTING_ITEMS:
					if (notification.Type == NotificationTypes.CLEAR_SELECTION)
					{
						selectionModel.SelectedItems.Clear();
					}
					selectionModel.Select(notification.Body);
					break;
				case Notifications.SELECT_ALL:
					selectionModel.SelectAll();
					break;
				case Notifications.SELECT_RANGE:
					selectionModel.SelectRange(notification.Body, notification.Type == NotificationTypes.CLEAR_SELECTION);
					break;
				case Notifications.DESELECTING_ITEMS:
					selectionModel.SelectedItems.Deselect(notification.Body);
					break;
				case Notifications.IS_ITEM_SELECTED:
					selectionModel.SendNotification(Notifications.ITEM_IS_SELECTED, notification.Body,
					                                selectionModel.SelectedItems.IsSelected(notification.Body).ToString());
					break;
				case Notifications.SELECTION_MODE_CHANGING:
					selectionModel.SelectionMode = (SelectionMode) notification.Body;
					break;
			}
		}
	}
}
