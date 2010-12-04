using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Company.Widgets.Controllers
{
	public class SumAggregate : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IEnumerable<object> values = from object item in (IEnumerable) value
			                             let result = DataBinder.GetValue(item, (string) parameter)
			                             where result != null
			                             select result;
			object firstValue = values.FirstOrDefault();
			if (firstValue == null)
			{
				return 0;
			}
			Type dataType = firstValue.GetType();
			MethodInfo cast = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(new[] { dataType });
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static |
			                                  BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod;
			return (from method in typeof(Enumerable).GetMethods(bindingFlags)
			        where method.Name == "Sum" && method.ReturnType == dataType
			        select method).First().Invoke(null, new[] { cast.Invoke(null, new[] { values }) });
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
