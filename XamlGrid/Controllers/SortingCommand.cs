using System.ComponentModel;
using XamlGrid.Core;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	public class SortingCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			base.Execute(notification);
			SortingModel sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ItemsSourceChanged:
					sortingModel.SetCollectionView(notification.Body as ICollectionView);
					break;
				case Notifications.SortingState:
					sortingModel.CheckSortingState((string) notification.Body);
					break;
				case Notifications.SortingRequested:
					sortingModel.Sort((ExtendedSortDescription) notification.Body);
					break;
				case Notifications.ItemPropertyChanged:
					sortingModel.RefreshIfSorted((string) ((object[]) notification.Body)[1]);
					break;
			}
		}
	}
}
