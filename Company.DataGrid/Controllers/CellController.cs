using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Company.Widgets.Core;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
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
		public virtual Cell Cell
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
            this.Cell.LostFocus += this.Cell_LostFocus;
			this.Cell.KeyDown += this.Cell_KeyDown;
			this.Cell.IsInEditModeChanged += this.Cell_IsInEditModeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Cell.GotFocus -= this.Cell_GotFocus;
		    this.Cell.LostFocus -= this.Cell_LostFocus;
			this.Cell.KeyDown -= this.Cell_KeyDown;
			this.Cell.IsInEditModeChanged -= this.Cell_IsInEditModeChanged;
		}

		/// <summary>
		/// List the <c>INotification</c> names this <c>Controller</c> is interested in being notified of.
		/// </summary>
		/// <returns>The list of <c>INotification</c> names.</returns>
		public override IList<int> ListNotificationInterests()
		{
			return new List<int> { Notifications.FocusCell };
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
			base.HandleNotification(notification);
			switch (notification.Code)
			{
				case Notifications.FocusCell:
					object[] data = (object[]) notification.Body;
					if (this.Cell.DataContext == data[0] && this.Cell.Column == data[1] && !this.Cell.HasFocus)
					{
						this.Cell.Focus();
					}
					break;
			}
		}


		private void Cell_GotFocus(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.CellFocused, this.Cell);
		}

		private void Cell_LostFocus(object sender, RoutedEventArgs e)
		{
			if (!this.Cell.HasFocus)
			{
				this.Cell.IsInEditMode = false;
			}
		}

		private void Cell_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.F2:
					this.Cell.IsInEditMode = true;
					break;
				case Key.Enter:
					if (!EditorController.SentFromMultilineTextBox(e))
					{
						this.Cell.IsInEditMode = !this.Cell.IsInEditMode;
						if (!this.Cell.IsInEditMode)
						{
							this.Cell.FocusHorizontalNeighbour(true);
						}
					}
					break;
				case Key.Escape:
					if (this.Cell.IsInEditMode)
					{
						this.Cell.IsInEditMode = false;
						// HACK: moving from edit state to view state causes the cell to lose focus somehow
						this.Cell.Focus();
					}
					break;
				case Key.Left:
				case Key.Right:
					if ((Keyboard.Modifiers & KeyHelper.CommandModifier) == ModifierKeys.None)
					{
						this.Cell.FocusHorizontalNeighbour(e.Key == Key.Right);
					}
					break;
			}
		}

		private void Cell_IsInEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CellEditModeChanged, this.Cell.IsInEditMode);
		}
	}
}
