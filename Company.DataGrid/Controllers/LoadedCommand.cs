using Company.DataGrid.Core;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class LoadedCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			switch (notification.Name)
			{
				case Notifications.HEADER_ROW_LOADED:
					SortingModel sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
					sortingModel.StartNotifications();
					break;
			}
		}
	}
}
