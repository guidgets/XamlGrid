using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a GUI element displaying a single characteristic of a data object, or a simple single value.
	/// </summary>
	public abstract class CellBase : ContentControl
	{
		/// <summary>
		/// Identifies the property which gets or sets the column to which the <see cref="CellBase"/> belongs.
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
		public Column Column
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


		private void Cell_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.Column.Width.IsAuto && (double.IsNaN(this.Column.ActualWidth) || this.Column.ActualWidth < e.NewSize.Width))
			{
				this.Column.ActualWidth = e.NewSize.Width + 1;
			}
		}
	}
}
