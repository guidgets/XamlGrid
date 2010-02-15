using System.ComponentModel;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class CurrentItemCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			CurrentItemModel currentItemModel = (CurrentItemModel) DataGridFacade.Instance.RetrieveModel(CurrentItemModel.NAME);
			switch (notification.Name)
			{
				case Notifications.ITEMS_SOURCE_CHANGED:
					currentItemModel.SetCollectionView(notification.Body as ICollectionView);
					break;
				case Notifications.CURRENT_ITEM_UP:
					currentItemModel.MoveCurrentToPrevious();
					break;
				case Notifications.CURRENT_ITEM_DOWN:
					currentItemModel.MoveCurrentToNext();
					break;
				case Notifications.CURRENT_ITEM_TO_POSITION:
					currentItemModel.MoveCurrentToPosition((int) notification.Body);
					break;
				case Notifications.CURRENT_ITEM_FIRST:
					currentItemModel.MoveCurrentToFirst();
					break;
				case Notifications.CURRENT_ITEM_LAST:
					currentItemModel.MoveCurrentToLast();
					break;
				case Notifications.CURRENT_ITEM_CHANGING:
					currentItemModel.MoveCurrentTo(notification.Body);
					break;
				case Notifications.IS_ITEM_CURRENT:
					currentItemModel.SendNotification(Notifications.ITEM_IS_CURRENT, notification.Body,
					                                  (currentItemModel.CurrentItem == notification.Body).ToString());
					break;
			}
		}
	}
}
