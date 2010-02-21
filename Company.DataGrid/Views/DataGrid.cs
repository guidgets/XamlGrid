using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.Automation;
using Company.DataGrid.Controllers;
using Company.DataGrid.Models;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a control for displaying and manipulating data with a default tabular view.
	/// </summary>
	public class DataGrid : ItemsControl
	{
		/// <summary>
		/// Occurs when the current item of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler CurrentItemChanged;
		/// <summary>
		/// Occurs when the data source of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler DataSourceChanged;
		/// <summary>
		/// Occurs when the source of items for the <see cref="DataGrid"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler ItemsSourceChanged;
		/// <summary>
		/// Occurs when the selection mode of the <see cref="DataGrid"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler SelectionModeChanged;


		/// <summary>
		/// Identifies the property which gets or sets the source that provides the data to display in the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty DataSourceProperty =
			DependencyProperty.Register("DataSource", typeof(object), typeof(DataGrid), new PropertyMetadata(OnDataSourceChanged));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether the columns of the <see cref="DataGrid"/> must be 
		/// automatically created according to the data source.
		/// </summary>
		public static readonly DependencyProperty AutoCreateColumnsProperty =
			DependencyProperty.Register("AutoCreateColumns", typeof(bool), typeof(DataGrid),
			                            new PropertyMetadata(true, OnAutoCreateColumnsChanged));

		/// <summary>
		/// Identifies the property which gets or sets the width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty ColumnWidthProperty =
			DependencyProperty.Register("ColumnWidth", typeof(ColumnWidth), typeof(DataGrid),
			                            new PropertyMetadata(new ColumnWidth(200)));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether 
		/// the <see cref="Columns"/> of this <see cref="DataGrid"/> are resizable.
		/// </summary>
		public static readonly DependencyProperty ResizableColumnsProperty =
			DependencyProperty.Register("ResizableColumns", typeof(bool), typeof(DataGrid), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the property which gets or sets the visibility of the header row of the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderVisibilityProperty =
			DependencyProperty.Register("HeaderVisibility", typeof(Visibility), typeof(DataGrid), new PropertyMetadata(null));

		/// <summary>
		/// Identifies the property which gets or sets the current item of the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty CurrentItemProperty =
			DependencyProperty.Register("CurrentItem", typeof(object), typeof(DataGrid), new PropertyMetadata(OnCurrentItemChanged));

		/// <summary>
		/// Identifies the property which gets or sets the mode which defines the behavior when selecting items in the <see cref="DataGrid"/>.
		/// </summary>
		public static readonly DependencyProperty SelectionModeProperty =
			DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(DataGrid),
			                            new PropertyMetadata(SelectionMode.Extended, OnSelectionModeChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating 
		/// whether the <see cref="Cell"/>s in a <see cref="DataGrid"/> are read-only.
		/// </summary>
		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register("IsEditable", typeof(bool), typeof(DataGrid), new PropertyMetadata(true));

		/// <summary>
		/// The ItemsSourceListener Attached Dependency Property is a private property
		/// used to silently bind to the <see cref="ItemsControl"/> ItemsSourceProperty.
		/// Once bound, the callback method will execute any time the ItemsSource property changes.
		/// </summary>
		private static readonly DependencyProperty itemsSourceListenerProperty =
			DependencyProperty.RegisterAttached("ItemsSourceListener", typeof(object), typeof(DataGrid),
												new PropertyMetadata(null, OnItemsSourceListenerChanged));

		private static readonly Binding itemsSourceBinding = new Binding("ItemsSource")
	                                                     	 {
	                                                     		 RelativeSource = new RelativeSource(RelativeSourceMode.Self),
	                                                     		 Mode = BindingMode.OneWay
	                                                     	 };

		private readonly SortingModel sortingModel;
		private readonly SelectionModel selectionModel;
		private readonly List<Column> otherColumns;

		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);
			this.Columns = new ObservableCollection<Column>();
			this.otherColumns = new List<Column>();

			this.SetBinding(itemsSourceListenerProperty, itemsSourceBinding);

			this.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;

			// TODO: move these to a startup Controller (page 19 and 20 of "Pure MVC - Best Practices")
			DataGridFacade.Instance.RegisterController(new DataGridController(this));
			DataGridFacade.Instance.RegisterModel(this.sortingModel = new SortingModel());
			DataGridFacade.Instance.RegisterModel(this.selectionModel = new SelectionModel());
		}

		/// <summary>
		/// Gets or sets the source that provides the data to display in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The data source that provides the data to display in the <see cref="DataGrid"/>.</value>
		public object DataSource
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
		/// Gets the <see cref="Column"/>s representing the properties of the objects the <see cref="DataGrid"/> displays.
		/// </summary>
		/// <value>The <see cref="Column"/>s representing the properties of the objects the <see cref="DataGrid"/> displays.</value>
		public ObservableCollection<Column> Columns
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
		public bool AutoCreateColumns
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
		/// Gets or sets the width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The width of each of the <see cref="Columns"/> of the <see cref="DataGrid"/>.</value>
		public ColumnWidth ColumnWidth
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
		public bool ResizableColumns
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
		public Visibility HeaderVisibility
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
		/// Gets the descriptions which tell the <see cref="DataGrid"/> how to sort the data it displays.
		/// </summary>
		/// <value>The descriptions which tell the <see cref="DataGrid"/> how to sort the data it displays.</value>
		public SortDescriptionCollection SortDescriptions
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
		public object CurrentItem
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
		/// Gets the list of items which are currently selected in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The list of items which are currently selected in the <see cref="DataGrid"/>.</value>
		public SelectedItemsCollection SelectedItems
		{
			get
			{
				return this.selectionModel.SelectedItems;
			}
		}

		/// <summary>
		/// Gets or sets the mode which defines the behavior when selecting items in the <see cref="DataGrid"/>.
		/// </summary>
		/// <value>The mode which defines the behavior when selecting items in the <see cref="DataGrid"/>.</value>
		public SelectionMode SelectionMode
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
		public bool IsEditable
		{
			get
			{
				return (bool) this.GetValue(IsEditableProperty);
			}
			set
			{
				this.SetValue(IsEditableProperty, value);
			}
		}


		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Row(this);
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
			ItemsControl itemsControl = (ItemsControl) element;
			if (itemsControl.ItemsSource == null)
			{
				itemsControl.ItemsSource = this.Columns;
			}
		}

		/// <summary>
		/// Returns a <see cref="DataGridAutomationPeer"/> object to use in the automation infrastructure.
		/// </summary>
		/// <returns>
		/// The <see cref="DataGridAutomationPeer"/> object to automate this <see cref="DataGrid"/> with.
		/// </returns>
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridAutomationPeer(this);
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
			object firstItem = (from object item in this.ItemsSource 
								where item != null 
								select item).FirstOrDefault();
			if (firstItem == null)
			{
				return;
			}
			if (this.otherColumns.Count == 0 && this.Columns.Count > 0)
			{
				this.otherColumns.AddRange(this.Columns);
			}
			this.Columns.Clear();
			foreach (PropertyInfo property in firstItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				Column column = new Column();
				column.Binding = new Binding(property.Name);
				column.Binding.Mode = property.CanWrite ? BindingMode.TwoWay : BindingMode.OneWay;
				column.Binding.ValidatesOnExceptions = true;
				column.Binding.NotifyOnValidationError = true;
				column.DataType = property.PropertyType;
				this.Columns.Add(column);
			}
		}

		private static void OnItemsSourceListenerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid) d;
			if (dataGrid.ItemsSourceChanged != null)
			{
				dataGrid.ItemsSourceChanged(dataGrid, e);
			}
		}

		private static void OnDataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid) d;
			if (dataGrid.DataSourceChanged != null)
			{
				dataGrid.DataSourceChanged(dataGrid, e);
			}
		}

		private static void OnAutoCreateColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid) d;
			List<Column> swap = new List<Column>(dataGrid.Columns);
			dataGrid.Columns.Clear();
			foreach (Column column in dataGrid.otherColumns)
			{
				dataGrid.Columns.Add(column);
			}
			dataGrid.otherColumns.Clear();
			dataGrid.otherColumns.AddRange(swap);
			swap.Clear();
			if (dataGrid.AutoCreateColumns && dataGrid.Columns.Count == 0)
			{
				dataGrid.CreateAutomaticColumns();
			}
		}

		private static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnCurrentItemChanged(e);
		}

		private void OnCurrentItemChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.CurrentItemChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid) d).OnSelectionModeChanged(e);
		}

		private void OnSelectionModeChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.SelectionModeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}