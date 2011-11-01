// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	Row.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using Guidgets.XamlGrid.Automation;
using Guidgets.XamlGrid.Controllers;

namespace Guidgets.XamlGrid.Views
{
	/// <summary>
	/// Represents a UI element that displays a data object.
	/// </summary>
	public class Row : RowBase
	{
		/// <summary>
		/// Occurs when the <see cref="Row"/> changes its currency.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler HasFocusChanged;
		/// <summary>
		/// Occurs when the <see cref="Row"/> changes its selection state.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler IsSelectedChanged;

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Row"/> is the current one.
		/// </summary>
		public static readonly DependencyProperty HasFocusProperty =
			DependencyProperty.Register("IsFocused", typeof(bool), typeof(Row), new PropertyMetadata(false, OnHasFocusedChanged));

		private static readonly Type rowType = typeof(Row);

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Row"/> is selected.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(Row), new PropertyMetadata(false, OnIsSelectedChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the index of this <see cref="Row"/> in a collection of rows.
		/// </summary>
		public static readonly DependencyProperty IndexProperty =
			DependencyProperty.Register("Index", typeof(int), typeof(Row), new PropertyMetadata(-1));


		private static readonly Binding isSelectedBinding = new Binding("IsSelected")
															{
																RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = rowType }
															};

		private static readonly Binding indexBinding = new Binding("Index")
														{
															RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = rowType }
														};

		/// <summary>
		/// Represents a UI element that displays a data object.
		/// </summary>
		public Row()
		{
			this.DefaultStyleKey = typeof(Row);

			DataGridFacade.Instance.RegisterController(new RowController(this));
		}


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Row"/> is the current one.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Row"/> is the current one; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsFocused
		{
			get
			{
				return (bool) this.GetValue(HasFocusProperty);
			}
			set
			{
				this.SetValue(HasFocusProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Row"/> is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Row"/> is selected; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsSelected
		{
			get
			{
				return (bool) this.GetValue(IsSelectedProperty);
			}
			set
			{
				this.SetValue(IsSelectedProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the index of this <see cref="Row"/> in a collection of rows.
		/// </summary>
		/// <value>
		/// The index to set to the row.
		/// </value>
		public int Index
		{
			get
			{
				return (int) this.GetValue(IndexProperty);
			}
			set
			{
				this.SetValue(IndexProperty, value);
			}
		}


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application, but see Remarks for more information.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.GoToFocused();
			this.GoToSelected();
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.GotFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.IsFocused = true;
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (!this.HasFocus())
			{
				this.IsFocused = false;
				base.OnLostFocus(e);
			}
		}

		/// <summary>
		/// When implemented in a derived class, returns class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> implementations for the Silverlight automation infrastructure.
		/// </summary>
		/// <returns>
		/// The class-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer"/> subclass to return.
		/// </returns>
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RowAutomationPeer(this);
		}

		/// <summary>
		/// Creates or identifies the element that is used to display the given item.
		/// </summary>
		/// <returns>
		/// The element that is used to display the given item.
		/// </returns>
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Cell();
		}

		/// <summary>
		/// Determines if the specified item is (or is eligible to be) its own container.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		/// <c>true</c> if the item is (or is eligible to be) its own container; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is Cell;
		}

		/// <summary>
		/// Prepares the specified element to display the specified item.
		/// </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			Cell cell = (Cell) element;
			DataTemplate contentTemplate = cell.ContentTemplate;

			base.PrepareContainerForItemOverride(element, item);

			if (this.ItemTemplate == null)
			{
				cell.ContentTemplate = contentTemplate;
			}

			DataGridFacade.Instance.RegisterController(new CellController(cell));

			cell.ClearValue(DataContextProperty);
			cell.BindValue();
			cell.SetBinding(Cell.IsSelectedProperty, isSelectedBinding);
			cell.BindRowIndex(indexBinding);
		}

		/// <summary>
		/// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);

			Cell cell = (Cell) element;

			cell.ClearValue(Cell.IsReadOnlyProperty);
			cell.ClearValue(Cell.ValueProperty);
			cell.ClearValue(Cell.IsSelectedProperty);
		}

		private static void OnHasFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Row row = (Row) d;
			row.GoToFocused();
			row.OnHasFocusedChanged(e);
		}

		private void GoToFocused()
		{
			VisualStateManager.GoToState(this, this.IsFocused ? "Focused" : "Unfocused", false);
		}

		protected virtual void OnHasFocusedChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.HasFocusChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Row row = (Row) d;
			row.GoToSelected();
			row.OnIsSelectedChanged(e);
		}

		private void GoToSelected()
		{
			VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Deselected", false);
		}

		protected virtual void OnIsSelectedChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.IsSelectedChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
