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
// File:	DataGridController.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Guidgets.XamlGrid.Core;
using Guidgets.XamlGrid.Models;
using Guidgets.XamlGrid.Models.Export;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller{T}"/> which is responsible for the functionality of a <see cref="Views.DataGrid"/>.
	/// </summary>
	public class DataGridController : Controller<DataGrid>
	{
		private Panel itemsHost;
		private bool continuousEditing;
		private bool fromFocusedCell;
		private Size viewportSize;
		private Size availableSize;
		private Key pressedKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataGridController"/> class.
		/// </summary>
		/// <param name="dataGrid">The view component.</param>
		public DataGridController(DataGrid dataGrid) : base(dataGrid.GetHashCode().ToString(), dataGrid)
		{

		}


		private Panel ItemsHost
		{
			get
			{
				return this.itemsHost ?? (this.itemsHost = this.View.GetItemsHost());
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.View.ViewportSizeChanged += this.DataGrid_ViewportSizeChanged;
			this.View.GotFocus += this.DataGrid_GotFocus;
			this.View.KeyDown += this.DataGrid_KeyDown;
			this.View.DataSourceChanged += this.DataGrid_DataSourceChanged;
			this.View.ItemTypeChanged += this.DataGrid_ItemTypeChanged;
			this.View.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;
			this.View.CurrentItemChanged += this.DataGrid_CurrentItemChanged;
			this.View.CurrentColumnChanged += this.DataGrid_CurrentColumnChanged;
			this.View.SelectionModeChanged += this.DataGrid_SelectionModeChanged;
            this.View.BringingIntoView += this.DataGrid_BringingIntoView;
			((INotifyCollectionChanged) this.View.Items).CollectionChanged += this.DataGridItems_CollectionChanged;
			this.View.Columns.CollectionChanged += this.DataGridColumns_CollectionChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller{T}"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.View.ViewportSizeChanged -= this.DataGrid_ViewportSizeChanged;
			this.View.GotFocus -= this.DataGrid_GotFocus;
			this.View.KeyDown -= this.DataGrid_KeyDown;
			this.View.DataSourceChanged -= this.DataGrid_DataSourceChanged;
			this.View.ItemTypeChanged -= this.DataGrid_ItemTypeChanged;
			this.View.ItemsSourceChanged -= this.DataGrid_ItemsSourceChanged;
			this.View.CurrentItemChanged -= this.DataGrid_CurrentItemChanged;
			this.View.CurrentColumnChanged -= this.DataGrid_CurrentColumnChanged;
			this.View.SelectionModeChanged -= this.DataGrid_SelectionModeChanged;
		    this.View.BringingIntoView -= this.DataGrid_BringingIntoView;
			((INotifyCollectionChanged) this.View.Items).CollectionChanged -= this.DataGridItems_CollectionChanged;
			this.View.Columns.CollectionChanged -= this.DataGridColumns_CollectionChanged;
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
						Notifications.NewItemCustom,
						Notifications.AvailableSizeChanged
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
			base.HandleNotification(notification);
			switch (notification.Code)
			{
				case Notifications.DataWrapped:
					this.View.ItemsSource = (IEnumerable) notification.Body;
					break;
				case Notifications.CurrentItemChanged:
					this.View.CurrentItem = notification.Body;
					if (this.View.HasFocus())
					{
						this.SendNotification(Notifications.FocusCell, new[] { this.View.CurrentItem, this.View.CurrentColumn });						
					}
					break;
				case Notifications.SelectionModeChanged:
					this.View.SelectionMode = (SelectionMode) notification.Body;
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
					this.SendNotification(Notifications.CurrentColumnChanged, this.View.CurrentColumn);
					break;
				case Notifications.CellFocused:
					Cell cell = (Cell) notification.Body;
					this.fromFocusedCell = true;
					this.View.CurrentColumn = cell.Column;
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
					this.View.OnNewItemAdding((NewItemEventArgs) notification.Body);
					break;
				case Notifications.AvailableSizeChanged:
					availableSize = (Size) notification.Body;
					this.View.Columns.CalculateRelativeWidths(((Size) notification.Body).Width);
					break;
			}
		}


		private void DataGrid_ViewportSizeChanged(object sender, CustomSizeChangedEventArgs e)
		{
			if (e.NewSize.Width != e.OldSize.Width)
			{
				this.viewportSize = e.NewSize;
			}
		}

		private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource == this.View)
			{
				this.SendNotification(Notifications.FocusCell, new[] { this.View.CurrentItem, this.View.CurrentColumn });
			}
		}

		private void DataGrid_LayoutUpdated(object sender, EventArgs e)
		{
			this.View.LayoutUpdated -= this.DataGrid_LayoutUpdated;
			Column columnToFocus = this.View.CurrentColumn ?? this.View.Columns.FirstOrDefault();
			this.View.CurrentColumn = null;
			this.View.CurrentColumn = columnToFocus;
		}

		private void DataGrid_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.Modifiers == KeyHelper.CommandModifier && e.Key == Key.C)
			{
				this.View.Copy();
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
			this.SendNotification(Notifications.ItemTypeChanged, this.View.ItemType);
		}

		private void DataGrid_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.View.ItemType = this.View.ItemsSource != null ? this.View.ItemsSource.GetElementType() : null;
			this.SendNotification(Notifications.ItemsSourceChanged, this.View.ItemsSource);
			this.SendNotification(Notifications.ItemsChanged, this.View.Items);
		}

		private void DataGrid_CurrentItemChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CurrentItemChanging, this.View.CurrentItem);
		}

		private void DataGrid_CurrentColumnChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.View.CurrentColumn != null && !this.fromFocusedCell && this.View.HasFocus())
			{
				this.SendNotification(Notifications.FocusCell, new[] { this.View.CurrentItem, this.View.CurrentColumn });
			}
		}

		private void DataGrid_SelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.SelectionModeChanging, this.View.SelectionMode);
		}

	    private void DataGrid_BringingIntoView (object sender, ScrollEventArgs e)
        {
            this.SendNotification (Notifications.ScrollIntoView, e.NewValue);
        }

		private void DataGridItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SendNotification(Notifications.ItemsCollectionChanged, e);
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.View.LayoutUpdated += this.DataGrid_LayoutUpdated;
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
						                             new Binding("ColumnWidth") { Source = this.View });
					}
					if (column.ReadLocalValue(Column.IsResizableProperty) == DependencyProperty.UnsetValue)
					{
						BindingOperations.SetBinding(column, Column.IsResizableProperty,
						                             new Binding("ResizableColumns") { Source = this.View });
					}
					if (column.ReadLocalValue(Column.IsReadOnlyProperty) == DependencyProperty.UnsetValue)
					{
						BindingOperations.SetBinding(column, Column.IsReadOnlyProperty,
						                             new Binding("IsReadOnly") { Source = this.View });
					}
					column.IsSelected = true;
					column.ActualWidthChanged += this.Column_WidthAffected;
					column.VisibilityChanged += this.Column_WidthAffected;
					if (this.View.Columns.Count == 1 && this.View.CurrentColumn == null)
					{
						this.View.LayoutUpdated += this.DataGrid_LayoutUpdated;
					}
				}
			}
			if (e.OldItems != null)
			{
				foreach (Column column in e.OldItems)
				{
					column.ClearValue(Column.WidthProperty);
					column.ClearValue(Column.IsResizableProperty);
					column.ClearValue(Column.IsReadOnlyProperty);
					column.ActualWidthChanged -= this.Column_WidthAffected;
					column.VisibilityChanged -= this.Column_WidthAffected;
					if (this.View.CurrentColumn == column)
					{
						this.View.CurrentColumn = this.View.Columns.FirstOrDefault();
					}
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.View.CurrentColumn = null;
			}
			if (this.View.Columns.Any(column => column.Width.SizeMode == SizeMode.Fill))
			{
				this.View.Columns.CalculateRelativeWidths(this.availableSize.Width);
			}
			this.SendNotification(Notifications.ColumnsChanged, e);
		}

		private void Column_WidthAffected(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.View.Columns.CalculateRelativeWidths(this.availableSize.Width);
		}

		protected virtual void HandleCurrentItem(Key key)
		{
			bool control = (Keyboard.Modifiers & KeyHelper.CommandModifier) != ModifierKeys.None;

			switch (key)
			{
				case Key.Left:
					if (control)
					{
						this.View.CurrentColumn = this.View.Columns.FirstOrDefault();
					}
					break;
				case Key.Right:
					if (control)
					{
						this.View.CurrentColumn = this.View.Columns.LastOrDefault();
					}
					break;
				case Key.Up:
					this.SendNotification(Notifications.CurrentItemUp);
					break;
				case Key.Down:
					this.SendNotification(Notifications.CurrentItemDown);
					break;
				case Key.PageUp:
					int pageUp = this.View.Items.IndexOf(this.View.CurrentItem) - this.GetPageSize();
					this.SendNotification(Notifications.CurrentItemToPosition, Math.Max(pageUp, 0));
					break;
				case Key.PageDown:
					// make sure the new page is scrolled to before any MakeVisible is called
					if (this.View.CurrentItem != this.View.Items.Last())
					{
						this.View.LayoutUpdated += this.DataGrid_CurrentItemLayoutUpdated;
					}
					break;
				case Key.Home:
					this.SendNotification(Notifications.CurrentItemChanging, this.View.Items.First());
					if (control)
					{
						this.View.CurrentColumn = this.View.Columns.FirstOrDefault();
					}
					break;
				case Key.End:
					this.SendNotification(Notifications.CurrentItemChanging, this.View.Items.Last());
					if (control)
					{
						this.View.CurrentColumn = this.View.Columns.LastOrDefault();
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
					this.SelectItems(this.View.CurrentItem, false, key);
					break;
				case Key.PageDown:
					// make sure the new page is scrolled to before any MakeVisible is called
					if (this.View.CurrentItem != this.View.Items.Last())
					{
						pressedKey = key;
						this.View.LayoutUpdated += this.DataGrid_SelectionLayoutUpdated;	
					}
					break;
				case Key.Space:
					if (control)
					{
						bool currentItemSelected = this.View.SelectedItems.IsSelected(this.View.CurrentItem);
						this.SendNotification(currentItemSelected ? Notifications.DeselectingItems : Notifications.SelectingItems,
						                      this.View.CurrentItem);
					}
					break;
				case Key.A:
					if (control && this.View.SelectionMode == SelectionMode.Extended)
					{
						this.SendNotification(Notifications.SelectAll);
					}
					break;
			}
		}

		private void DataGrid_CurrentItemLayoutUpdated(object sender, EventArgs e)
		{
			this.View.LayoutUpdated -= this.DataGrid_CurrentItemLayoutUpdated;
			int pageDown = this.View.Items.IndexOf(this.View.CurrentItem) + this.GetPageSize();
			this.SendNotification(Notifications.CurrentItemToPosition, Math.Min(pageDown, this.View.Items.Count - 1));
		}

		private void DataGrid_SelectionLayoutUpdated(object sender, EventArgs e)
		{
			this.View.LayoutUpdated -= this.DataGrid_SelectionLayoutUpdated;
			this.SelectItems(this.View.CurrentItem, false, pressedKey);
		}

		private void SelectItems(object itemToSelect, bool clicked, Key key)
		{
			bool selected = this.View.SelectedItems.IsSelected(itemToSelect);
			int notificationToSend = selected ? Notifications.DeselectingItems : Notifications.SelectingItems;
			switch (this.View.SelectionMode)
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
