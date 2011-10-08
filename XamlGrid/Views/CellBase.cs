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
// File:	CellBase.cs
// Authors:	Dimitar Dobrev <dpldobrev@yahoo.com>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XamlGrid.Controllers;
using XamlGrid.Models;

namespace XamlGrid.Views
{
	/// <summary>
	/// Represents a GUI element displaying a single characteristic of a data object, or a simple single value.
	/// </summary>
	public abstract class CellBase : ContentControl
	{
		/// <summary>
		/// Identifies the dependency property which gets or sets the column to which the <see cref="CellBase"/> belongs.
		/// </summary>
		private static readonly DependencyProperty columnProperty =
			DependencyProperty.Register("Column", typeof(Column), typeof(CellBase), new PropertyMetadata(null));

		private static readonly DependencyProperty rowIndexProperty =
			DependencyProperty.Register("RowIndex", typeof(int), typeof(Cell), new PropertyMetadata(-1));


		/// <summary>
		/// Represents a GUI element displaying a single characteristic of a data object, or a simple single value.
		/// </summary>
		protected CellBase()
		{
			this.SizeChanged += this.Cell_SizeChanged;
		}


		/// <summary>
		/// Gets the column to which the <see cref="CellBase"/> belongs.
		/// </summary>
		public virtual Column Column
		{
			get
			{
				return (Column) this.GetValue(columnProperty);
			}
		}

		/// <summary>
		/// Gets the index of the <see cref="Row"/> to which this <see cref="Cell"/> belongs.
		/// </summary>
		public int RowIndex
		{
			get { return (int) this.GetValue(rowIndexProperty); }
		}

		/// <summary>
		/// Called when the value of the <see cref="ContentControl.Content"/> property changes.
		/// </summary>
		/// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
		/// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			if (this.Column == null)
			{
				Column column = newContent as Column;
				if (column != null)
				{
					this.SetValue(columnProperty, column);
				}
			}
		}


		/// <summary>
		/// Determines whether the <see cref="Cell"/> is automatically sized according to its contents.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the <see cref="Cell"/> is automatically sized according to its contents; otherwise, <c>false</c>.
		/// </returns>
		protected virtual bool IsAutoSized()
		{
			return this.Column.Width.SizeMode == SizeMode.Auto;
		}

		public virtual void BindRowIndex(Binding binding)
		{
			this.SetBinding(rowIndexProperty, binding);
		}

		private void Cell_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.IsAutoSized() && (double.IsNaN(this.Column.ActualWidth) || this.Column.ActualWidth < e.NewSize.Width))
			{
				this.Column.ActualWidth = e.NewSize.Width + 1;
			}
		}
	}
}
