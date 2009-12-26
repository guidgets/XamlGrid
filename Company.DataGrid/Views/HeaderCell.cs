using System.Windows;
using System.Windows.Controls.Primitives;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a cell that stays as a header for a <see cref="Controllers.Column"/>.
	/// </summary>
	public class HeaderCell : ToggleButton
	{
		/// <summary>
		/// Identifies the property which gets or sets the column to which a <see cref="HeaderCell"/> belongs.
		/// </summary>
		public static readonly DependencyProperty ColumnProperty =
			DependencyProperty.Register("Column", typeof(Column), typeof(HeaderCell), new PropertyMetadata(OnColumnChanged));

		private static void OnColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((HeaderCell) d).Content = ((Column) e.NewValue).Header;
		}

		public HeaderCell()
		{
			this.DefaultStyleKey = typeof(HeaderCell);

			DataGridFacade.Instance.RegisterController(new HeaderCellContoller(this));
		}

		/// <summary>
		/// Gets or sets the column to which the <see cref="HeaderCell"/> belongs.
		/// </summary>
		/// <value>The column.</value>
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
	}
}
