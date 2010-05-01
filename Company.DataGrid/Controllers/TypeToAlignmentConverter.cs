using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Obtains a <see cref="HorizontalAlignment"/> according to a supplied <see cref="Type"/>.
	/// </summary>
	public class TypeToAlignmentConverter : IValueConverter
	{
		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		/// <param name="value">The source data being passed to the target.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the target dependency property.
		/// </returns>
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Type type = (Type) value;
			if (type.IsNumeric() || type == typeof(DateTime) || type == typeof(DateTime?))
			{
				return HorizontalAlignment.Right;
			}
			if (type == typeof(bool) || type == typeof(bool?) || type == typeof(byte[]) || type == typeof(Uri))
			{
				return HorizontalAlignment.Center;
			}
			return HorizontalAlignment.Left;
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <param name="value">The target data being passed to the source.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the source object.
		/// </returns>
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
