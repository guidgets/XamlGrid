using System;
using System.ComponentModel;
using Company.Widgets.Core;
using Company.Widgets.Models;

namespace Company.Widgets.Controllers
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
