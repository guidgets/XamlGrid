using System.Collections;
using System.Collections.Generic;
using Company.DataGrid.Core;
using Company.DataGrid.Views;

namespace Company.DataGrid.Controllers
{
	/// <summary>
	/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Cell"/>.
	/// </summary>
	public class CellController : Controller
	{
		/// <summary>
		/// Represents a <see cref="Controller"/> which is responsible for the functionality of a <see cref="Views.Cell"/>.
		/// </summary>
		/// <param name="cell">The cell for which functionality the <see cref="Controller"/> is responsible.</param>
		public CellController(Cell cell) : base(cell.GetHashCode().ToString(), cell)
		{

		}

		/// <summary>
		/// Gets the cell for which functionality the <see cref="CellController"/> is responsible.
		/// </summary>
		public Cell Cell
		{
			get
			{
				return (Cell) this.ViewComponent;
			}
		}


		/// <summary>
		/// Called by the <see cref="Controller"/> when it is registered.
		/// </summary>
		public override void OnRegister()
		{
			base.OnRegister();

			this.SendNotification(Notifications.IS_ITEM_SELECTED, this.Cell.DataContext);
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<string> ListNotificationInterests()
		{
			return new List<string>
			       	{
			       		Notifications.SELECTED_ITEMS,
			       		Notifications.DESELECTED_ITEMS,
			       		Notifications.ITEM_IS_SELECTED
			       	};
		}

		/// <summary>
		/// Handle <c>INotification</c>s.
		/// </summary>
		/// <param name="notification">The <c>INotification</c> instance to handle</param>
		/// <remarks>
		/// Typically this will be handled in a switch statement, with one 'case' entry per <c>INotification</c> the <c>Controller</c> is interested in.
		/// </remarks>
		public override void HandleNotification(INotification notification)
		{
			switch (notification.Name)
			{
				case Notifications.SELECTED_ITEMS:
					bool isSelected = ((IList) notification.Body).Contains(this.Cell.DataContext);
					if (isSelected)
					{
						this.Cell.IsSelected = this.Cell.Column.IsSelected;						
					}
					break;
				case Notifications.DESELECTED_ITEMS:
					IList list = (IList) notification.Body;
					if (list.Contains(this.Cell.DataContext) || list.Count == 0)
					{
						this.Cell.IsSelected = false;
					}
					break;
				case Notifications.ITEM_IS_SELECTED:
					if (this.Cell.DataContext == notification.Body)
					{
						this.Cell.IsSelected = bool.Parse(notification.Type);
					}
					break;
			}
		}
	}
}
