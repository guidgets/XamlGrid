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
// File:	Column.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Windows;
using System.Windows.Data;
using Guidgets.XamlGrid.Models;
using Guidgets.XamlGrid.Views;

namespace Guidgets.XamlGrid.Controllers
{
	/// <summary>
	/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
	/// </summary>
	public class Column : DependencyObject
	{
		/// <summary>
		/// Occurs when the <see cref="Width"/> of this <see cref="Column"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler IndexChanged;

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

		public static readonly DependencyProperty IndexProperty =
			DependencyProperty.Register("Index", typeof(int), typeof(Column), new PropertyMetadata(-1, OnIndexChanged));

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
		/// Identifies the dependency property which gets or sets a value indicating whether data can be sorted through a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty IsSortableProperty =
			DependencyProperty.Register("IsSortable", typeof(bool), typeof(Column), new PropertyMetadata(true));

		/// <summary>
		/// Identifies the dependency property which gets or sets the visibility of a <see cref="Column"/>.
		/// </summary>
		public static readonly DependencyProperty VisibilityProperty =
			DependencyProperty.Register("Visibility", typeof(Visibility), typeof(Column),
			                            new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating 
		/// whether the <see cref="Cell"/>s in a <see cref="Column"/> are read-only.
		/// </summary>
		public static readonly DependencyProperty IsReadOnlyProperty =
			DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(Column), new PropertyMetadata(false));

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
		/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
		/// </summary>
		public Column()
		{
		}

		/// <summary>
		/// Represents a controller that tells a <see cref="Cell"/> what data to display and how to display it.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="propertyType">Type of the property.</param>
		/// <param name="writeable">if set to <c>true</c> the values of the cells in the <see cref="Column"/> are writeable.</param>
		public Column(string propertyName, Type propertyType, bool writeable)
		{
			Binding binding = new Binding(propertyName);
			binding.Mode = writeable ? BindingMode.TwoWay : BindingMode.OneWay;
			binding.ValidatesOnExceptions = true;
			binding.NotifyOnValidationError = true;
			this.SetValue(DataBindingProperty, binding);
			this.SetValue(DataTypeProperty, propertyType);
		}


		public int Index
		{
			get { return (int) this.GetValue(IndexProperty); }
			set { this.SetValue(IndexProperty, value); }
		}

		/// <summary>
		/// Gets or sets the width of the cells in the <see cref="Column"/>.
		/// </summary>
		/// <value>The width of the cells in the <see cref="Column"/>.</value>
		public virtual ColumnWidth Width
		{
			get { return (ColumnWidth) this.GetValue(WidthProperty); }
			set { this.SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the absolute width, in pixels, of the <see cref="Column"/>.
		/// </summary>
		/// <value>The actual width.</value>
		public virtual double ActualWidth
		{
			get { return (double) this.GetValue(ActualWidthProperty); }
			set { this.SetValue(ActualWidthProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Column"/> is resizable.
		/// </summary>
		/// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
		public virtual bool IsResizable
		{
			get { return (bool) this.GetValue(IsResizableProperty); }
			set { this.SetValue(IsResizableProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether data can be sorted through this <see cref="Column"/>.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if data can be sorted through this <see cref="Column"/>; otherwise, <c>false</c>.
		/// </value>
		public bool IsSortable
		{
			get { return (bool) this.GetValue(IsSortableProperty); }
			set { this.SetValue(IsSortableProperty, value); }
		}

		/// <summary>
		/// Gets or sets the visibility of this <see cref="Column"/>.
		/// </summary>
		/// <value>The visibility of this <see cref="Column"/>.</value>
		public virtual Visibility Visibility
		{
			get { return (Visibility) this.GetValue(VisibilityProperty); }
			set { this.SetValue(VisibilityProperty, value); }
		}

		/// <summary>
		/// Gets or sets the header which displays visual information about this <see cref="Column"/>.
		/// </summary>
		/// <value>The header to display the information about this <see cref="Column"/>.</value>
		public virtual object Header
		{
			get { return this.GetValue(HeaderProperty); }
			set { this.SetValue(HeaderProperty, value); }
		}

		/// <summary>
		/// Gets or sets the binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.
		/// </summary>
		/// <value>The binding which the <see cref="Cell"/>s in this <see cref="Column"/> use to get the data they display.</value>
		public virtual Binding Binding
		{
			get { return (Binding) this.GetValue(DataBindingProperty); }
			set { this.SetValue(DataBindingProperty, value); }
		}

		public virtual Binding FooterBinding
		{
			get { return (Binding) this.GetValue(FooterDataBindingProperty); }
			set { this.SetValue(FooterDataBindingProperty, value); }
		}

		/// <summary>
		/// Gets or sets the type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The type of the data in the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public virtual Type DataType
		{
			get { return (Type) this.GetValue(DataTypeProperty); }
			set { this.SetValue(DataTypeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Column"/> is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Column"/> is selected; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsSelected
		{
			get { return (bool) this.GetValue(IsSelectedProperty); }
			set { this.SetValue(IsSelectedProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Cell"/>s in this <see cref="Column"/> are read-only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the <see cref="Cell"/>s in this <see cref="Column"/> are read-only; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsReadOnly
		{
			get { return (bool) this.GetValue(IsReadOnlyProperty); }
			set { this.SetValue(IsReadOnlyProperty, value); }
		}

		/// <summary>
		/// Gets or sets the style of the <see cref="Cell"/>s in this <see cref="Column"/>.
		/// </summary>
		/// <value>The style of the <see cref="Cell"/>s in this <see cref="Column"/>.</value>
		public virtual Style CellStyle
		{
			get { return (Style) this.GetValue(CellStyleProperty); }
			set { this.SetValue(CellStyleProperty, value); }
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
			if (this.IsReadOnly != (this.Binding.Mode != BindingMode.TwoWay))
			{
				this.IsReadOnly = this.Binding.Mode != BindingMode.TwoWay;
			}
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

		private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Column) d).OnIndexChanged(e);
		}

		protected virtual void OnIndexChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.IndexChanged;
			if (handler != null)
			{
				handler(this, e);
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
