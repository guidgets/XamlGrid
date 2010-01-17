using System.ComponentModel;
using System.Windows;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Converts a <see cref="string"/> to a <see cref="ColumnWidth"/> object.
	/// </summary>
	public class StringToColumnWidthConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether the type converter can convert an object from the specified type to the type of this converter.
		/// </summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="sourceType">The type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts from the specified value to the intended conversion type of the converter.
		/// </summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The value to convert to the type of this converter.</param>
		/// <returns>The converted value.</returns>
		/// <exception cref="T:System.NotImplementedException">
		/// 	<see cref="M:System.ComponentModel.TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)"/> not implemented in base <see cref="T:System.ComponentModel.TypeConverter"/>.</exception>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			switch ((string) value)
			{
				case "*":
					return new ColumnWidth(0, GridUnitType.Star);
				case "Auto":
					return new ColumnWidth(0, GridUnitType.Auto);
				default:
					return new ColumnWidth(double.Parse(value.ToString()), GridUnitType.Pixel);
			}
		}
	}
}
