using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Company.Widgets.Core;
using Company.Widgets.Models;
using Company.Widgets.Models.Export;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.DataGrid"/>.
	/// </summary>
	public class DataGridController : Controller
	{
		private Panel itemsHost;
		private bool continuousEditing;
		private bool fromFocusedCell;
		private Size viewportSize;
		private Key pressedKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataGridController"/> class.
		/// </summary>
		/// <param name="dataGrid">The view component.</param>
		public DataGridController(DataGrid dataGrid) : base(dataGrid.GetHashCode().ToString(), dataGrid)
		{

		}


		/// <summary>
		/// Gets the <see cref="Views.DataGrid"/> for which functionality the <see cref="DataGridController"/> is responsible.
		/// </summary>
		public virtual DataGrid DataGrid
		{
			get
			{
				return (DataGrid) this.ViewComponent;
			}
		}


		private Panel ItemsHost
		{
			get
			{
				return this.itemsHost ?? (this.itemsHost = this.DataGrid.GetItemsHost());
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.DataGrid.ViewportSizeChanged += this.DataGrid_ViewportSizeChanged;
			this.DataGrid.GotFocus += this.DataGrid_GotFocus;
			this.DataGrid.KeyDown += this.DataGrid_KeyDown;
			this.DataGrid.DataSourceChanged += this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemTypeChanged += this.DataGrid_ItemTypeChanged;
			this.DataGrid.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;
			this.DataGrid.CurrentItemChanged += this.DataGrid_CurrentItemChanged;
			this.DataGrid.CurrentColumnChanged += this.DataGrid_CurrentColumnChanged;
			this.DataGrid.SelectionModeChanged += this.DataGrid_SelectionModeChanged;
			((INotifyCollectionChanged) this.DataGrid.Items).CollectionChanged += this.DataGridItems_CollectionChanged;
			this.DataGrid.Columns.CollectionChanged += this.DataGridColumns_CollectionChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.DataGrid.ViewportSizeChanged -= this.DataGrid_ViewportSizeChanged;
			this.DataGrid.GotFocus -= this.DataGrid_GotFocus;
			this.DataGrid.KeyDown -= this.DataGrid_KeyDown;
			this.DataGrid.DataSourceChanged -= this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemTypeChanged -= this.DataGrid_ItemTypeChanged;
			this.DataGrid.ItemsSourceChanged -= this.DataGrid_ItemsSourceChanged;
			this.DataGrid.CurrentItemChanged -= this.DataGrid_CurrentItemChanged;
			this.DataGrid.CurrentColumnChanged -= this.DataGrid_CurrentColumnChanged;
			this.DataGrid.SelectionModeChanged -= this.DataGrid_SelectionModeChanged;
			((INotifyCollectionChanged) this.DataGrid.Items).CollectionChanged -= this.DataGridItems_CollectionChanged;
			this.DataGrid.Columns.CollectionChanged -= this.DataGridColumns_CollectionChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int>
			       	{
			       		Notifications.DataWrapped,
			       		Notifications.CurrentItemChanged,
			       		Notifications.SelectionModeChanged,
						Notifications.ItemKeyDown,
						Notifications.ItemClicked,
						Notifications.CellFocused,
						Notifications.CellEditModeChanged,
						Notifications.CellEditingCancelled,
						Notifications.IsColumnCurrent,
						Notifications.NewItemCustom
			       	};
		}

		/// <summary>
		/// Handle <c>INotification</c>s
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Code)
			{
				case Notifications.DataWrapped:
					this.DataGrid.ItemsSource = (IEnumerable) notification.Body;
					break;
				case Notifications.CurrentItemChanged:
					this.DataGrid.CurrentItem = notification.Body;
					if (this.DataGrid.HasFocus())
					{
						this.SendNotification(Notifications.FocusCell, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });						
					}
					break;
				case Notifications.SelectionModeChanged:
					this.DataGrid.SelectionMode = (SelectionMode) notification.Body;
					break;
				case Notifications.ItemKeyDown:
					KeyEventArgs e = (KeyEventArgs) notification.Body;
					this.HandleCurrentItem(e.Key);
					this.HandleSelection(e.Key);
					break;
				case Notifications.ItemClicked:
					this.SelectItems(notification.Body, true, Key.None);
					break;
				case Notifications.IsColumnCurrent:
					this.SendNotification(Notifications.CurrentColumnChanged, this.DataGrid.CurrentColumn);
					break;
				case Notifications.CellFocused:
					Cell cell = (Cell) notification.Body;
					this.fromFocusedCell = true;
					this.DataGrid.CurrentColumn = cell.Column;
					this.fromFocusedCell = false;
					// TODO: this should be replaced by sending a notification because it tightens the coupling
					cell.IsInEditMode = continuousEditing;
					break;
				case Notifications.CellEditModeChanged:
					if ((bool) notification.Body)
					{
						continuousEditing = true;
					}
					break;
				case Notifications.CellEditingCancelled:
					continuousEditing = false;
					break;
				case Notifications.NewItemCustom:
					this.DataGrid.OnNewItemAdding((NewItemEventArgs) notification.Body);
					break;
			}
		}


		private void DataGrid_ViewportSizeChanged(object sender, CustomSizeChangedEventArgs e)
		{
			if (e.NewSize.Width != e.OldSize.Width)
			{
				this.viewportSize = e.NewSize;
				this.DataGrid.Columns.CalculateRelativeWidths(this.viewportSize.Width);
			}
		}

		private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource == this.DataGrid)
			{
				this.SendNotification(Notifications.FocusCell, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });
			}
		}

		private void DataGrid_LayoutUpdated(object sender, EventArgs e)
		{
			this.DataGrid.LayoutUpdated -= this.DataGrid_LayoutUpdated;
			Column columnToFocus = this.DataGrid.CurrentColumn ?? this.DataGrid.Columns.FirstOrDefault();
			this.DataGrid.CurrentColumn = null;
			this.DataGrid.CurrentColumn = columnToFocus;
		}

		private void DataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.Modifiers == KeyHelper.CommandModifier && e.Key == Key.C)
			{
				this.DataGrid.Copy();
			}
		}

		private void DataGrid_DataSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null || e.NewValue is IEnumerable)
			{
				this.SendNotification(Notifications.DataSourceChanged, e.NewValue);
			}
		}

		private void DataGrid_ItemTypeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.ItemTypeChanged, this.DataGrid.ItemType);
		}

		private void DataGrid_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.DataGrid.ItemType = this.DataGrid.ItemsSource != null ? this.DataGrid.ItemsSource.GetElementType() : null;
			this.SendNotification(Notifications.ItemsSourceChanged, this.DataGrid.ItemsSource);
			this.SendNotification(Notifications.ItemsChanged, this.DataGrid.Items);
		}

		private void DataGrid_CurrentItemChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CurrentItemChanging, this.DataGrid.CurrentItem);
		}

		private void DataGrid_CurrentColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.DataGrid.CurrentColumn != null && !this.fromFocusedCell && this.DataGrid.HasFocus())
			{
				this.SendNotification(Notifications.FocusCell, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });
			}
		}

		private void DataGrid_SelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.SelectionModeChanging, this.DataGrid.SelectionMode);
		}

		private void DataGridItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SendNotification(Notifications.ItemsCollectionChanged, e);
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.DataGrid.LayoutUpdated += this.DataGrid_LayoutUpdated;
			}
		}

		private void DataGridColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (Column column in e.NewItems)
				{
					if (column.ReadLocalValue(Column.WidthProperty) == DependencyProperty.UnsetValue)
					{
						BindingOperations.SetBinding(column, Column.WidthProperty,
						                             new Binding("ColumnWidth") { Source = this.DataGrid });
					}
					if (column.ReadLocalValue(Column.IsResizableProperty) == DependencyProperty.UnsetValue)
					{
						BindingOperations.SetBinding(column, Column.IsResizableProperty,
						                             new Binding("ResizableColumns") { Source = this.DataGrid });
					}
					if (column.ReadLocalValue(Column.IsEditableProperty) == DependencyProperty.UnsetValue)
					{
						BindingOperations.SetBinding(column, Column.IsEditableProperty,
						                             new Binding("IsEditable") { Source = this.DataGrid });
					}
					column.IsSelected = true;
					column.ActualWidthChanged += this.Column_WidthAffected;
					column.VisibilityChanged += this.Column_WidthAffected;
					if (this.DataGrid.Columns.Count == 1 && this.DataGrid.CurrentColumn == null)
					{
						this.DataGrid.LayoutUpdated += this.DataGrid_LayoutUpdated;
					}
				}
			}
			if (e.OldItems != null)
			{
				foreach (Column column in e.OldItems)
				{
					column.ClearValue(Column.WidthProperty);
					column.ClearValue(Column.IsResizableProperty);
					column.ClearValue(Column.IsEditableProperty);
					column.ActualWidthChanged -= this.Column_WidthAffected;
					column.VisibilityChanged -= this.Column_WidthAffected;
					if (this.DataGrid.CurrentColumn == column)
					{
						this.DataGrid.CurrentColumn = this.DataGrid.Columns.FirstOrDefault();
					}
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.DataGrid.CurrentColumn = null;
			}
			if (this.DataGrid.Columns.Any(column => column.Width.SizeMode == SizeMode.Fill))
			{
				this.DataGrid.Columns.CalculateRelativeWidths(this.viewportSize.Width);
			}
			this.SendNotification(Notifications.ColumnsChanged, e);
		}

		private void Column_WidthAffected(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.DataGrid.Columns.CalculateRelativeWidths(this.viewportSize.Width);
		}

		protected virtual void HandleCurrentItem(Key key)
		{
			bool control = (Keyboard.Modifiers & KeyHelper.CommandModifier) != ModifierKeys.None;

			switch (key)
			{
				case Key.Left:
					if (control)
					{
						this.DataGrid.CurrentColumn = this.DataGrid.Columns.FirstOrDefault();
					}
					break;
				case Key.Right:
					if (control)
					{
						this.DataGrid.CurrentColumn = this.DataGrid.Columns.LastOrDefault();
					}
					break;
				case Key.Up:
					this.SendNotification(Notifications.CurrentItemUp);
					break;
				case Key.Down:
					this.SendNotification(Notifications.CurrentItemDown);
					break;
				case Key.PageUp:
					int pageUp = this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem) - this.GetPageSize();
					this.SendNotification(Notifications.CurrentItemToPosition, Math.Max(pageUp, 0));
					break;
				case Key.PageDown:
					// make sure the new page is scrolled to before any MakeVisible is called
					if (this.DataGrid.CurrentItem != this.DataGrid.Items.Last())
					{
						this.DataGrid.LayoutUpdated += this.DataGrid_CurrentItemLayoutUpdated;
					}
					break;
				case Key.Home:
					this.SendNotification(Notifications.CurrentItemChanging, this.DataGrid.Items.First());
					if (control)
					{
						this.DataGrid.CurrentColumn = this.DataGrid.Columns.FirstOrDefault();
					}
					break;
				case Key.End:
					this.SendNotification(Notifications.CurrentItemChanging, this.DataGrid.Items.Last());
					if (control)
					{
						this.DataGrid.CurrentColumn = this.DataGrid.Columns.LastOrDefault();
					}
					break;
			}
		}

		protected virtual void HandleSelection(Key key)
		{
			bool control = (Keyboard.Modifiers & KeyHelper.CommandModifier) != ModifierKeys.None;

			switch (key)
			{
				case Key.Up:
				case Key.Down:
				case Key.PageUp:
				case Key.Home:
				case Key.End:
					this.SelectItems(this.DataGrid.CurrentItem, false, key);
					break;
				case Key.PageDown:
					// make sure the new page is scrolled to before any MakeVisible is called
					if (this.DataGrid.CurrentItem != this.DataGrid.Items.Last())
					{
						pressedKey = key;
						this.DataGrid.LayoutUpdated += this.DataGrid_SelectionLayoutUpdated;	
					}
					break;
				case Key.Space:
					if (control)
					{
						bool currentItemSelected = this.DataGrid.SelectedItems.IsSelected(this.DataGrid.CurrentItem);
						this.SendNotification(currentItemSelected ? Notifications.DeselectingItems : Notifications.SelectingItems,
						                      this.DataGrid.CurrentItem);
					}
					break;
				case Key.A:
					if (control && this.DataGrid.SelectionMode == SelectionMode.Extended)
					{
						this.SendNotification(Notifications.SelectAll);
					}
					break;
			}
		}

		private void DataGrid_CurrentItemLayoutUpdated(object sender, EventArgs e)
		{
			this.DataGrid.LayoutUpdated -= this.DataGrid_CurrentItemLayoutUpdated;
			int pageDown = this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem) + this.GetPageSize();
			this.SendNotification(Notifications.CurrentItemToPosition, Math.Min(pageDown, this.DataGrid.Items.Count - 1));
		}

		private void DataGrid_SelectionLayoutUpdated(object sender, EventArgs e)
		{
			this.DataGrid.LayoutUpdated -= this.DataGrid_SelectionLayoutUpdated;
			this.SelectItems(this.DataGrid.CurrentItem, false, pressedKey);
		}

		private void SelectItems(object itemToSelect, bool clicked, Key key)
		{
			bool selected = this.DataGrid.SelectedItems.IsSelected(itemToSelect);
			int notificationToSend = selected ? Notifications.DeselectingItems : Notifications.SelectingItems;
			switch (this.DataGrid.SelectionMode)
			{
				case SelectionMode.Single:
					if (!selected || (Keyboard.Modifiers & KeyHelper.CommandModifier) != ModifierKeys.None)
					{
						this.SendNotification(notificationToSend, itemToSelect);
					}
					break;
				case SelectionMode.Multiple:
					this.ToggleSelection(notificationToSend, itemToSelect, clicked, key);
					break;
				case SelectionMode.Extended:
					switch (Keyboard.Modifiers)
					{
						case ModifierKeys.None:
							this.SendNotification(Notifications.SelectingItems, itemToSelect,
							                      NotificationTypes.ClearSelection);
							break;
						case ModifierKeys.Shift:
							this.SendNotification(Notifications.SelectRange, itemToSelect,
												  NotificationTypes.ClearSelection);
							break;
					}
					if (Keyboard.Modifiers == KeyHelper.CommandModifier)
					{
						this.ToggleSelection(notificationToSend, itemToSelect, clicked, key);
					}
					if (Keyboard.Modifiers == (KeyHelper.CommandModifier | ModifierKeys.Shift))
					{
						this.SendNotification(Notifications.SelectRange, itemToSelect);
					}
					break;
			}
		}

		private void ToggleSelection(int notificationToSend, object itemToSelect, bool clickedItem, Key key)
		{
			if (clickedItem)
			{
				this.SendNotification(notificationToSend, itemToSelect);
			}
			else
			{
				if (key == Key.Home || key == Key.End)
				{
					this.SendNotification(Notifications.SelectingItems, itemToSelect,
					                      NotificationTypes.ClearSelection);
				}
			}
		}

		private int GetPageSize()
		{
			int page = 0;
			switch (this.GetOrientation())
			{
				case Orientation.Vertical:
					page = (int) this.viewportSize.Height;
					break;
				case Orientation.Horizontal:
					page = (int) this.viewportSize.Width;
					break;
			}
			return page;
		}

		private Orientation GetOrientation()
		{
			Orientation orientation = Orientation.Vertical;
			StackPanel stackPanel = this.ItemsHost as StackPanel;
			if (stackPanel != null)
			{
				orientation = stackPanel.Orientation;
			}
			VirtualizingStackPanel virtualizingStackPanel = this.ItemsHost as VirtualizingStackPanel;
			if (virtualizingStackPanel != null)
			{
				orientation = virtualizingStackPanel.Orientation;
			}
			return orientation;
		}
	}
}
