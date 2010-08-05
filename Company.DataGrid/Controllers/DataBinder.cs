using System.Windows;
using System.Windows.Data;

namespace Company.Widgets.Controllers
{
	public static class DataBinder
	{
		private class ValueGetter : DependencyObject
		{
			public object Value
			{
				get
				{
					return this.GetValue(ValueProperty);
				}
			}

			public static readonly DependencyProperty ValueProperty =
				DependencyProperty.Register("Value", typeof(object), typeof(ValueGetter), new PropertyMetadata(null));
		}

		public static object GetValue(object dataItem, string propertyPath)
		{
			ValueGetter valueGetter = new ValueGetter();
			Binding workingBinding = new Binding(propertyPath) { Source = dataItem };
			BindingOperations.SetBinding(valueGetter, ValueGetter.ValueProperty, workingBinding);
			object value = valueGetter.Value;
			valueGetter.ClearValue(ValueGetter.ValueProperty);
			return value;
		}
	}
}
