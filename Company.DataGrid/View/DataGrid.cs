using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Represents a control for displaying and manipulating data with a default tabular view.
	/// </summary>
	public class DataGrid : ItemsControl
	{
		public static readonly DependencyProperty AutoCreateColumnsProperty =
			DependencyProperty.Register("AutoCreateColumns", typeof(bool), typeof(DataGrid),
			                            new PropertyMetadata(true, OnAutoCreateColumnsChanged));

		private readonly List<Column> otherColumns;

		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);

			// TODO: carefully review this line: it may be more appropriate to initialize the columns elsewhere and/or use another collection
			this.Columns = new ObservableCollection<Column>();
			this.otherColumns = new List<Column>();
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
			if (element is ItemsControl)
			{
				ItemsControl itemsControl = (ItemsControl) element;
				if (itemsControl.ItemsSource == null)
				{
					itemsControl.ItemsSource = this.Columns;
				}
			}
		}

		/// <summary>
		/// Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> property changes.
		/// </summary>
		/// <param name="e">Information about the change.</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.CreateColumns();
			}
		}

		private void CreateColumns()
		{
			if (this.Items.Count > 0 && this.AutoCreateColumns)
			{
				// TODO: possible optimization: do not recreate all columns if the new items are from the same type
				if (this.otherColumns.Count == 0 && this.Columns.Count > 0)
				{
					this.otherColumns.AddRange(this.Columns);
				}
				this.Columns.Clear();
				// TODO: do not use reflection here; use our data readers when they are ready.
				foreach (PropertyInfo property in this.Items[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
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
				dataGrid.CreateColumns();
			}
		}
	}
}