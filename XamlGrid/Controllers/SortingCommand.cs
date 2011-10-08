// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	SortingCommand.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

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
