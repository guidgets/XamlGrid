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

			this.DataGrid.ViewportSizeChanged += DataGrid_ViewportSizeChanged;
			this.DataGrid.GotFocus += DataGrid_GotFocus;
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
			       		Notifications.DATA_WRAPPED,
			       		Notifications.CURRENT_ITEM_CHANGED,
			       		Notifications.SELECTION_MODE_CHANGED,
						Notifications.ITEM_KEY_DOWN,
						Notifications.ITEM_CLICKED,
						Notifications.CELL_FOCUSED,
						Notifications.CELL_EDIT_MODE_CHANGED,
						Notifications.CELL_EDITING_CANCELLED,
						Notifications.IS_COLUMN_CURRENT,
						Notifications.NEW_ITEM_CUSTOM
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
				case Notifications.DATA_WRAPPED:
					this.DataGrid.ItemsSource = (IEnumerable) notification.Body;
					break;
				case Notifications.CURRENT_ITEM_CHANGED:
					this.DataGrid.CurrentItem = notification.Body;
					if (this.DataGrid.HasFocus())
					{
						this.SendNotification(Notifications.FOCUS_CELL, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });						
					}
					break;
				case Notifications.SELECTION_MODE_CHANGED:
					this.DataGrid.SelectionMode = (SelectionMode) notification.Body;
					break;
				case Notifications.ITEM_KEY_DOWN:
					KeyEventArgs e = (KeyEventArgs) notification.Body;
					this.HandleCurrentItem(e.Key);
					this.HandleSelection(e.Key);
					break;
				case Notifications.ITEM_CLICKED:
					this.SelectItems(notification.Body, true, Key.None);
					break;
				case Notifications.IS_COLUMN_CURRENT:
					this.SendNotification(Notifications.CURRENT_COLUMN_CHANGED, this.DataGrid.CurrentColumn);
					break;
				case Notifications.CELL_FOCUSED:
					Cell cell = (Cell) notification.Body;
					this.fromFocusedCell = true;
					this.DataGrid.CurrentColumn = cell.Column;
					this.fromFocusedCell = false;
					// TODO: this should be replaced by sending a notification because it tightens the coupling
					cell.IsInEditMode = continuousEditing;
					break;
				case Notifications.CELL_EDIT_MODE_CHANGED:
					if ((bool) notification.Body)
					{
						continuousEditing = true;
					}
					break;
				case Notifications.CELL_EDITING_CANCELLED:
					continuousEditing = false;
					break;
				case Notifications.NEW_ITEM_CUSTOM:
					this.DataGrid.OnNewItemAdding((NewItemEventArgs) notification.Body);
					break;
			}
		}


		private void DataGrid_ViewportSizeChanged(object sender, CustomSizeChangedEventArgs e)
		{
			if (e.NewSize.Width != e.OldSize.Width)
			{
				this.viewportSize = e.NewSize;
				this.CalculateRelativeColumnWidths();
			}
		}

		private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource == this.DataGrid)
			{
				this.SendNotification(Notifications.FOCUS_CELL, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });
			}
		}

		private void DataGrid_LayoutUpdated(object sender, EventArgs e)
		{
			Column columnToFocus = this.DataGrid.CurrentColumn ?? this.DataGrid.Columns.FirstOrDefault();
			this.DataGrid.CurrentColumn = null;
			this.DataGrid.CurrentColumn = columnToFocus;
			this.DataGrid.LayoutUpdated -= this.DataGrid_LayoutUpdated;
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
				this.SendNotification(Notifications.DATA_SOURCE_CHANGED, e.NewValue);
			}
		}

		private void DataGrid_ItemTypeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.ITEM_TYPE_CHANGED, this.DataGrid.ItemType);
		}

		private void DataGrid_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.DataGrid.ItemType = this.DataGrid.ItemsSource != null ? this.DataGrid.ItemsSource.GetElementType() : null;
			this.SendNotification(Notifications.ITEMS_SOURCE_CHANGED, this.DataGrid.ItemsSource);
			this.SendNotification(Notifications.ITEMS_CHANGED, this.DataGrid.Items);
		}

		private void DataGrid_CurrentItemChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.CurrentItem);
		}

		private void DataGrid_CurrentColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.DataGrid.CurrentColumn != null && !this.fromFocusedCell && this.DataGrid.HasFocus())
			{
				this.SendNotification(Notifications.FOCUS_CELL, new[] { this.DataGrid.CurrentItem, this.DataGrid.CurrentColumn });
			}
		}

		private void DataGrid_SelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.SELECTION_MODE_CHANGING, this.DataGrid.SelectionMode);
		}

		private void DataGridItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SendNotification(Notifications.ITEMS_COLLECTION_CHANGED, e);
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
				this.CalculateRelativeColumnWidths();
			}
			this.SendNotification(Notifications.COLUMNS_CHANGED, e);
		}

		private void Column_WidthAffected(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.CalculateRelativeColumnWidths();
		}

		private void CalculateRelativeColumnWidths()
		{
			if (this.DataGrid.Columns.Any(column => double.IsNaN(column.ActualWidth)))
			{
				return;
			}
			IEnumerable<Column> relativeColumns = from column in this.DataGrid.Columns
			                                      where column.Visibility == Visibility.Visible &&
			                                            column.Width.SizeMode == SizeMode.Fill
			                                      select column;
			double stars = relativeColumns.Sum(column => column.Width.Value);
			double availableWidth = this.viewportSize.Width - (from column in this.DataGrid.Columns
			                                                   where column.Visibility == Visibility.Visible &&
			                                                         column.Width.SizeMode != SizeMode.Fill
			                                                   select column.ActualWidth).Sum();
			foreach (Column column in relativeColumns.Skip(1))
			{
				double width = Math.Floor(column.Width.Value * availableWidth / stars);
				column.ActualWidth = Math.Max(width, 1);
			}
			Column firstColumn = relativeColumns.FirstOrDefault();
			if (firstColumn != null)
			{
				double width = this.viewportSize.Width - (from column in this.DataGrid.Columns
				                                          where column.Visibility == Visibility.Visible &&
				                                                column != firstColumn
				                                          select column.ActualWidth).Sum();
				firstColumn.ActualWidth = Math.Max(width, 1);
			}
		}

		protected virtual void HandleCurrentItem(Key key)
		{
			bool control = (Keyboard.Modifiers & KeyHelper.CommandModifier) != ModifierKeys.None;

			switch (key)
			{
				case Key.Up:
					this.SendNotification(Notifications.CURRENT_ITEM_UP);
					break;
				case Key.Down:
					this.SendNotification(Notifications.CURRENT_ITEM_DOWN);
					break;
				case Key.PageUp:
					int pageUp = this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem) - this.GetPageSize();
					this.SendNotification(Notifications.CURRENT_ITEM_TO_POSITION, Math.Max(pageUp, 0));
					break;
				case Key.PageDown:
					// make sure the new page is scrolled to before any MakeVisible is called
					if (this.DataGrid.CurrentItem != this.DataGrid.Items.Last())
					{
						this.DataGrid.LayoutUpdated += this.DataGrid_CurrentItemLayoutUpdated;
					}
					break;
				case Key.Home:
					if (control)
					{
						this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.Items.First());
					}
					this.DataGrid.CurrentColumn = this.DataGrid.Columns.FirstOrDefault();
					break;
				case Key.End:
					if (control)
					{
						this.SendNotification(Notifications.CURRENT_ITEM_CHANGING, this.DataGrid.Items.Last());
					}
					this.DataGrid.CurrentColumn = this.DataGrid.Columns.LastOrDefault();
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

		private void DataGrid_CurrentItemLayoutUpdated(object sender, EventArgs e)
		{
			int pageDown = this.DataGrid.Items.IndexOf(this.DataGrid.CurrentItem) + this.GetPageSize();
			this.SendNotification(Notifications.CURRENT_ITEM_TO_POSITION, Math.Min(pageDown, this.DataGrid.Items.Count - 1));
			this.DataGrid.LayoutUpdated -= this.DataGrid_CurrentItemLayoutUpdated;
		}

		private void DataGrid_SelectionLayoutUpdated(object sender, EventArgs e)
		{
			this.SelectItems(this.DataGrid.CurrentItem, false, pressedKey);
			this.DataGrid.LayoutUpdated -= this.DataGrid_SelectionLayoutUpdated;
		}

		private void SelectItems(object itemToSelect, bool clicked, Key key)
		{
			bool selected = this.DataGrid.SelectedItems.IsSelected(itemToSelect);
			int notificationToSend = selected ? Notifications.DESELECTING_ITEMS : Notifications.SELECTING_ITEMS;
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
							this.SendNotification(Notifications.SELECTING_ITEMS, itemToSelect,
							                      NotificationTypes.CLEAR_SELECTION);
							break;
						case ModifierKeys.Shift:
							this.SendNotification(Notifications.SELECT_RANGE, itemToSelect,
												  NotificationTypes.CLEAR_SELECTION);
							break;
					}
					if (Keyboard.Modifiers == KeyHelper.CommandModifier)
					{
						this.ToggleSelection(notificationToSend, itemToSelect, clicked, key);
					}
					if (Keyboard.Modifiers == (KeyHelper.CommandModifier | ModifierKeys.Shift))
					{
						this.SendNotification(Notifications.SELECT_RANGE, itemToSelect);
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
					this.SendNotification(Notifications.SELECTING_ITEMS, itemToSelect,
					                      NotificationTypes.CLEAR_SELECTION);
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
