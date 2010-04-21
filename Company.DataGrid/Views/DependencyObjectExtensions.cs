using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Contains methods that extend the functionality of <see cref="DependencyObject"/>.
	/// </summary>
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Determines whether the focus is within the <paramref name="element"/>.
		/// </summary>
		/// <param name="element">The <see cref="DependencyObject"/> to check for focus possession.</param>
		/// <returns>
		/// 	<c>true</c> if the focus is within the <see cref="DependencyObject"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFocusWithin(this DependencyObject element)
		{
			object currentElement = FocusManager.GetFocusedElement();
			while (true)
			{
				if (currentElement == element)
				{
					return true;
				}
				DependencyObject dependencyObject = currentElement as DependencyObject;
				if (dependencyObject == null)
				{
					return false;
				}
				currentElement = dependencyObject.GetParent();
			}
		}

		/// <summary>
		/// Gets the visual, if none, the logical, parent of the specified <paramref name="element"/> .
		/// </summary>
		/// <param name="element">The element to get the parent of.</param>
		/// <returns>The visual, if any, or, if none, the logical, if any, parent.</returns>
		public static DependencyObject GetParent(this DependencyObject element)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			if (parent == null)
			{
				FrameworkElement frameworkElement = element as FrameworkElement;
				if (frameworkElement != null)
				{
					return frameworkElement.Parent;
				}
			}
			return parent;
		}
	}
}


