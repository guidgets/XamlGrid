// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	SumAggregate.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Data;

namespace Guidgets.XamlGrid.Controllers
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
