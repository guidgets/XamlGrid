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
					if (!selectionModel.SelectedItems.Contains(notification.Body))
					{
						for (int index = selectionModel.SelectedItems.Count - 1; index >= 0; index--)
						{
							selectionModel.SelectedItems.Remove(selectionModel.SelectedItems[index]);
						}
						selectionModel.SelectedItems.Add(notification.Body);
					}
					break;
				case Notifications.ITEMS_DESELECTING:
					if (selectionModel.SelectedItems.Contains(notification.Body))
					{
						selectionModel.SelectedItems.Remove(notification.Body);						
					}
					break;
				case Notifications.IS_ITEM_SELECTED:
					selectionModel.SendNotification(Notifications.ITEM_IS_SELECTED, notification.Body,
					                                selectionModel.SelectedItems.Contains(notification.Body).ToString());
					break;
			}
		}
	}
}
