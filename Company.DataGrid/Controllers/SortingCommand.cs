using System.ComponentModel;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class SortingCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			SortingModel sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
			switch (notification.Name)
			{
				case Notifications.ITEMS_SOURCE_CHANGED:
					sortingModel.SetCollectionView(notification.Body as ICollectionView);
					break;
				case Notifications.SORTING_REQUESTED:
					SortDescription sortDescription = (SortDescription) notification.Body;
					if (notification.Type == NotificationTypes.REMOVED_SORTING)
					{
						sortingModel.Sort(sortDescription.PropertyName, null);
					}
					else
					{
						sortingModel.Sort(sortDescription.PropertyName, sortDescription.Direction);
					}
					break;
				case Notifications.REFRESH_SORTING:
					sortingModel.RefreshIfSorted((string) notification.Body);
					break;
			}
		}
	}
}
