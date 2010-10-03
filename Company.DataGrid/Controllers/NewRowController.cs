﻿using System.Collections.Generic;
using System.Windows;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	public class NewRowController : Controller
	{
		public NewRowController(object viewComponent) : base(viewComponent.GetHashCode().ToString(), viewComponent)
		{

		}


		public NewRow NewRow
		{
			get
			{
				return (NewRow) this.ViewComponent;
			}
		}


		public override void OnRegister()
		{
			base.OnRegister();

			this.NewRow.Loaded += this.NewRow_Loaded;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.NewRow.Loaded -= this.NewRow_Loaded;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.NEW_ITEM_ADDED };
		}

		/// <summary>
		/// Handle <c>INotification</c>s.
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.NEW_ITEM_ADDED:
					this.NewRow.DataContext = notification.Body;
					break;
			}
		}


		private void NewRow_Loaded(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.NEW_ITEM_ADD);
		}
	}
}
