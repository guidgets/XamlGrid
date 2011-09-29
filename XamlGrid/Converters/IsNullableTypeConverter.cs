using System;
using System.Globalization;
using System.Windows.Data;

namespace XamlGrid.Controllers
{
	public class IsNullableTypeConverter : IValueConverter
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
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Type type = (Type) value;
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
