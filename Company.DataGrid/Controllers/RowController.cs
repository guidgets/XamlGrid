using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Company.DataGrid.Core;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	public class RowController : Controller
	{
		public RowController(object viewComponent) : base(viewComponent.GetHashCode().ToString(), viewComponent)
		{

		}

		public Row Row
		{
			get
			{
				return (Row) this.ViewComponent;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			this.Row.DataContextChanged += this.Row_DataContextChanged;
			this.Row.IsCurrentChanged += this.Row_IsCurrentChanged;
			this.Row.IsSelectedChanged += this.Row_IsSelectedChanged;
			this.Row.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Row_MouseUp), true);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			this.Row.DataContextChanged -= this.Row_DataContextChanged;
			this.Row.IsCurrentChanged -= this.Row_IsCurrentChanged;
			this.Row.IsSelectedChanged -= this.Row_IsSelectedChanged;
			this.Row.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Row_MouseUp));
		}

		public override IList<string> ListNotificationInterests()
		{
			return new List<string>
			       	{
			       		Notifications.CURRENT_ITEM_CHANGED,
						Notifications.ITEM_IS_CURRENT,
			       		Notifications.ITEMS_SELECTED,
			       		Notifications.ITEMS_DESELECTED,
						Notifications.ITEM_IS_SELECTED
			       	};
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
				case Notifications.ITEM_IS_CURRENT:
					if (this.Row.DataContext == notification.Body)
					{
						this.Row.IsCurrent = bool.Parse(notification.Type);
					}
					break;
				case Notifications.CURRENT_ITEM_CHANGED:
					this.Row.IsCurrent = this.Row.DataContext == notification.Body;
					break;
				case Notifications.ITEMS_SELECTED:
					if (((IList) notification.Body).Contains(this.Row.DataContext))
					{
						this.Row.IsSelected = ((IList) notification.Body).Contains(this.Row.DataContext);
					}
					break;
				case Notifications.ITEMS_DESELECTED:
					IList list = (IList) notification.Body;
					if (list.Contains(this.Row.DataContext) || list.Count == 0)
					{
						this.Row.IsSelected = false;
					}
					break;
				case Notifications.ITEM_IS_SELECTED:
					if (this.Row.DataContext == notification.Body)
					{
						this.Row.IsSelected = bool.Parse(notification.Type);
					}
					break;
			}
		}

		private void Row_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.IS_ITEM_CURRENT, this.Row.DataContext);
			this.SendNotification(Notifications.IS_ITEM_SELECTED, this.Row.DataContext);
		}

		private void Row_IsCurrentChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Row.IsCurrent)
			{
				this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.Row.DataContext);
			}
		}

		private void Row_IsSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Row.IsSelected)
			{
				this.SendNotification(Notifications.ITEMS_SELECTING, this.Row.DataContext);
			}
			else
			{
				this.SendNotification(Notifications.ITEMS_DESELECTING, this.Row.DataContext);
			}
		}

		private void Row_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.Row.IsCurrent = true;
			string notification = this.Row.IsSelected ? Notifications.ITEMS_DESELECTING : Notifications.ITEMS_SELECTING;
			switch (this.Row.DataGrid.SelectionMode)
			{
				case SelectionMode.Single:
					if (!this.Row.IsSelected || 
						(this.Row.IsSelected && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
					{
						this.SendNotification(notification, this.Row.DataContext);
					}
					break;
				case SelectionMode.Multiple:
					this.SendNotification(notification, this.Row.DataContext);
					break;
				case SelectionMode.Extended:
					break;
			}
		}
	}
}
