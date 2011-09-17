using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;

namespace XamlGrid.Controllers
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

			ParameterExpression par1 = Expression.Parameter(typeof(object), "p1");
			ParameterExpression par2 = Expression.Parameter(typeof(object), "p2");
			BinaryExpression binaryExpression = Expression.Add(Expression.Convert(par1, dataType), Expression.Convert(par2, dataType));
			UnaryExpression unaryExpression = Expression.Convert(binaryExpression, typeof(object));
			LambdaExpression lambdaExpression = Expression.Lambda(unaryExpression, par1, par2);
			Func<object, object, object> @delegate = (Func<object, object, object>) lambdaExpression.Compile();

			return values.Aggregate(0, @delegate);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
