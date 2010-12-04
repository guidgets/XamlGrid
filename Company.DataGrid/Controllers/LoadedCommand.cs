using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
{
	public class LoadedCommand : SimpleCommand
	{
		public override void Execute(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.HEADER_ROW_LOADED:
					SortingModel sortingModel = (SortingModel) DataGridFacade.Instance.RetrieveModel(SortingModel.NAME);
					sortingModel.StartNotifications();
					break;
			}
		}
	}
}
