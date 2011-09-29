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
// File:	DataGrid.cs
// Authors:	Dimitar Dobrev

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using System.Linq;
using XamlGrid.Automation;
using XamlGrid.Controllers;
using XamlGrid.Converters;
using XamlGrid.Models;
using XamlGrid.Models.Export;

namespace XamlGrid.Views
{
	/// <summary>
	/// Represents a control for displaying and manipulating data with a default tabular view.
	/// </summary>
	public class DataGrid : ItemsControl
	{
		private event DependencyPropertyChangedEventHandler itemsSourceChanged;


		/// <summary>
		/// Occurs when the current item of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler CurrentItemChanged;
		/// <summary>
		/// Occurs when the current column of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler CurrentColumnChanged;
		/// <summary>
		/// Occurs when the data source of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler DataSourceChanged;
		/// <summary>
		/// Occurs when the type of the items contained in the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler ItemTypeChanged;
		/// <summary>
		/// Occurs when a new item is being added to the source the <see cref="DataGrid"/> is bound to.
		/// </summary>
		public virtual event EventHandler<NewItemEventArgs> NewItemAdding;
		/// <summary>
		/// Occurs when a new item of the <see cref="DataGrid"/> is being brought into view.
		/// </summary>
		public virtual event EventHandler<ScrollEventArgs> BringingIntoView;

		/// <summary>
		/// Occurs when the source of items for the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler ItemsSourceChanged
		{
			add
			{
				this.itemsSourceChanged += value;
			}
			remove
			{
				this.itemsSourceChanged -= value;
			}
		}

		/// <summary>
		/// Occurs when the selection mode of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler SelectionModeChanged;
		/// <summary>
		/// Occurs when the view port where the <see cref="DataGrid"/> positions its items changes its width.
		/// </summary>
		public virtual event EventHandler<CustomSizeChangedEventArgs> ViewportSizeChanged;


