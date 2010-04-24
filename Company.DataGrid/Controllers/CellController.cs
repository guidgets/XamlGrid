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


		private void FocusHorizontalNeighbor(bool next)
		{
			DependencyObject dependencyObject = this.Cell;
			while (dependencyObject != null)
			{
				Control control = dependencyObject as Control;
				if (control == null)
				{
					dependencyObject = dependencyObject.GetParent();
					continue;
				}
				IEnumerable<Control> siblings = from sibling in control.GetVisualSiblingsAndSelf().OfType<Control>()
				                                orderby sibling.TabIndex
				                                select sibling;
				if (!next)
				{
					siblings = siblings.Reverse();
				}
				Func<Control, bool> focus = c => ((next || c.IsTabStop) && c.Focus()) ||
				                                 GetChildControls(c).LastOrDefault(v => v.Focus()) != null;
				if (siblings.SkipWhile(s => s != control).Skip(1).Any(focus))
				{
					return;
				}
			}
		}

		private void FocusVerticalNeighbor(bool next)
		{
			DependencyObject parent = this.Cell.GetParent();
			while (parent != null)
			{
				ItemsControl itemsControl = parent as ItemsControl;
				if (itemsControl == null)
				{
					parent = parent.GetParent();
					continue;
				}
				IEnumerable<ItemsControl> siblings = from sibling in itemsControl.GetVisualSiblingsAndSelf().OfType<ItemsControl>()
				                                     orderby sibling.TabIndex
				                                     select sibling;
				if (!next)
				{
					siblings = siblings.Reverse();
				}
				ItemsControl neighbor = siblings.SkipWhile(sibling => sibling != itemsControl).Skip(1).FirstOrDefault();
				if (neighbor == null)
				{
					return;
				}
				int index = neighbor.Items.IndexOf(this.Cell.Column);
				Control cell = neighbor.ItemContainerGenerator.ContainerFromIndex(index) as Control;
				if (cell != null)
				{
					cell.Focus();
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
						this.FocusHorizontalNeighbor(true);
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
						this.FocusHorizontalNeighbor(e.Key == Key.Right);						
					}
					break;
				case Key.Up:
				case Key.Down:
					if (!e.Handled)
					{
						this.FocusVerticalNeighbor(e.Key == Key.Down);
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
