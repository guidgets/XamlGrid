using System.Collections.ObjectModel;
using System.Windows;
using XamlGrid.Core;

namespace XamlGrid.Controllers
{
	/// <summary>
	/// Represents a collection of controllers associated with the same <see cref="DependencyObject"/>.
	/// </summary>
	public class ControllerCollection : ObservableCollection<Controller>
	{
		private DependencyObject _associatedObject;

		/// <summary>
		/// Represents a collection of controllers associated with the same <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="o"></param>
		public ControllerCollection(DependencyObject o)
		{
			_associatedObject = o;
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert.</param>
		protected override void InsertItem(int index, Controller item)
		{
			base.InsertItem(index, item);

			item.ViewComponent = _associatedObject;

			DataGridFacade.Instance.RegisterController(item);
		}

		/// <summary>
		/// Removes the item at the specified index from the collection.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		protected override void RemoveItem(int index)
		{
			Controller controller = this[index];

			controller.ViewComponent = null;

			base.RemoveItem(index);

			DataGridFacade.Instance.RemoveController(controller.Name);
		}
	}
}
