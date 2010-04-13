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
		/// Determines whether the focus is within the <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="dependencyObject">The <see cref="DependencyObject"/> to check for focus possession.</param>
		/// <returns>
		/// 	<c>true</c> if the focus is within the <see cref="DependencyObject"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFocusWithin(this DependencyObject dependencyObject)
		{
			object element = FocusManager.GetFocusedElement();
			while (element is DependencyObject)
			{
				if (element == dependencyObject)
				{
					return true;
				}
				element = ((DependencyObject) element).GetParent();
			}
			return false;
		}

		/// <summary>
		/// Gets the visual, if none, the logical, parent of the specified <paramref name="element"/> .
		/// </summary>
		/// <param name="element">The element to get the parent of.</param>
		/// <returns>The visual, if any, or, if none, the logical, if any, parent.</returns>
		public static DependencyObject GetParent(this DependencyObject element)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(element);
			if (parent == null && element is FrameworkElement)
			{
				return ((FrameworkElement) element).Parent;
			}
			return parent;
		}
	}
}


