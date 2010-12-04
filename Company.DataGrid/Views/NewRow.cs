using Company.Widgets.Controllers;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Represents an empty <see cref="Row"/> that serves to fill in and enter new items to the set of <see cref="Row"/>s.
	/// </summary>
	public class NewRow : Row
	{
		/// <summary>
		/// Represents an empty <see cref="Row"/> that serves to fill in and enter new items to the set of <see cref="Row"/>s.
		/// </summary>
		public NewRow()
		{
			DataGridFacade.Instance.RemoveController(this.GetHashCode().ToString());
			DataGridFacade.Instance.RegisterController(new NewRowController(this));
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride (System.Windows.DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride (element, item);

            Cell cell = (Cell) element;
            DataGridFacade.Instance.RemoveController(cell.GetHashCode().ToString());
            cell.IsInEditMode = true;
        }
	}
}