		/// <summary>
		/// Identifies the dependency property which gets or sets the source that provides the data to display in a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty DataSourceProperty =
			DependencyProperty.Register("DataSource", typeof(object), typeof(DataGrid), new PropertyMetadata(OnDataSourceChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the type of the items contained in a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty ItemTypeProperty =
			DependencyProperty.Register("ItemType", typeof(Type), typeof(DataGrid), new PropertyMetadata(typeof(object), OnItemTypeChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether the columns of a <see cref="DataGrid"/> must be 
		/// automatically created according to the data source.
		/// </summary>
		public static readonly DependencyProperty AutoCreateColumnsProperty =
			DependencyProperty.Register("AutoCreateColumns", typeof(bool), typeof(DataGrid),
			                            new PropertyMetadata(true, OnAutoCreateColumnsChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="DataGrid"/> displays numbers for its rows.
		/// </summary>
		public static readonly DependencyProperty NumberRowsProperty =
			DependencyProperty.Register("NumberRows", typeof(bool), typeof(DataGrid), new PropertyMetadata(false, OnNumberRowsChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty ColumnWidthProperty =
			DependencyProperty.Register("ColumnWidth", typeof(ColumnWidth), typeof(DataGrid),
			                            new PropertyMetadata(new ColumnWidth(200)));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether 
		/// the <see cref="Columns"/> of this <see cref="DataGrid"/> are resizable.
		/// </summary>
		public static readonly DependencyProperty ResizableColumnsProperty =
			DependencyProperty.Register("ResizableColumns", typeof(bool), typeof(DataGrid), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of the header row of a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderVisibilityProperty =
			DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(DataGrid), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of the new row of a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty NewRowVisibilityProperty =
			DependencyProperty.Register("NewRowVisibility", typeof(Visibility), typeof(DataGrid), new PropertyMetadata(Visibility.Collapsed));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of the footer row of a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty FooterVisibilityProperty =
			DependencyProperty.Register("FooterVisibility", typeof(Visibility), typeof(DataGrid), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Identifies the dependency property which gets or sets the current item of a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty CurrentItemProperty =
			DependencyProperty.Register("CurrentItem", typeof(object), typeof(DataGrid), new PropertyMetadata(OnCurrentItemChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the current column, that is, the columns of the focused cell, of a <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty CurrentColumnProperty =
			DependencyProperty.Register("CurrentColumn", typeof(Column), typeof(DataGrid), new PropertyMetadata(OnCurrentColumnChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the mode which defines the behaviour when selecting items in the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty SelectionModeProperty =
			DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(DataGrid),
			                            new PropertyMetadata(SelectionMode.Extended, OnSelectionModeChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating 
		/// whether the <see cref="DataGrid"/>s in a <see cref="Cell"/> are read-only.
		/// </summary>
		public static readonly DependencyProperty IsReadOnlyProperty =
			DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGrid), new PropertyMetadata(false));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether to copy the header of a <see cref="DataGrid"/> to the clipboard.
		/// </summary>
		public static readonly DependencyProperty CopyOptionsProperty =
			DependencyProperty.Register("CopyOptions", typeof(ExportOptions), typeof(DataGrid), new PropertyMetadata(ExportOptions.Header));


		/// <summary>
		/// The ItemsSourceListener Attached Dependency Property is a private property
		/// used to silently bind to the <see cref="ItemsControl"/> ItemsSourceProperty.
		/// Once bound, the callback method will execute any time the ItemsSource property changes.
		/// </summary>
		private static readonly DependencyProperty itemsSourceListenerProperty =
			DependencyProperty.RegisterAttached("itemsSourceListener", typeof(object), typeof(DataGrid),
												new PropertyMetadata(null, OnItemsSourceListenerChanged));

		private static readonly DependencyProperty viewportWidthListenerProperty =
			DependencyProperty.Register("viewportWidthListener", typeof(double), typeof(ScrollViewer), new PropertyMetadata(OnViewportWidthChanged));

		private static readonly DependencyProperty viewportHeightListenerProperty =
			DependencyProperty.Register("viewportHeightListener", typeof(double), typeof(ScrollViewer), new PropertyMetadata(OnViewportHeightChanged));

		private static readonly Binding itemsSourceBinding = new Binding("ItemsSource")
	                                                     	 {
	                                                     		 RelativeSource = new RelativeSource(RelativeSourceMode.Self),
	                                                     		 Mode = BindingMode.OneWay
															 };

		private static readonly Binding viewportWidthBinding = new Binding("ViewportWidth")
		                                                       {
																   RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                                       };

		private static readonly Binding viewportHeightBinding = new Binding("ViewportHeight")
		                                                        {
		                                                        	RelativeSource = new RelativeSource(RelativeSourceMode.Self)
																};

		private readonly Binding dataItemToIndexBinding = new Binding("DataContext")
		                                                  	{
		                                                  		RelativeSource = new RelativeSource(RelativeSourceMode.Self),
		                                                  		Converter = new DataItemToIndexConverter()
		                                                  	};

		private readonly SortingModel sortingModel;
		private readonly SelectionModel selectionModel;
		private readonly List<Column> otherColumns;
		private ScrollViewer scroll;
		private readonly List<Row> cachedGUI = new List<Row>();
		private int cacheIndex;

		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);
			this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
			this.Columns = new ColumnsCollection();
			this.otherColumns = new List<Column>();

			this.SetBinding(itemsSourceListenerProperty, itemsSourceBinding);

			this.itemsSourceChanged += this.DataGrid_ItemsSourceChanged;

			// TODO: move these to a startup Controller (page 19 and 20 of "Pure MVC - Best Practices")
			DataGridFacade.Instance.RegisterController(new DataGridController(this));
			DataGridFacade.Instance.RegisterModel(this.sortingModel = new SortingModel());
			DataGridFacade.Instance.RegisterModel(this.selectionModel = new SelectionModel());

			dataItemToIndexBinding.ConverterParameter = this.Items;
		}

		private ScrollViewer Scroll
		{
			get
			{
				return this.scroll ?? (this.scroll = this.GetScroll());
			}
		}

		/// <summary>
		/// Gets or sets the source that provides the data to display in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The data source that provides the data to display in the <see cref="DataGrid"/>.</value>
		public virtual object DataSource
		{
			get
			{
				return this.GetValue(DataSourceProperty);
			}
			set
			{
				this.SetValue(DataSourceProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the type of the items contained in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The type of the items contained in the <see cref="DataGrid"/>.</value>
		public Type ItemType
		{
			get
			{
				return (Type) this.GetValue(ItemTypeProperty);
			}
			set
			{
				this.SetValue(ItemTypeProperty, value);
			}
		}

		/// <summary>
		/// Gets the <see cref="Column"/>s representing the properties of the objects the <see cref="DataGrid"/> displays.
		/// </summary>
		/// <value>The <see cref="Column"/>s representing the properties of the objects the <see cref="DataGrid"/> displays.</value>
		public virtual ColumnsCollection Columns
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the columns of the <see cref="DataGrid"/> must be 
		/// automatically created according to the data source.
		/// </summary>
		/// <value><c>true</c> if the columns of the <see cref="DataGrid"/> must be automatically created 
		/// according to the data source; otherwise, <c>false</c>.</value>
		public virtual bool AutoCreateColumns
		{
			get
			{
				return (bool) this.GetValue(AutoCreateColumnsProperty);
			}
			set
			{
				this.SetValue(AutoCreateColumnsProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="DataGrid"/> displays numbers for its rows.
		/// </summary>
		/// <value>
		///   <c>true</c> if the <see cref="DataGrid"/> displays numbers for its rows; otherwise, <c>false</c>.
		/// </value>
		public bool NumberRows
		{
			get
			{
				return (bool) this.GetValue(NumberRowsProperty);
			}
			set
			{
				this.SetValue(NumberRowsProperty, value);
			}
		}

		/// <summary>
		/// Gets the column, if any, that displays row numbers.
		/// </summary>
		public Column NumberColumn
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.</value>
		public virtual ColumnWidth ColumnWidth
		{
			get
			{
				return (ColumnWidth) this.GetValue(ColumnWidthProperty);
			}
			set
			{
				this.SetValue(ColumnWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Columns"/> of this <see cref="DataGrid"/> are resizable.
		/// </summary>
		/// <value><c>true</c> if the <see cref="Columns"/> of this <see cref="DataGrid"/> are resizable; otherwise, <c>false</c>.</value>
		public virtual bool ResizableColumns
		{
			get
			{
				return (bool) this.GetValue(ResizableColumnsProperty);
			}
			set
			{
				this.SetValue(ResizableColumnsProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the header row of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The visibility of the header row of the <see cref="DataGrid"/>.</value>
		public virtual Visibility HeaderVisibility
		{
			get
			{
				return (Visibility) this.GetValue(HeaderVisibilityProperty);
			}
			set
			{
				this.SetValue(HeaderVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the new row of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The visibility of the new row of the <see cref="DataGrid"/>.</value>
		public Visibility NewRowVisibility
		{
			get
			{
				return (Visibility) this.GetValue(NewRowVisibilityProperty);
			}
			set
			{
				this.SetValue(NewRowVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the footer row of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The visibility of the footer row of the <see cref="DataGrid"/>.</value>
		public virtual Visibility FooterVisibility
		{
			get
			{
				return (Visibility) this.GetValue(FooterVisibilityProperty);
			}
			set
			{
				this.SetValue(FooterVisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets the descriptions which tell the <see cref="DataGrid"/> how to sort the data it displays.
		/// </summary>
		/// <value>The descriptions which tell the <see cref="DataGrid"/> how to sort the data it displays.</value>
		public virtual SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.sortingModel.SortDescriptions;
			}
		}

		/// <summary>
		/// Gets or sets the current item of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The current item of the <see cref="DataGrid"/>.</value>
		public virtual object CurrentItem
		{
			get
			{
				return this.GetValue(CurrentItemProperty);
			}
			set
			{
				this.SetValue(CurrentItemProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the current column, that is, the column of the focused cell, of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The current column.</value>
		public Column CurrentColumn
		{
			get
			{
				return (Column) this.GetValue(CurrentColumnProperty);
			}
			set
			{
				this.SetValue(CurrentColumnProperty, value);
			}
		}


		/// <summary>
		/// Gets the list of items which are currently selected in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The list of items which are currently selected in the <see cref="DataGrid"/>.</value>
		public virtual SelectedItemsCollection SelectedItems
		{
			get
			{
				return this.selectionModel.SelectedItems;
			}
		}

		/// <summary>
		/// Gets or sets the mode which defines the behaviour when selecting items in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The mode which defines the behaviour when selecting items in the <see cref="DataGrid"/>.</value>
		public virtual SelectionMode SelectionMode
		{
			get
			{
				return (SelectionMode) this.GetValue(SelectionModeProperty);
			}
			set
			{
				this.SetValue(SelectionModeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Cell"/>s in this <see cref="DataGrid"/> are read-only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the <see cref="Cell"/>s in this <see cref="DataGrid"/> are read-only; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsReadOnly
		{
			get
			{
				return (bool) this.GetValue(IsReadOnlyProperty);
			}
			set
			{
				this.SetValue(IsReadOnlyProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to copy the header of the <see cref="DataGrid"/> to the clipboard.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the header should be copied to the clipboard; otherwise, <c>false</c>.
		/// </value>
		public ExportOptions CopyOptions
		{
			get
			{
				return (ExportOptions) this.GetValue(CopyOptionsProperty);
			}
			set
			{
				this.SetValue(CopyOptionsProperty, value);
			}
		}


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (this.Scroll != null)
			{
				this.Scroll.SetBinding(viewportWidthListenerProperty, viewportWidthBinding);
				this.Scroll.SetBinding(viewportHeightListenerProperty, viewportHeightBinding);
			}
		}

		/// <summary>
		/// When implemented in a derived class, returns class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> implementations for the Silverlight automation infrastructure.
		/// </summary>
		/// <returns>
		/// The class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> subclass to return.
		/// </returns>
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridAutomationPeer(this);
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			if (this.cacheIndex < this.cachedGUI.Count)
			{
				return this.cachedGUI[this.cacheIndex++];
			}
			this.cacheIndex++;
			Row row = new Row();
			row.SetBinding(Row.IndexProperty, dataItemToIndexBinding);
			this.cachedGUI.Add(row);
			return row;
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		/// <c>true</c> if the item is (or is eligible to be) its own container; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is Row;
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (!this.IsItemItsOwnContainerOverride(element))
			{
				return;
			}
			Row row = (Row) element;
			if (row.ItemsSource == null)
			{
				row.ItemsSource = this.Columns;
			}
		}

		/// <summary>
		/// Called when the value of the <see cref="ItemsControl.Items"/> property changes.
		/// </summary>
		/// <param name="e">A <see cref="NotifyCollectionChangedEventArgs"/> that contains the event data</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
					foreach (Row row in from row in this.cachedGUI 
										from object oldItem in e.OldItems
										where oldItem == row.DataContext
										select row)
					{
						row.DataContext = null;
						row.ItemsSource = null;
						row.ClearValue(Row.IndexProperty);
						DataGridFacade.Instance.RemoveController(row.GetHashCode().ToString());
						this.cachedGUI.Remove(row);
					}
					break;
			}
			this.cacheIndex = 0;
		}


		/// <summary>
		/// Raises the <see cref="NewItemAdding"/> event.
		/// </summary>
		/// <param name="e">The <see cref="NewItemEventArgs"/> instance containing the event data.</param>
		public void OnNewItemAdding(NewItemEventArgs e)
		{
			EventHandler<NewItemEventArgs> handler = this.NewItemAdding;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public virtual void BringIntoView(object item)
		{
			this.BringIntoView(this.Items.IndexOf(item));
		}

	    public virtual void BringIntoView(int itemIndex)
	    {
	        if (itemIndex < 0)
	        {
	            return;
	        }
	        EventHandler<ScrollEventArgs> handler = this.BringingIntoView;
	        if (handler != null)
	        {
	            handler(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, itemIndex));
	        }
	    }


	    private void DataGrid_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.CreateAutomaticColumns();
		}

		private void CreateAutomaticColumns()
		{
			if (this.ItemsSource == null)
			{
				// TODO: must columns be cleared when the items are cleared?
				this.Columns.Clear();
				return;
			}
			if (!this.AutoCreateColumns)
			{
				return;
			}
			if (this.otherColumns.Count == 0 && this.Columns.Count > 0)
			{
				this.otherColumns.AddRange(this.Columns);
			}
			this.Columns.Clear();
			if (this.ItemType.IsSimple())
			{
				this.Columns.Add(new Column(string.Empty, this.ItemType, false));
			}
			else
			{
				foreach (PropertyInfo property in this.ItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					this.Columns.Add(new Column(property.Name, property.PropertyType, property.CanWrite));
				}
			}
		}

		private static void OnViewportWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.GetVisualAncestors().OfType<DataGrid>().Last().OnViewportWidthChanged(e);
		}

		private void OnViewportWidthChanged(DependencyPropertyChangedEventArgs e)
		{
			Size oldSize = new Size((double) e.OldValue, this.Scroll.ViewportHeight);
			Size newSize = new Size((double) e.NewValue, this.Scroll.ViewportHeight);
			this.OnViewportSizeChanged(new CustomSizeChangedEventArgs(oldSize, newSize));
		}

		private static void OnViewportHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.GetVisualAncestors().OfType<DataGrid>().Last().OnViewportHeightChanged(e);
		}

		private void OnViewportHeightChanged(DependencyPropertyChangedEventArgs e)
		{
			Size oldSize = new Size(this.Scroll.ViewportWidth, (double) e.OldValue);
			Size newSize = new Size(this.Scroll.ViewportWidth, (double) e.NewValue);
			this.OnViewportSizeChanged(new CustomSizeChangedEventArgs(oldSize, newSize));
		}

		/// <summary>
		/// Raises the <see cref="ViewportSizeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="CustomSizeChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnViewportSizeChanged(CustomSizeChangedEventArgs e)
		{
			EventHandler<CustomSizeChangedEventArgs> handler = this.ViewportSizeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnItemsSourceListenerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnItemsSourceChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="ItemsSourceChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.itemsSourceChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnDataSourceChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="DataSourceChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnDataSourceChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.DataSourceChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnItemTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnItemTypeChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="ItemTypeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnItemTypeChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.ItemTypeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnAutoCreateColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// TODO: move to the controller?
			((DataGrid) d).OnAutoCreateColumnsChanged(d);
		}

		protected virtual void OnAutoCreateColumnsChanged(DependencyObject d)
		{
			List<Column> swap = new List<Column>(this.Columns);
			this.Columns.Clear();
			foreach (Column column in this.otherColumns)
			{
				this.Columns.Add(column);
			}
			this.otherColumns.Clear();
			this.otherColumns.AddRange(swap);
			swap.Clear();
			if (this.AutoCreateColumns && this.Columns.Count == 0)
			{
				this.CreateAutomaticColumns();
			}
		}

		private static void OnNumberRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnNumberRowsChanged(e);
		}

		protected virtual void OnNumberRowsChanged(DependencyPropertyChangedEventArgs e)
		{
			// TODO: move to the controller?
			if ((bool) e.NewValue)
			{
				this.NumberColumn = new Column("RowIndex", typeof(int), false);
				this.NumberColumn.Header = "¹";
				this.NumberColumn.IsSortable = false;
				this.NumberColumn.Binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
				this.NumberColumn.Binding.Converter = new IndexToNumberConverter();
				this.NumberColumn.Width = new ColumnWidth(SizeMode.Auto);
				this.Columns.Insert(0, this.NumberColumn);
			}
			else
			{
				this.Columns.Remove(this.NumberColumn);
				this.NumberColumn = null;
			}
		}

		private static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnCurrentItemChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="CurrentItemChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnCurrentItemChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.CurrentItemChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnCurrentColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnCurrentColumnChanged(e);
		}

		private void OnCurrentColumnChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = CurrentColumnChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnSelectionModeChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="SelectionModeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnSelectionModeChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.SelectionModeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
