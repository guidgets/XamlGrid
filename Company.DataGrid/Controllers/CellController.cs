using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
			this.Cell.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Cell_KeyDown), true);
			this.Cell.IsInEditModeChanged += this.Cell_IsInEditModeChanged;
		}

		/// <summary>
		/// Called by the <see cref="Controller"/> when it is removed.
		/// </summary>
		public override void OnRemove()
		{
			base.OnRemove();

			this.Cell.GotFocus -= this.Cell_GotFocus;
			this.Cell.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Cell_KeyDown));
			this.Cell.IsInEditModeChanged -= this.Cell_IsInEditModeChanged;
		}


		private void FocusNeighbor(bool next)
		{
			DependencyObject dependencyObject = this.Cell;
			while (true)
			{
				IEnumerable<Control> siblings = from sibling in dependencyObject.GetVisualSiblingsAndSelf().OfType<Control>()
				                                orderby sibling.TabIndex
				                                select sibling;
				List<Control> siblingList = next ? siblings.ToList() : siblings.Reverse().ToList();
				Func<Control, bool> focus = c => ((next || c.IsTabStop) && c.Focus()) ||
				                                 GetChildControls(c).LastOrDefault(v => v.Focus()) != null;
				Control control = dependencyObject as Control;
				if (control != null)
				{
					Control childControl = control;
					if ((from sibling in siblingList
						 where sibling != childControl && sibling.TabIndex == childControl.TabIndex &&
							   siblingList.IndexOf(sibling) > siblingList.IndexOf(childControl)
						 select sibling).Any(focus) ||
						(from sibling in siblingList
						 where sibling.TabIndex > childControl.TabIndex
						 select sibling).Any(focus))
					{
						return;
					}
				}

				dependencyObject = dependencyObject.GetParent();
				if (dependencyObject == null)
				{
					return;
				}
			}
		}

		private static IEnumerable<Control> GetChildControls(DependencyObject parent)
		{
			List<Control> childControls = new List<Control>();
			Control control = parent as Control;
			if (control != null)
			{
				childControls.Add(control);
			}
			foreach (DependencyObject dependencyObject in parent.GetVisualChildren())
			{
				childControls.AddRange(GetChildControls(dependencyObject));
			}
			return childControls;
		}


		private void Cell_GotFocus(object sender, RoutedEventArgs e)
		{
			this.SendNotification(Notifications.CELL_FOCUSED, this.Cell);
		}

		private void Cell_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Enter:
					this.Cell.IsInEditMode = !this.Cell.IsInEditMode;
					if (!this.Cell.IsInEditMode)
					{
						FocusNeighbor(true);
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
					if (!e.Handled)
					{
						FocusNeighbor(e.Key == Key.Right);						
					}
					break;
			}
		}

		private void Cell_IsInEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CELL_EDIT_MODE_CHANGED, this.Cell.IsInEditMode);
		}
	}
}
