using System.Windows;
using System.Reflection;

namespace Company.DataGrid.Controllers
{
	public static class StyleBinder
	{
		/// <summary>
		/// Binding Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty BindingProperty =
			DependencyProperty.RegisterAttached("Binding", typeof(StyleBinding), typeof(StyleBinder),
			                                    new PropertyMetadata(null, OnBindingChanged));

		/// <summary>
		/// Gets the Binding property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static StyleBinding GetBinding(FrameworkElement frameworkElement)
		{
			return (StyleBinding) frameworkElement.GetValue(BindingProperty);
		}

		/// <summary>
		/// Sets the Binding property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetBinding(FrameworkElement frameworkElement, StyleBinding value)
		{
			frameworkElement.SetValue(BindingProperty, value);
		}

		/// <summary>
		/// By using reflection, we can find the dependency property of the target element
		/// and then apply a binding to that property.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement) d;

			StyleBinding styleBinding = (StyleBinding) e.NewValue;
			string depPropName = styleBinding.Property + "Property";
			if (depPropName.IndexOf('.') > -1)
			{
				int index = depPropName.LastIndexOf('.');
				depPropName = depPropName.Substring(index);
			}
			FieldInfo dependencyPropertyField = frameworkElement.GetType().GetField(depPropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (dependencyPropertyField != null)
			{
				DependencyProperty dependencyProperty = dependencyPropertyField.GetValue(null) as DependencyProperty;
				if (dependencyProperty != null)
				{
					frameworkElement.SetBinding(dependencyProperty, styleBinding.Binding);
				}
			}
		}
	}
}