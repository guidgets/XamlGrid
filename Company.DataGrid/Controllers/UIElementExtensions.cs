using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.DataGrid.Controllers
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
			object parent = FocusManager.GetFocusedElement();
			do
			{
				if (parent == uiElement)
				{
					return true;
				}
				if (parent is DependencyObject)
				{
					DependencyObject upperParent = VisualTreeHelper.GetParent((DependencyObject) parent);
					if (upperParent == null)
					{
						if (parent is FrameworkElement)
						{
							parent = ((FrameworkElement) parent).Parent;
						}
					}
					else
					{
						parent = upperParent;
					}
				}
			}
			while (parent != null);
			return false;
		}
	}
}
