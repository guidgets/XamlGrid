﻿using System;
using System.Windows;
using System.Windows.Data;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column : DependencyObject
	{
		/// <summary>
		/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
		/// </summary>
		public Column()
		{
			this.DataType = typeof(object);
		}

		/// <summary>
		/// Identifies the dependency property which gets or sets the header which 
		/// displays visual information about a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(object), typeof(Column), new PropertyMetadata(null));

		/// <summary>
		/// Identifies the dependency property which gets or sets the binding which 
		/// the <see cref="Cell"/>s in a <see cref="Column"/> use to get the data they display.
		/// </summary>
		public static readonly DependencyProperty BindingProperty =
			DependencyProperty.Register("DataBinding", typeof(Binding), typeof(Column), new PropertyMetadata(OnBindingChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the width of the cells in a <see cref="Column"/>.
		/// </summary>		
		public static readonly DependencyProperty WidthProperty =
			DependencyProperty.Register("Width", typeof(GridLength), typeof(Column),
			                            new PropertyMetadata(new GridLength(200), OnWidthChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the absolute width, in pixels, of a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty ActualWidthProperty =
			DependencyProperty.Register("ActualWidth", typeof(double), typeof(Column),
			                            new PropertyMetadata(200d, OnActualWidthChanged));

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
		/// Identifies the dependency property which gets or sets the style of the <see cref="Cell"/>s in a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty CellStyleProperty =
			DependencyProperty.Register("CellStyle", typeof(Style), typeof(Column), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the width of the cells in the <see cref="Column"/>.
		/// </summary>
		/// <value>The width of the cells in the <see cref="Column"/>.</value>
		public GridLength Width
		{
			get
			{
				return (GridLength) this.GetValue(WidthProperty);
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
		/// Gets or sets the header which displays visual information about the <see cref="Column"/>.
		/// </summary>
		/// <value>The header to display the information about the <see cref="Column"/>.</value>
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
				return (Binding) this.GetValue(BindingProperty);
			}
			set
			{
				this.SetValue(BindingProperty, value);
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
			this.Width = new GridLength(this.ActualWidth, GridUnitType.Pixel);
			this.Width = new GridLength(1, GridUnitType.Auto);
		}


		private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Column column = (Column) d;
			if (column.Header == null)
			{
				column.Header = column.Binding.Path.Path;
			}
		}

		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridLength columnWidth = (GridLength) e.NewValue;
			switch (columnWidth.GridUnitType)
			{
				case GridUnitType.Auto:
					((Column) d).ActualWidth = double.NaN;
					break;
				case GridUnitType.Pixel:
					((Column) d).ActualWidth = columnWidth.Value;
					break;
			}
		}

		private static void OnActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Column column = (Column) d;
			double actualWidth = (double) e.NewValue;
			if (!column.Width.IsAuto)
			{
				column.Width = double.IsNaN(actualWidth) ? new GridLength(1, GridUnitType.Auto) : new GridLength(actualWidth);
			}
		}
	}
}
