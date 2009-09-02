using System.Windows;
using System.Windows.Controls;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents an extended <see cref="ContentPresenter"/> that has an object property to maintain a connection to its <see cref="ContentControl"/>.
	/// </summary>
	public class ValueContentPresenter : ContentPresenter
	{
		/// <summary>
		/// Gets or sets an object value to be transfered back and forth between a <see cref="ContentControl"/> and its <see cref="ContentControl.ContentTemplate"/>.
		/// </summary>
		/// <value>The value.</value>
		public object Value
		{
			get
			{
				return this.GetValue(ValueProperty);
			}
			set
			{
				this.SetValue(ValueProperty, value);
			}
		}

		/// <summary>
		/// Identifies the <see cref="Value"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(ValueContentPresenter), new PropertyMetadata(null));
	}
}