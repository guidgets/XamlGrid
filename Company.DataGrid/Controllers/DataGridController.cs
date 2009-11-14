using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Company.DataGrid.Core;

namespace Company.DataGrid.Controllers
{
	public class DataGridController : Controller
	{
		public new const string NAME = "DataGridController";

		public DataGridController(object viewComponent) : base(NAME, viewComponent)
		{

		}

		public Views.DataGrid DataGrid
		{
			get
			{
				return (Views.DataGrid) this.ViewComponent;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();

			this.DataGrid.DataSourceChanged += this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged += this.DataGrid_ItemsChanged;
			this.DataGrid.CurrentItemChanged += this.DataGrid_CurrentItemChanged;
			this.DataGrid.SelectionModeChanged += this.DataGrid_SelectionModeChanged;
			this.DataGrid.Columns.CollectionChanged += this.Columns_CollectionChanged;

			this.DataGrid.KeyDown += this.DataGrid_KeyDown;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.DataGrid.DataSourceChanged -= this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged -= this.DataGrid_ItemsChanged;
			this.DataGrid.CurrentItemChanged -= this.DataGrid_CurrentItemChanged;
			this.DataGrid.SelectionModeChanged -= this.DataGrid_SelectionModeChanged;
			this.DataGrid.Columns.CollectionChanged -= this.Columns_CollectionChanged;

			this.DataGrid.KeyDown -= this.DataGrid_KeyDown;
		}

		public override IList<string> ListNotificationInterests()
		{
			return new List<string>
			       	{
			       		Notifications.DATA_WRAPPED,
			       		Notifications.CURRENT_ITEM_CHANGED,
			       		Notifications.SELECTION_MODE_CHANGED
			       	};
		}

		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
				case Notifications.DATA_WRAPPED:
					this.DataGrid.ItemsSource = (IEnumerable) notification.Body;
					break;
				case Notifications.CURRENT_ITEM_CHANGED:
					this.DataGrid.CurrentItem = notification.Body;
					break;
				case Notifications.SELECTION_MODE_CHANGED:
					this.DataGrid.SelectionMode = (SelectionMode) notification.Body;
					break;
			}
		}

		private void DataGrid_DataSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null || e.NewValue is IEnumerable)
			{
				this.SendNotification(Notifications.DATA_WRAPPING_REQUESTED, e.NewValue);
			}
		}

		private void DataGrid_ItemsChanged(object sender, DependencyPropertyChangedEventArgs e1)
		{
			this.SendNotification(Notifications.ITEMS_SOURCE_CHANGED, this.DataGrid.ItemsSource);
		}

		private void DataGrid_CurrentItemChanged(object sender, DependencyPropertyChangedEventArgs e1)
		{
			this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.CurrentItem);
		}

		private void DataGrid_SelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.SELECTION_MODE_CHANGING, this.DataGrid.SelectionMode);
		}

		private void Columns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SendNotification(Notifications.COLUMNS_CHANGED, e);
		}

		private void DataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			string notification = null;
			switch (e.Key)
			{
				case Key.Up:
					notification = Notifications.CURRENT_ITEM_UP;
					break;
				case Key.Down:
					notification = Notifications.CURRENT_ITEM_DOWN;
					break;
				case Key.Home:
					notification = Notifications.CURRENT_ITEM_FIRST;
					break;
				case Key.End:
					notification = Notifications.CURRENT_ITEM_LAST;
					break;
				case Key.Space:
					if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
					{
						notification = this.DataGrid.SelectedItems.Contains(this.DataGrid.CurrentItem) ?
											Notifications.ITEMS_DESELECTING : Notifications.ITEMS_SELECTING;
					}
					break;
			}
			if (notification != null)
			{
				this.SendNotification(notification, this.DataGrid.CurrentItem);
				switch (this.DataGrid.SelectionMode)
				{
					case SelectionMode.Single:
						if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
						{
							this.SendNotification(Notifications.ITEMS_SELECTING, this.DataGrid.CurrentItem);
						}
						break;
					case SelectionMode.Multiple:
						break;
					case SelectionMode.Extended:
						break;
				}
			}
		}
	}
}