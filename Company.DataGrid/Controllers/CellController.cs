﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
			return new List<int> { Notifications.FOCUS_CELL };
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
			switch (notification.Code)
			{
				case Notifications.FOCUS_CELL:
					object[] data = (object[]) notification.Body;
					if (this.Cell.DataContext == data[0] && this.Cell.Column == data[1] && !this.Cell.HasFocus)
					{
						this.Cell.Focus();
					}
					break;
			}
		}

		private void FocusHorizontalNeighbor(bool next)
		{
			DependencyObject dependencyObject = this.Cell;
			while (dependencyObject != null)
			{
				Control control = dependencyObject as Control;
				if (control != null)
				{
					IEnumerable<Control> siblings = from sibling in control.GetVisualSiblingsAndSelf().OfType<Control>()
					                                orderby sibling.TabIndex
					                                select sibling;
					if (!next)
					{
						siblings = siblings.Reverse();
					}
					Func<Control, bool> focus = c => ((next || c.IsTabStop) && c.Focus()) ||
					                                 GetChildControls(c).LastOrDefault(v => v.Focus()) != null;
					if (siblings.SkipWhile(sibling => sibling != control).Skip(1).Any(focus))
					{
						return;
					}
				}
				dependencyObject = dependencyObject.GetParent();
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

        private void Cell_LostFocus (object sender, RoutedEventArgs e)
        {
            this.Cell.IsInEditMode = false;
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
							this.FocusHorizontalNeighbor(true);
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
					this.FocusHorizontalNeighbor(e.Key == Key.Right);
					e.Handled = true;
					break;
			}
		}

		private void Cell_IsInEditModeChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			this.SendNotification(Notifications.CELL_EDIT_MODE_CHANGED, this.Cell.IsInEditMode);
		}
	}
}
