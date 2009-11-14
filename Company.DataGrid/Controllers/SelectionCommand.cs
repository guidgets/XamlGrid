using System.Windows.Controls;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class SelectionCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			SelectionModel selectionModel = (SelectionModel) DataGridFacade.Instance.RetrieveModel(SelectionModel.NAME);
			switch (notification.Name)
			{
				case Notifications.ITEMS_SELECTING:
					selectionModel.SelectedItems.Add(notification.Body);
					break;
				case Notifications.ITEMS_DESELECTING:
					selectionModel.SelectedItems.Remove(notification.Body);
					break;
				case Notifications.IS_ITEM_SELECTED:
					selectionModel.SendNotification(Notifications.ITEM_IS_SELECTED, notification.Body,
					                                selectionModel.SelectedItems.Contains(notification.Body).ToString());
					break;
				case Notifications.SELECTION_MODE_CHANGING:
					selectionModel.SelectionMode = (SelectionMode) notification.Body;
					break;
			}
		}
	}
}
