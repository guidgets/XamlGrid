using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
		/// The ItemsSourceListener Attached Dependency Property is a private property
		/// the ItemsSourceChangedBehavior will use silently to bind to the ItemsControl
		/// ItemsSourceProperty.
		/// Once bound, the callback method will execute anytime the ItemsSource property changes
		/// </summary>
		private static readonly DependencyProperty ItemsSourceListenerProperty =
			DependencyProperty.RegisterAttached("ItemsSourceListener", typeof(object), typeof(DataGrid),
												new PropertyMetadata(null, OnItemsSourceListenerChanged));

		public static readonly DependencyProperty AutoCreateColumnsProperty =
			DependencyProperty.Register("AutoCreateColumns", typeof(bool), typeof(DataGrid),
			                            new PropertyMetadata(true, OnAutoCreateColumnsChanged));

		public static readonly DependencyProperty DataSourceProperty =
			DependencyProperty.Register("DataSource", typeof(object), typeof(DataGrid), new PropertyMetadata(OnDataSourceChanged));

		public event DependencyPropertyChangedEventHandler DataSourceChanged;
		public event DependencyPropertyChangedEventHandler ItemsSourceChanged;

		private readonly SortingModel sortingModel;
		private readonly List<Column> otherColumns;

		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);
			this.AutoCreateColumns = true;
			this.Columns = new ObservableCollection<Column>();
			this.otherColumns = new List<Column>();

			Binding binding = new Binding("ItemsSource") { Source = this, Mode = BindingMode.OneWay };
			this.SetBinding(ItemsSourceListenerProperty, binding);

			this.ItemsSourceChanged += this.DataGrid_ItemsSourceChanged;

			// TODO: move these to a startup Controller (page 19 and 20 of "Pure MVC - Best Practices")
			DataGridFacade.Instance.RegisterController(new DataGridController(this));
			DataGridFacade.Instance.RegisterModel(this.sortingModel = new SortingModel());
		}

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
		/// Gets the <see cref="Column"/>s representing the subdata (most often properties) of the objects the <see cref="DataGrid"/> displays.
		/// </summary>
		/// <value>The <see cref="Column"/>s representing the subdata (most often properties) of the objects the <see cref="DataGrid"/> displays.</value>
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

		public SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.sortingModel.SortDescriptions;
			}
		}

		/// <summary>
		/// Dependency Property Changed Call Back method. This will be called anytime
		/// the ItemsSourceListenerProperty value changes on a Dependency Object
		/// </summary>
		/// <param name="d">The obj.</param>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Row();
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		/// true if the item is (or is eligible to be) its own container; otherwise, false.
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
	}
}