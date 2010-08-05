using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Company.Widgets.Controllers
{
	public static class StyleBinder
	{
		private static readonly Dictionary<string, DependencyProperty> dependencyProperties = new Dictionary<string, DependencyProperty>();

		/// <summary>
		/// Binding Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty BindingsProperty =
			DependencyProperty.RegisterAttached("Bindings", typeof(StyleBindingCollection), typeof(StyleBinder),
			                                    new PropertyMetadata(null, OnBindingChanged));

		/// <summary>
		/// Gets the Binding property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static StyleBindingCollection GetBindings(FrameworkElement frameworkElement)
		{
			return (StyleBindingCollection) frameworkElement.GetValue(BindingsProperty);
		}

		/// <summary>
		/// Sets the Binding property.  This dependency property 
		/// indicates ....
		/// </summary>
		public static void SetBindings(FrameworkElement frameworkElement, StyleBindingCollection value)
		{
			frameworkElement.SetValue(BindingsProperty, value);
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

			StyleBindingCollection styleBindings = (StyleBindingCollection) e.NewValue;
			foreach (StyleBinding styleBinding in styleBindings)
			{
				DependencyProperty dependencyProperty;
				if (dependencyProperties.ContainsKey(styleBinding.Property))
				{
					dependencyProperty = dependencyProperties[styleBinding.Property];
				}
				else
				{
					string dependencyPropertyName = styleBinding.Property + "Property";
					const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
					FieldInfo dependencyPropertyField = frameworkElement.GetType().GetField(dependencyPropertyName, bindingFlags);
					dependencyProperty = (DependencyProperty) dependencyPropertyField.GetValue(null);
					dependencyProperties.Add(styleBinding.Property, dependencyProperty);
				}
				frameworkElement.SetBinding(dependencyProperty, styleBinding.Binding);
			}
		}
	}
}