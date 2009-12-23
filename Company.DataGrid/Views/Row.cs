using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a UI element that displays a data object.
	/// </summary>
	public class Row : ItemsControl
	{
		public event DependencyPropertyChangedEventHandler DataContextChanged;
		public event DependencyPropertyChangedEventHandler IsCurrentChanged;
		public event DependencyPropertyChangedEventHandler IsSelectedChanged;

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether a <see cref="Row"/> is the current one.
		/// </summary>
		public static readonly DependencyProperty IsCurrentProperty =
			DependencyProperty.Register("IsCurrent", typeof(bool), typeof(Row), new PropertyMetadata(false, OnIsCurrentChanged));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether a <see cref="Row"/> is selected.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(Row), new PropertyMetadata(false, OnIsSelectedChanged));

		private static readonly DependencyProperty dataContextListenerProperty =
			DependencyProperty.Register("dataContextListener", typeof(object), typeof(Row), new PropertyMetadata(OnDataContextListenerChanged));

		private readonly Binding dataBinding;

		/// <summary>
		/// Represents a UI element that displays a data object.
		/// </summary>
		public Row()
		{
			this.DefaultStyleKey = typeof(Row);

			this.dataBinding = new Binding { Source = this.DataContext, Mode = BindingMode.OneTime };
			
			Binding binding = new Binding("DataContext") { Source = this, Mode = BindingMode.OneWay };
			this.SetBinding(dataContextListenerProperty, binding);

			DataGridFacade.Instance.RegisterController(new RowController(this));
		}

		public Row(DataGrid dataGrid) : this()
		{
			this.DataGrid = dataGrid;
		}

		public DataGrid DataGrid
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Row"/> is the current one.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Row"/> is the current one; otherwise, <c>false</c>.
		/// </value>
		public bool IsCurrent
		{
			get
			{
				return (bool) this.GetValue(IsCurrentProperty);
			}
			set
			{
				this.SetValue(IsCurrentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Row"/> is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Row"/> is selected; otherwise, <c>false</c>.
		/// </value>
		public bool IsSelected
		{
			get
			{
				return (bool) this.GetValue(IsSelectedProperty);
			}
			set
			{
				this.SetValue(IsSelectedProperty, value);
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.GoToCurrent();
			this.GoToSelected();
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Cell();
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
			return item is Cell;
		}

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
        	Cell cell = (Cell) element;
        	Column column = (Column) item;
			cell.Column = column;
        	cell.DataType = column.DataType;
        	cell.Style = column.CellStyle;
        	cell.SetBinding(DataContextProperty, this.dataBinding);
        	cell.SetBinding(Cell.ValueProperty, column.Binding);
        }

		/// <summary>
		/// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			Cell cell = (Cell) element;
			cell.Column = null;
			cell.ClearValue(DataContextProperty);
			cell.DataContext = null;
			cell.ClearValue(Cell.ValueProperty);
			cell.ClearValue(Cell.EditorValueProperty);
			cell.ClearValue(WidthProperty);
		}

		private static void OnDataContextListenerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Row) d).OnDataContextChanged(e);
		}

		private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.DataContextChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnIsCurrentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Row row = (Row) d;
			row.GoToCurrent();
			row.OnIsCurrentChanged(e);
		}

		private void GoToCurrent()
		{
			VisualStateManager.GoToState(this, this.IsCurrent ? "Current" : "NotCurrent", false);
		}

		private void OnIsCurrentChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.IsCurrentChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Row row = (Row) d;
			row.GoToSelected();
			row.OnIsSelectedChanged(e);
		}

		private void GoToSelected()
		{
			VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Deselected", false);
		}

		private void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.IsSelectedChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
