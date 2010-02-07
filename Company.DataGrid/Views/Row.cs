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
		/// <summary>
		/// Occurs when the data context of the <see cref="Row"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler DataContextChanged;
		/// <summary>
		/// Occurs when the <see cref="Row"/> changes its currency.
		/// </summary>
		public event DependencyPropertyChangedEventHandler IsCurrentChanged;
		/// <summary>
		/// Occurs when the <see cref="Row"/> changes its selection state.
		/// </summary>
		public event DependencyPropertyChangedEventHandler IsSelectedChanged;

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether a <see cref="Row"/> is the current one.
		/// </summary>
		public static readonly DependencyProperty IsFocusedProperty =
			DependencyProperty.Register("IsCurrent", typeof(bool), typeof(Row), new PropertyMetadata(false, OnIsFocusedChanged));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether a <see cref="Row"/> is selected.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(Row), new PropertyMetadata(false, OnIsSelectedChanged));

		private static readonly DependencyProperty dataContextListenerProperty =
			DependencyProperty.Register("dataContextListener", typeof(object), typeof(Row), new PropertyMetadata(OnDataContextListenerChanged));

		private static readonly Binding dataContextBinding = new Binding("DataContext")
	                                                     	 {
	                                                     		 RelativeSource = new RelativeSource(RelativeSourceMode.Self),
	                                                     		 Mode = BindingMode.OneWay
	                                                     	 };


		private readonly Binding dataBinding;

		/// <summary>
		/// Represents a UI element that displays a data object.
		/// </summary>
		public Row()
		{
			this.DefaultStyleKey = typeof(Row);

			this.dataBinding = new Binding { Source = this.DataContext, Mode = BindingMode.OneTime };

			this.SetBinding(dataContextListenerProperty, dataContextBinding);

			DataGridFacade.Instance.RegisterController(new RowController(this));
		}

		/// <summary>
		/// Represents a UI element that displays a data object.
		/// </summary>
		/// <param name="dataGrid">The data grid which contains the <see cref="Row"/>.</param>
		public Row(DataGrid dataGrid) : this()
		{
			this.DataGrid = dataGrid;
		}

		/// <summary>
		/// Gets the data grid in which the <see cref="Row"/> is contained.
		/// </summary>
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
		public bool IsFocused
		{
			get
			{
				return (bool) this.GetValue(IsFocusedProperty);
			}
			set
			{
				this.SetValue(IsFocusedProperty, value);
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

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application, but see Remarks for more information.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.GoToFocused();
			this.GoToSelected();
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.GotFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.IsFocused = true;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (!this.IsFocusWithin())
			{
				this.IsFocused = false;
			}
			base.OnLostFocus(e);
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
		/// <c>true</c> if the item is (or is eligible to be) its own container; otherwise, <c>false</c>.
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
			base.PrepareContainerForItemOverride(element, item);
        	Cell cell = (Cell) element;
        	DataTemplate contentTemplate = cell.ContentTemplate;
			base.PrepareContainerForItemOverride(element, item);
        	if (string.IsNullOrEmpty(this.DisplayMemberPath))
        	{
        		cell.ContentTemplate = contentTemplate;
        	}
        	Column column = (Column) item;
			cell.Column = column;
        	cell.DataType = column.DataType;
			cell.Style = column.CellStyle;
			if (cell.ReadLocalValue(Cell.IsEditableProperty) == DependencyProperty.UnsetValue)
			{
				cell.SetBinding(Cell.IsEditableProperty,
								new Binding("Column.IsEditable") { RelativeSource = new RelativeSource(RelativeSourceMode.Self) });
			}
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
			cell.ClearValue(DataContextProperty);
			cell.DataContext = null;
			cell.ClearValue(Cell.ValueProperty);
			cell.ClearValue(Cell.IsEditableProperty);
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

		private static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Row row = (Row) d;
			if (row.IsFocused)
			{
				row.Focus();
			}
			row.GoToFocused();
			row.OnIsFocusedChanged(e);
		}

		private void GoToFocused()
		{
			VisualStateManager.GoToState(this, this.IsFocused ? "Focused" : "Unfocused", false);
		}

		private void OnIsFocusedChanged(DependencyPropertyChangedEventArgs e)
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
