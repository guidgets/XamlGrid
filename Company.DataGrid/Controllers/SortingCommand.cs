using System.ComponentModel;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public class SortingCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			SortingModel sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ITEMS_SOURCE_CHANGED:
					sortingModel.SetCollectionView(notification.Body as ICollectionView);
					break;
				case Notifications.SORTING_REQUESTED:
					sortingModel.Sort((ExtendedSortDescription) notification.Body);
					break;
				case Notifications.ITEM_PROPERTY_CHANGED:
					sortingModel.RefreshIfSorted((string) ((object[]) notification.Body)[1]);
					break;
			}
		}
	}
}
