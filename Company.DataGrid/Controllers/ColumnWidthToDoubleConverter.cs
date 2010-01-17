using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Company.DataGrid.Models;

namespace Company.DataGrid.Controllers
{
	public class ColumnWidthToDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ColumnWidth gridLength = (ColumnWidth) value;
			if (gridLength.UnitType == GridUnitType.Auto && gridLength.Value == 0)
			{
				return double.NaN;
			}
			return gridLength.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double number = (double) value;
			return number == double.NaN ? new ColumnWidth(0, GridUnitType.Auto) : new ColumnWidth(number, GridUnitType.Pixel);
		}
	}
}
