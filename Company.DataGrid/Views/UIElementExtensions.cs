using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Contains methods that extend the functionality of <see cref="UIElement"/>.
	/// </summary>
	public static class UIElementExtensions
	{
		/// <summary>
		/// Determines whether the focus is within the <see cref="UIElement"/>.
		/// </summary>
		/// <param name="uiElement">The <see cref="UIElement"/> to check for focus possession.</param>
		/// <returns>
		/// 	<c>true</c> if the focus is within the <see cref="UIElement"/>; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsFocusWithin(this UIElement uiElement)
		{
			object element = FocusManager.GetFocusedElement();
			while (element is DependencyObject)
			{
				if (element == uiElement)
				{
					return true;
				}
				DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) element);
				if (parent == null)
				{
					if (element is FrameworkElement)
					{
						element = ((FrameworkElement) element).Parent;
					}
					else
					{
						return false;
					}
				}
				else
				{
					element = parent;
				}
			}
			return false;
		}
	}
}


