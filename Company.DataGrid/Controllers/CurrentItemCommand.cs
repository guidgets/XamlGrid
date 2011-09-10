using System.ComponentModel;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
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
