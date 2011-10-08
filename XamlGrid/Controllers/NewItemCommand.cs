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
// File:	NewItemCommand.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System;
using System.ComponentModel;
using XamlGrid.Core;
using XamlGrid.Models;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Delegates actions to models meant to create new items.
	/// </summary>
	public class NewItemCommand : SimpleCommand
	{
		/// <summary>
		/// Fulfill the use-case initiated by the given <c>INotification</c>
		/// </summary>
		/// <param name="notification">The <c>INotification</c> to handle</param>
		/// <remarks>
		/// In the Command Pattern, an application use-case typically begins with some user action, which results in an <c>INotification</c> being broadcast, which is handled by business logic in the <c>execute</c> method of an <c>ICommand</c>
		/// </remarks>
		public override void Execute(INotification notification)
		{
			base.Execute(notification);
			NewItemModel newItemModel = (NewItemModel) DataGridFacade.Instance.RetrieveModel(NewItemModel.NAME);
			switch (notification.Code)
			{
				case Notifications.ItemsSourceChanged:
					newItemModel.SetSource(notification.Body as ICollectionView);
					break;
				case Notifications.ItemTypeChanged:
					newItemModel.ItemType = (Type) notification.Body;
					break;
				case Notifications.NewItemAdd:
					newItemModel.AddItem();
					break;
				case Notifications.NewItemCommit:
					newItemModel.CommitItem();
					break;
			}
		}
	}
}
