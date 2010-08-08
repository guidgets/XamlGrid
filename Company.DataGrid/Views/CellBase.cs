using System.Windows;
using System.Windows.Controls;
using Company.Widgets.Controllers;
using Company.Widgets.Models;

namespace Company.Widgets.Views
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
				return (Column) this.GetValue(columnProperty);
			}
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


		private void Cell_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.IsAutoSized() && (double.IsNaN(this.Column.ActualWidth) || this.Column.ActualWidth < e.NewSize.Width))
			{
				this.Column.ActualWidth = e.NewSize.Width + 1;
			}
		}
	}
}
