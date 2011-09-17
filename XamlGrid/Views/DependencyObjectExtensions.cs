using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace XamlGrid.Views
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
		public static bool HasFocus(this DependencyObject element)
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

		public static void FocusHorizontalNeighbour(this DependencyObject dependencyObject, bool next)
		{
			DependencyObject current = dependencyObject;
			while (current != null)
			{
				int indexOfCurrent = -1;
				int currentIndex = 0;

				foreach (DependencyObject sibling in current.GetVisualSiblingsAndSelf())
				{
					indexOfCurrent++;
					if (current == sibling)
					{
						break;
					}
				}
				DependencyObject c = current;
				IEnumerable<Control> neighbours = (from sibling in current.GetVisualSiblings()
												   let descendant = (from descendant in sibling.GetVisualDescendantsAndSelf().OfType<Control>()
																	 where descendant.IsEnabled && descendant.IsTabStop
																	 select descendant).FirstOrDefault()
												   where descendant != null
												   let descendants = (from descendantSibling in descendant.GetVisualSiblingsAndSelf().OfType<Control>()
																	  where descendantSibling != c && descendantSibling.IsEnabled && descendantSibling.IsTabStop
																	  orderby descendantSibling.TabIndex
																	  select descendantSibling)
												   let child = descendant == sibling ? descendant : next ? descendants.FirstOrDefault() : descendants.LastOrDefault()
												   let tabIndex = c is Control ? ((Control) c).TabIndex : next ? int.MaxValue : int.MinValue
												   where child != null && ((next && (child.TabIndex > tabIndex || currentIndex++ >= indexOfCurrent)) ||
														 (!next && (child.TabIndex < tabIndex || currentIndex++ < indexOfCurrent)))
												   orderby child.TabIndex
												   select child);

				Control neighbour = next ? neighbours.FirstOrDefault() : neighbours.LastOrDefault();
				if (neighbour != null && neighbour.Focus())
				{
					return;
				}
				current = current.GetParent();
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


