using System;
using System.Windows;
using System.Windows.Input;
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

			this.Cell.GotFocus += this.Cell_GotFocus;
			this.Cell.KeyUp += this.Cell_KeyUp;
			this.Cell.IsInEditModeChanged += this.Cell_IsInEditModeChanged;
			this.Cell.EditingCancelled += this.Cell_EditingCancelled;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Cell.GotFocus -= this.Cell_GotFocus;
			this.Cell.KeyUp -= this.Cell_KeyUp;
			this.Cell.IsInEditModeChanged -= this.Cell_IsInEditModeChanged;
			this.Cell.EditingCancelled -= this.Cell_EditingCancelled;
		}


		private void Cell_GotFocus(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.CELL_FOCUSED, this.Cell);
		}

		private void Cell_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					this.Cell.IsInEditMode = !this.Cell.IsInEditMode;
					if (!this.Cell.IsInEditMode)
					{
						this.Cell.FocusNext();
					}
					break;
				case Key.Escape:
					this.Cell.CancelEdit();
					break;
			}
		}

		private void Cell_IsInEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CELL_EDIT_MODE_CHANGED, this.Cell.IsInEditMode);
		}

		private void Cell_EditingCancelled(object sender, EventArgs e)
		{
			this.SendNotification(Notifications.CELL_EDITING_CANCELLED);
		}
	}
}
