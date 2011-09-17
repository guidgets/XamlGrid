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
