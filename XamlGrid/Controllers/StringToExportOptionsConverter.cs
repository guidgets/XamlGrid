using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using XamlGrid.Aspects;
using XamlGrid.Models.Export;

namespace XamlGrid.Controllers
{
	public class StringToExportOptionsConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether the type converter can convert an object from the specified type to the type of this converter.
		/// </summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="sourceType">The type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
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
		/// <returns>
		/// The converted value.
		/// </returns>
		/// <exception cref="T:System.NotImplementedException">
		///   <see cref="M:System.ComponentModel.TypeConverter.ConvertFrom(System.ComponentModel.ITypeDescriptorContext,System.Globalization.CultureInfo,System.Object)"/> not implemented in base <see cref="T:System.ComponentModel.TypeConverter"/>.</exception>
		[Validate]
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture,
		                                   [ValidStringEnum(typeof(ExportOptions))] object value)
		{
			string source = (string) value;
			return source.Split('|').Aggregate(ExportOptions.None,
			                                   (current, enumValue) =>
			                                   current | (ExportOptions) Enum.Parse(typeof(ExportOptions), enumValue, true));
		}
	}
}
