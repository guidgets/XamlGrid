﻿using System.ComponentModel;
using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class SortingCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			SortingModel sortingModel;
			switch (notification.Name)
			{
				case Notifications.SORTING_REQUESTED:
					sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
					SortDescription sortDescription = (SortDescription) notification.Body;
					if (notification.Type == NotificationTypes.REMOVED_SORTING)
					{
						sortingModel.HandleSortingRequest(sortDescription.PropertyName, null);
					}
					else
					{
						sortingModel.HandleSortingRequest(sortDescription.PropertyName, sortDescription.Direction);
					}
					break;
				case Notifications.REFRESH_SORTING:
					sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
					sortingModel.RefreshIfSorted((string) notification.Body);
					break;
				case Notifications.ITEMS_SOURCE_CHANGED:
					sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
					sortingModel.SetCollectionView(notification.Body as ICollectionView);
					break;
			}
		}
	}
}