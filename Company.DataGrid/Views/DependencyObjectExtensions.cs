using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Linq;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Extends the functionality of an <see cref="ItemsControl"/>.
	/// </summary>
	public static class ItemsControlExtensions
	{
		/// <summary>
		/// Gets the <see cref="ItemsPresenter"/>, if any, of the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="dependencyObject">The <see cref="DependencyObject"/> to get the <see cref="ItemsPresenter"/> of.</param>
		/// <returns>The <see cref="ItemsPresenter"/>, if any, of the specified <see cref="DependencyObject"/></returns>
		public static ItemsPresenter GetItemsPresenter(this DependencyObject dependencyObject)
		{
			if (dependencyObject is ItemsPresenter)
			{
				return (ItemsPresenter) dependencyObject;
			}
			DependencyObject content = null;
			if (dependencyObject is ContentControl)
			{
				content = ((ContentControl) dependencyObject).Content as DependencyObject;
			}
			if (dependencyObject is ContentPresenter)
			{
				content = ((ContentPresenter) dependencyObject).Content as DependencyObject;
			}
			if (content != null)
			{
				ItemsPresenter itemsPresenter = GetItemsPresenter(content);
				if (itemsPresenter != null)
				{
					return itemsPresenter;
				}
			}
			return (from descendantControl in dependencyObject.GetVisualDescendants()
					let itemsPresenter = GetItemsPresenter(descendantControl)
					where itemsPresenter != null
					select itemsPresenter).FirstOrDefault();
		}
	}
}
