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
using System.Windows.Media;
using Company.DataGrid.Core;
using Company.DataGrid.Models;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.DataGrid"/>.
	/// </summary>
	public class DataGridController : Controller
	{
		private ItemsPresenter itemsPresenter;


		/// <summary>
		/// Initializes a new instance of the <see cref="DataGridController"/> class.
		/// </summary>
		/// <param name="dataGrid">The view component.</param>
		public DataGridController(Views.DataGrid dataGrid) : base(dataGrid.GetHashCode().ToString(), dataGrid)
		{

		}


		/// <summary>
		/// Gets the <see cref="Views.DataGrid"/> for which functionality the <see cref="Controller"/> is responsible.
		/// </summary>
		public Views.DataGrid DataGrid
		{
			get
			{
				return (Views.DataGrid) this.ViewComponent;
			}
		}

		private ItemsPresenter ItemsPresenter
		{
			get
			{
				return this.itemsPresenter ?? (this.itemsPresenter = this.DataGrid.GetItemsPresenter());
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.DataGrid.Loaded += this.DataGrid_Loaded;
			this.DataGrid.DataSourceChanged += this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;
			this.DataGrid.CurrentItemChanged += this.DataGrid_CurrentItemChanged;
			this.DataGrid.SelectionModeChanged += this.DataGrid_SelectionModeChanged;
			this.DataGrid.Columns.CollectionChanged += this.Columns_CollectionChanged;
			this.DataGrid.ItemContainerGenerator.ItemsChanged += this.ItemContainerGenerator_ItemsChanged;
		}

		private void Scroll_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width != e.PreviousSize.Width)
			{
				this.CalculateRelativeColumnWidths();				
			}
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.DataGrid.Loaded -= this.DataGrid_Loaded;
			this.DataGrid.DataSourceChanged -= this.DataGrid_DataSourceChanged;
			this.DataGrid.ItemsSourceChanged -= this.DataGrid_ItemsSourceChanged;
			this.DataGrid.CurrentItemChanged -= this.DataGrid_CurrentItemChanged;
			this.DataGrid.SelectionModeChanged -= this.DataGrid_SelectionModeChanged;
			this.DataGrid.Columns.CollectionChanged -= this.Columns_CollectionChanged;
			this.DataGrid.ItemContainerGenerator.ItemsChanged -= this.ItemContainerGenerator_ItemsChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of
		/// </summary>
		/// <returns>The list of <c>INotification</c> names</returns>
		public override IList<string> ListNotificationInterests()
		{
			return new List<string>
			       	{
			       		Notifications.DATA_WRAPPED,
			       		Notifications.CURRENT_ITEM_CHANGED,
			       		Notifications.SELECTION_MODE_CHANGED,
						Notifications.ITEM_KEY_DOWN
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
				case Notifications.ITEM_KEY_DOWN:
					KeyEventArgs e = (KeyEventArgs) notification.Body;
					this.HandleCurrentItem(e.Key);
					this.HandleSelection(e.Key);
					break;
			}
		}


		private void DataGrid_Loaded(object sender, EventArgs eventArgs)
		{
			this.DataGrid.ApplyTemplate();
			this.ItemsPresenter.SizeChanged += this.Scroll_SizeChanged;
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
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Replace:
					foreach (Column column in e.NewItems)
					{
						if (column.ReadLocalValue(Column.WidthProperty) == DependencyProperty.UnsetValue)
						{
							BindingOperations.SetBinding(column, Column.WidthProperty,
														 new Binding("ColumnWidth") { Source = this.DataGrid });
						}
						if (column.ReadLocalValue(Column.ResizableProperty) == DependencyProperty.UnsetValue)
						{
							BindingOperations.SetBinding(column, Column.ResizableProperty,
														 new Binding("ResizableColumns") { Source = this.DataGrid });
						}
						if (column.ReadLocalValue(Column.IsEditableProperty) == DependencyProperty.UnsetValue)
						{
							BindingOperations.SetBinding(column, Column.IsEditableProperty,
														 new Binding("IsEditable") { Source = this.DataGrid });
						}
						column.Index = this.DataGrid.Columns.IndexOf(column);
						column.ActualWidthChanged += this.Column_ActualWidthChanged;
					}
					if ((from column in this.DataGrid.Columns
						 where column.Width.SizeMode == SizeMode.Fill
						 select column).Any())
					{
						this.CalculateRelativeColumnWidths();
					}
					break;
			}
			this.SendNotification(Notifications.COLUMNS_CHANGED, e);
		}

		private void Column_ActualWidthChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.CalculateRelativeColumnWidths();
		}

		private void CalculateRelativeColumnWidths()
		{
			if (this.ItemsPresenter == null || (from column in this.DataGrid.Columns
												where double.IsNaN(column.ActualWidth)
												select column).Any())
			{
				return;
			}
			IList<Column> relativeColumns = (from column in this.DataGrid.Columns
			                                 where column.Width.SizeMode == SizeMode.Fill
			                                 select column).ToList();
			double stars = relativeColumns.Sum(column => column.Width.Value);
			double availableWidth = this.ItemsPresenter.ActualWidth - (from column in this.DataGrid.Columns
			                                                           where column.Width.SizeMode != SizeMode.Fill
			                                                           select column.ActualWidth).Sum();
			for (int index = 0; index < relativeColumns.Count; index++)
			{
				Column currentColumn = relativeColumns[index];
				double width;
				if (index != relativeColumns.Count - 1)
				{
					width = Math.Floor(relativeColumns[index].Width.Value * availableWidth / stars) - 1;
				}
				else
				{
					width = this.itemsPresenter.ActualWidth - (from column in this.DataGrid.Columns
					                                           where column != currentColumn
					                                           select column.ActualWidth).Sum() - 2;
				}
				currentColumn.ActualWidth = Math.Max(width, 1);
			}
		}

		private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
		{
			this.SendNotification(Notifications.GENERATED_ITEMS_CHANGED, e);
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
			int firstItemIndex = 0;
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
				firstItemIndex = index += step;
			}
			while (0 <= index && index < this.DataGrid.Items.Count &&
			       this.IsOnCurrentPage(index, scroll, orientation))
			{
				firstItemIndex = index += step;
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
