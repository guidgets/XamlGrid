using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;
using Company.DataGrid.Models;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a GUI element displaying a single characteristic of a data object, or a simple single value.
	/// </summary>
	public abstract class CellBase : ContentControl
	{
		/// <summary>
		/// Identifies the dependency property which gets or sets the column to which the <see cref="CellBase"/> belongs.
		/// </summary>
		public static readonly DependencyProperty ColumnProperty =
			DependencyProperty.Register("Column", typeof(Column), typeof(CellBase), new PropertyMetadata(null));


		/// <summary>
		/// Represents a GUI element displaying a single characteristic of a data object, or a simple single value.
		/// </summary>
		protected CellBase()
		{
			this.SizeChanged += this.Cell_SizeChanged;
		}


		/// <summary>
		/// Gets or sets the column to which the <see cref="CellBase"/> belongs.
		/// </summary>
		/// <value>The column to which the <see cref="CellBase"/> belongs.</value>
		public virtual Column Column
		{
			get
			{
				return (Column) this.GetValue(ColumnProperty);
			}
			set
			{
				this.SetValue(ColumnProperty, value);
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


		private void Cell_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.IsAutoSized() && (double.IsNaN(this.Column.ActualWidth) || this.Column.ActualWidth < e.NewSize.Width))
			{
				this.Column.ActualWidth = e.NewSize.Width + 1;
			}
		}
	}
}
