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
		/// <summary>
		/// Represents a control for displaying and manipulating data with a default tabular view.
		/// </summary>
		public DataGrid()
		{
			this.DefaultStyleKey = typeof(DataGrid);

			// TODO: carefully review this line: it may be more appropriate to initialize the columns elsewhere
			// TODO: or use another collection
			this.Columns = new ObservableCollection<Column>();
		}

		/// <summary>
		/// Gets the collection of columns of the <see cref="DataGrid"/>.
		/// </summary>
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
			get;
			set;
		}

		/// <summary>
		/// Creates or identifies the element used to display a specified item.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Windows.Controls.ListBoxItem"/> corresponding to a specified item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Row();
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own item container.
		/// </summary>
		/// <param name="item">The specified item.</param>
		/// <returns>
		/// true if the item is its own item container; otherwise, false.
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
				((ItemsControl) element).ItemsSource = this.Columns;				
			}
		}

		/// <summary>
		/// Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> property changes.
		/// </summary>
		/// <param name="e">Information about the change.</param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Reset && this.Items.Count > 0 && this.AutoCreateColumns)
			{
				// TODO: possible optimization: do not recreate all columns if the new items are from the same type
				this.Columns.Clear();
				// TODO: do not use reflection here; use our data readers when they are ready.
				foreach (PropertyInfo property in this.Items[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
				{
					Column column = new Column();
					column.DataBinding = new Binding(property.Name);
					column.DataBinding.Mode = property.CanWrite? BindingMode.TwoWay: BindingMode.OneWay;
					this.Columns.Add(column);
				}
			}
		}
	}
}