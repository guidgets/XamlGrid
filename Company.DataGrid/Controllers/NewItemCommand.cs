using System;
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
			switch (notification.Code)
			{
				case Notifications.ITEM_TYPE_CHANGED:
					NewItemModel newItemModel = (NewItemModel) DataGridFacade.Instance.RetrieveModel(NewItemModel.NAME);
					newItemModel.ItemType = (Type) notification.Body;
					break;
			}
		}
	}
}
