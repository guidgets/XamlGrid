using System;
using System.ComponentModel;
using System.Globalization;
using XamlGrid.Aspects;
using XamlGrid.Models;

namespace XamlGrid.Controllers
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
		/// <c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
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
		[Validate]
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, [NotEmpty] object value)
		{
			string width = (string) value;
			switch (width)
			{
				case "Auto":
					return new ColumnWidth(SizeMode.Auto);
				case "Header":
					return new ColumnWidth(SizeMode.ToHeader);
				case "Data":
					return new ColumnWidth(SizeMode.ToData);
				case "Fill":
					return new ColumnWidth(SizeMode.Fill);
			}
			double widthValue;
			NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
			if (width[width.Length - 1] == '*')
			{
				if (width.Length == 1)
				{
					return new ColumnWidth(SizeMode.Fill);
				}
				if (double.TryParse(width.Remove(width.Length - 1), NumberStyles.Any, numberFormat, out widthValue))
				{
					return new ColumnWidth(widthValue, SizeMode.Fill);
				}
			}
			if (double.TryParse(width, NumberStyles.Any, numberFormat, out widthValue))
			{
				return new ColumnWidth(widthValue);
			}
			return null;
		}
	}
}