using System;
using System.Windows;
using System.Windows.Data;
using Company.Widgets.Models;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column : DependencyObject
	{
		/// <summary>
		/// Occurs when the <see cref="Width"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler WidthChanged;
		/// <summary>
		/// Occurs when the <see cref="ActualWidth"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler ActualWidthChanged;
		/// <summary>
		/// Occurs when the <see cref="Visibility"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler VisibilityChanged;


		/// <summary>
		/// Identifies the dependency property which gets or sets the header which 
		/// displays visual information about a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(object), typeof(Column), new PropertyMetadata(null));

		public static readonly DependencyProperty DataBindingProperty =
			DependencyProperty.Register("DataBinding", typeof(Binding), typeof(Column),
			                            new PropertyMetadata(null, OnBindingChanged));

		public static readonly DependencyProperty FooterDataBindingProperty =
			DependencyProperty.Register("FooterDataBinding", typeof(Binding), typeof(Column),
										new PropertyMetadata(null, OnFooterBindingChanged));

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
		public static readonly DependencyProperty IsResizableProperty =
			DependencyProperty.Register("IsResizable", typeof(bool), typeof(Column), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty VisibilityProperty =
			DependencyProperty.Register("Visibility", typeof(Visibility), typeof(Column), new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

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


		/// <summary>
		/// Gets or sets the width of the cells in the <see cref="Column"/>.
		/// </summary>
		/// <value>The width of the cells in the <see cref="Column"/>.</value>
		public virtual ColumnWidth Width
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
		public virtual double ActualWidth
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
		public virtual bool IsResizable
		{
			get
			{
				return (bool) this.GetValue(IsResizableProperty);
			}
			set
			{
				this.SetValue(IsResizableProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the visibility of this <see cref="Column"/>.
		/// </summary>
		/// <value>The visibility of this <see cref="Column"/>.</value>
		public virtual Visibility Visibility
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
		public virtual object Header
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
				return (Binding) this.GetValue(DataBindingProperty);
			}
			set
			{
				this.SetValue(DataBindingProperty, value);
			}
		}

		public Binding FooterBinding
		{
			get
			{
				return (Binding) this.GetValue(FooterDataBindingProperty);
			}
			set
			{
				this.SetValue(FooterDataBindingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public virtual Type DataType
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
		public virtual bool IsSelected
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
		public virtual bool IsEditable
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
		public virtual Style CellStyle
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
		public virtual void AutoSize()
		{
			this.Width = new ColumnWidth(this.ActualWidth);
			this.Width = new ColumnWidth(SizeMode.Auto);
		}


		private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Column) d).OnBindingChanged(e);
		}

		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.Binding == null)
			{
				return;
			}
			if (this.ReadLocalValue(HeaderProperty) == DependencyProperty.UnsetValue)
			{
				this.Header = this.Binding.Path.Path;
			}
			this.IsEditable = this.Binding.Mode == BindingMode.TwoWay;
		}

		private static void OnFooterBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Column) d).OnFooterBindingChanged(e);
		}

		protected virtual void OnFooterBindingChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.FooterBinding != null && this.FooterBinding.ConverterParameter == null)
			{
				this.FooterBinding.ConverterParameter = this.Binding.Path.Path;
			}
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

		/// <summary>
		/// Raises the <see cref="WidthChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnWidthChanged(DependencyPropertyChangedEventArgs e)
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

		/// <summary>
		/// Raises the <see cref="ActualWidthChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnActualWidthChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.ActualWidthChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Column) d).OnVisibilityChanged(e);
		}

		/// <summary>
		/// Raises the <see cref="VisibilityChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.VisibilityChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
