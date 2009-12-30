using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
			this.DataGrid.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;
			this.DataGrid.CurrentItemChanged += this.DataGrid_CurrentItemChanged;
			this.DataGrid.SelectionModeChanged += this.DataGrid_SelectionModeChanged;
			this.DataGrid.Columns.CollectionChanged += this.Columns_CollectionChanged;

			this.DataGrid.KeyDown += this.DataGrid_KeyDown;
		}

		public override void OnRemove()
		{
			base.OnRemove();

			this.DataGrid.DataSourceChanged -= this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged -= this.DataGrid_ItemsSourceChanged;
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
					this.DataGrid.CurrentItem = this.DataGrid.Items.FirstOrDefault();
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

		private void DataGrid_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.ITEMS_SOURCE_CHANGED, this.DataGrid.ItemsSource);
			this.SendNotification(Notifications.ITEMS_CHANGED, this.DataGrid.Items);
		}

		private void DataGrid_CurrentItemChanged(object sender, DependencyPropertyChangedEventArgs e)
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
			this.HandleCurrentItem(e.Key);
			this.HandleSelection(e.Key);
		}

		private void HandleCurrentItem(Key key)
		{
			switch (key)
			{
				case Key.Up:
					this.SendNotification(Notifications.CURRENT_ITEM_UP);
					break;
				case Key.Down:
					this.SendNotification(Notifications.CURRENT_ITEM_DOWN);
					break;
				case Key.PageUp:
					this.SendNotification(Notifications.CURRENT_ITEM_TO_POSITION,
					                      this.GetFirstItemOnCurrentPage(this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem) - 1, false));
					break;
				case Key.PageDown:
					this.SendNotification(Notifications.CURRENT_ITEM_TO_POSITION,
					                      this.GetFirstItemOnCurrentPage(this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem), true));
					break;
				case Key.Home:
					this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.Items.First());
					break;
				case Key.End:
					this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.Items.Last());
					break;
			}
		}

		private void HandleSelection(Key key)
		{
			bool control = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None;

			switch (key)
			{
				case Key.Up:
				case Key.Down:
				case Key.PageUp:
				case Key.PageDown:
				case Key.Home:
				case Key.End:
					switch (this.DataGrid.SelectionMode)
					{
						case SelectionMode.Single:
							if (!control)
							{
								this.SendNotification(Notifications.SELECTING_ITEMS, this.DataGrid.CurrentItem);
							}
							break;
						case SelectionMode.Extended:
							if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
							{
								if (!control)
								{
									this.SendNotification(Notifications.SELECTING_ITEMS, this.DataGrid.CurrentItem,
														  NotificationTypes.CLEAR_SELECTION);
								}
							}
							else
							{
								this.SendNotification(Notifications.SELECT_RANGE,
													  this.DataGrid.CurrentItem, control ? null : NotificationTypes.CLEAR_SELECTION);
							}
							break;
					}
					break;
				case Key.Space:
					if (control)
					{
						bool currentItemSelected = this.DataGrid.SelectedItems.Contains(this.DataGrid.CurrentItem);
						this.SendNotification(currentItemSelected ? Notifications.DESELECTING_ITEMS : Notifications.SELECTING_ITEMS,
						                      this.DataGrid.CurrentItem);
					}
					break;
				case Key.A:
					if (control && this.DataGrid.SelectionMode == SelectionMode.Extended)
					{
						this.SendNotification(Notifications.SELECT_ALL);
					}
					break;
			}
		}

		private int GetFirstItemOnCurrentPage(int startingIndex, bool forward)
		{
			int step = forward ? 1 : -1;
			int firstItemIndex = -1;
			ScrollViewer scroll = this.DataGrid.GetScrollHost();
			Panel itemsHost = this.DataGrid.GetItemsHost();
			Orientation orientation = Orientation.Vertical;
			if (itemsHost is StackPanel)
			{
				orientation = ((StackPanel) itemsHost).Orientation;
			}
			if (itemsHost is VirtualizingStackPanel)
			{
				orientation = ((VirtualizingStackPanel) itemsHost).Orientation;
			}
			int index = startingIndex;
			while (0 <= index && index < this.DataGrid.Items.Count &&
			       !this.IsOnCurrentPage(index, scroll, orientation))
			{
				firstItemIndex = index;
				index += step;
			}
			while (0 <= index && index < this.DataGrid.Items.Count &&
			       this.IsOnCurrentPage(index, scroll, orientation))
			{
				firstItemIndex = index;
				index += step;
			}
			return firstItemIndex >= 0 ? firstItemIndex : 0;
		}

		private bool IsOnCurrentPage(int index, FrameworkElement scroll, Orientation orientation)
		{
			if (scroll == null)
			{
				return true;
			}
			Rect scrollRect = new Rect(0.0, 0.0, scroll.ActualWidth, scroll.ActualHeight);
			FrameworkElement item = (FrameworkElement) this.DataGrid.ItemContainerGenerator.ContainerFromIndex(index);
			if (item == null)
			{
				return false;
			}
			GeneralTransform transform = item.TransformToVisual(scroll);
			Rect itemRect = new Rect(transform.Transform(new Point()),
			                         transform.Transform(new Point(item.ActualWidth, item.ActualHeight)));
			if (orientation == Orientation.Horizontal)
			{
				return (scrollRect.Left <= itemRect.Left && itemRect.Right <= scrollRect.Right);
			}
			return (scrollRect.Top <= itemRect.Top && itemRect.Bottom <= scrollRect.Bottom);
		}
	}
}