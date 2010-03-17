using System;
using System.Windows;
using System.Windows.Data;
using Company.DataGrid.Models;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column : DependencyObject
	{
		/// <summary>
		/// Occurs when the <see cref="Width"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler WidthChanged;
		/// <summary>
		/// Occurs when the <see cref="ActualWidth"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public event DependencyPropertyChangedEventHandler ActualWidthChanged;


		/// <summary>
		/// Identifies the dependency property which gets or sets the header which 
		/// displays visual information about a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(object), typeof(Column), new PropertyMetadata(null));

		/// <summary>
		/// Identifies the dependency property which gets or sets the width of the cells in a <see cref="Column"/>.
		/// </summary>		
		public static readonly DependencyProperty WidthProperty =
			DependencyProperty.Register("Width", typeof(ColumnWidth), typeof(Column),
			                            new PropertyMetadata(new ColumnWidth(200), OnWidthChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the absolute width, in pixels, of a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty ActualWidthProperty =
			DependencyProperty.Register("ActualWidth", typeof(double), typeof(Column),
			                            new PropertyMetadata(200d, OnActualWidthChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Column"/> is resizable.
		/// </summary>
		public static readonly DependencyProperty ResizableProperty =
			DependencyProperty.Register("Resizable", typeof(bool), typeof(Column), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty VisibilityProperty =
			DependencyProperty.Register("Visibility", typeof(Visibility), typeof(Column), new PropertyMetadata(Visibility.Visible));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating 
		/// whether the <see cref="Cell"/>s in a <see cref="Column"/> are read-only.
		/// </summary>
		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register("IsEditable", typeof(bool), typeof(Column), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the dependency property which gets or sets the type of the data in the <see cref="Cell"/>s in a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(Type), typeof(Column), new PropertyMetadata(typeof(object)));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Column"/> is selected.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(Column), new PropertyMetadata(false));

		/// <summary>
		/// Identifies the dependency property which gets or sets the style of the <see cref="Cell"/>s in a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty CellStyleProperty =
			DependencyProperty.Register("CellStyle", typeof(Style), typeof(Column), new PropertyMetadata(null));


		private Binding binding;


		/// <summary>
		/// Gets or sets the width of the cells in the <see cref="Column"/>.
		/// </summary>
		/// <value>The width of the cells in the <see cref="Column"/>.</value>
		public ColumnWidth Width
		{
			get
			{
				return (ColumnWidth) this.GetValue(WidthProperty);
			}
			set
			{
				this.SetValue(WidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the absolute width, in pixels, of the <see cref="Column"/>.
		/// </summary>
		/// <value>The actual width.</value>
		public double ActualWidth
		{
			get
			{
				return (double) this.GetValue(ActualWidthProperty);
			}
			set
			{
				this.SetValue(ActualWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Column"/> is resizable.
		/// </summary>
		/// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
		public bool Resizable
		{
			get
			{
				return (bool) this.GetValue(ResizableProperty);
			}
			set
			{
				this.SetValue(ResizableProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of this <see cref="Column"/>.
		/// </summary>
		/// <value>The visibility of this <see cref="Column"/>.</value>
		public Visibility Visibility
		{
			get
			{
				return (Visibility) this.GetValue(VisibilityProperty);
			}
			set
			{
				this.SetValue(VisibilityProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the header which displays visual information about this <see cref="Column"/>.
		/// </summary>
		/// <value>The header to display the information about this <see cref="Column"/>.</value>
		public object Header
		{
			get
			{
				return this.GetValue(HeaderProperty);
			}
			set
			{
				this.SetValue(HeaderProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.
		/// </summary>
		/// <value>The binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.</value>
		public Binding Binding
		{
			get
			{
				return this.binding;
			}
			set
			{
				if (this.binding != value)
				{
					this.binding = value;
					if (this.ReadLocalValue(HeaderProperty) == DependencyProperty.UnsetValue)
					{
						this.Header = this.binding.Path.Path;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public Type DataType
		{
			get
			{
				return (Type) this.GetValue(DataTypeProperty);
			}
			set
			{
				this.SetValue(DataTypeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Column"/> is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Column"/> is selected; otherwise, <c>false</c>.
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
		/// Gets or sets a value indicating whether the <see cref="Cell"/>s in this <see cref="Column"/> are read-only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the <see cref="Cell"/>s in this <see cref="Column"/> are read-only; otherwise, <c>false</c>.
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
		/// Gets or sets the style of the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The style of the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public Style CellStyle
		{
			get
			{
				return (Style) this.GetValue(CellStyleProperty);
			}
			set
			{
				this.SetValue(CellStyleProperty, value);
			}
		}


		/// <summary>
		/// Recalculates the automatic size of the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		public void AutoSize()
		{
			this.Width = new ColumnWidth(this.ActualWidth);
			this.Width = new ColumnWidth(SizeMode.Auto);
		}


		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ColumnWidth columnWidth = (ColumnWidth) e.NewValue;
			switch (columnWidth.SizeMode)
			{
				case SizeMode.Auto:
				case SizeMode.ToHeader:
				case SizeMode.ToData:
					((Column) d).ActualWidth = double.NaN;
					break;
				case SizeMode.Absolute:
					((Column) d).ActualWidth = columnWidth.Value;
					break;
			}
			((Column) d).OnWidthChanged(e);
		}

		private void OnWidthChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.WidthChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Column column = (Column) d;
			double actualWidth = (double) e.NewValue;
			if (column.Width.SizeMode == SizeMode.Absolute)
			{
				column.Width = double.IsNaN(actualWidth) ? new ColumnWidth(SizeMode.Auto) : new ColumnWidth(actualWidth);
			}
			column.OnActualWidthChanged(e);
		}

		private void OnActualWidthChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.ActualWidthChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
