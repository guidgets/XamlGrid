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
// File:	Cell.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Input;
using Guidgets.XamlGrid.Automation;
using Guidgets.XamlGrid.Models;

namespace Guidgets.XamlGrid.Views
{
	/// <summary>
	/// Represents an element that displays and manipulates a piece of a data object.
	/// </summary>
	public class Cell : CellBase
	{
		/// <summary>
		/// Occurs when the edit mode of this <see cref="Cell"/> is changed.
		/// </summary>
		public virtual event DependencyPropertyChangedEventHandler IsInEditModeChanged;


		private static readonly Type typeOfObject = typeof(object);
		private static readonly Type typeOfBoolean = typeof(bool);
		private static readonly Type typeOfNullableBoolean = typeof(bool?);
		private static readonly Type typeOfByteArray = typeof(byte[]);
		private static readonly Type typeOfUri = typeof(Uri);

		/// <summary>
		/// Identifies the dependency property which gets or sets the value contained in a <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(Cell), new PropertyMetadata(OnValueChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Cell"/> has focus.
		/// </summary>
		public static readonly DependencyProperty HasFocusProperty =
			DependencyProperty.Register("HasFocus", typeof(bool), typeof(Cell), new PropertyMetadata(OnHasFocusChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether the content of a <see cref="Cell"/> is read-only.
		/// </summary>
		public static readonly DependencyProperty IsReadOnlyProperty =
			DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(Cell), new PropertyMetadata(false, OnIsReadOnlyChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Cell"/> is in edit mode.
		/// </summary>
		public static readonly DependencyProperty IsInEditModeProperty =
			DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(Cell), new PropertyMetadata(OnIsInEditModeChanged));

		public static readonly DependencyProperty AlwaysInEditModeProperty =
			DependencyProperty.Register("AlwaysInEditMode", typeof(bool), typeof(Cell), new PropertyMetadata(false, OnAlwaysInEditModeChanged));


		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Cell"/> is selected.
		/// </summary>
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(Cell),
			                            new PropertyMetadata(false, OnIsSelectedChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the type of the data a <see cref="Cell"/> represents.
		/// </summary>
		private static readonly DependencyProperty dataTypeProperty =
			DependencyProperty.Register("DataType", typeof(Type), typeof(Cell), new PropertyMetadata(typeOfObject));

		private static readonly Binding dataTypeBinding = new Binding("Column.DataType")
		                                                  	{
		                                                  		RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                                  	};

		private static readonly Binding isReadOnlyBinding = new Binding("Column.IsReadOnly")
		                                                    {
		                                                    	RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                                    };

		private static readonly Binding styleBinding = new Binding("Column.CellStyle")
		                                               	{
		                                               		RelativeSource = new RelativeSource(RelativeSourceMode.Self)
														};

		private static readonly Binding valueBinding = new Binding("Value")
		                                               	{
		                                               		RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                               	};


		/// <summary>
		/// Represents an element that displays and manipulates a piece of a data object.
		/// </summary>
		public Cell()
		{
			this.DefaultStyleKey = typeof(Cell);

			this.SetBinding(dataTypeProperty, dataTypeBinding);
			this.SetBinding(IsReadOnlyProperty, isReadOnlyBinding);
			this.SetBinding(StyleProperty, styleBinding);
		}


		/// <summary>
		/// Gets or sets the value contained in the <see cref="Cell"/>.
		/// </summary>
		/// <value>The value contained in the <see cref="Cell"/>.</value>
		public virtual object Value
		{
			get { return this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> has focus.
		/// </summary>
		/// <value><c>true</c> if this <see cref="Cell"/> has focus; otherwise, <c>false</c>.</value>
		public bool HasFocus
		{
			get { return (bool) this.GetValue(HasFocusProperty); }
			set { this.SetValue(HasFocusProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the content of the <see cref="Cell"/> is read-only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the content of the <see cref="Cell"/> is read-only; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsReadOnly
		{
			get { return (bool) this.GetValue(IsReadOnlyProperty); }
			set { this.SetValue(IsReadOnlyProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is in edit mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Cell"/> is in edit mode; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsInEditMode
		{
			get { return (bool) this.GetValue(IsInEditModeProperty); }
			set { this.SetValue(IsInEditModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is always in edit mode.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Cell"/> is always in edit mode; otherwise, <c>false</c>.
		/// </value>
		public bool AlwaysInEditMode
		{
			get { return (bool) GetValue(AlwaysInEditModeProperty); }
			set { SetValue(AlwaysInEditModeProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is selected.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Cell"/> is selected; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsSelected
		{
			get { return (bool) this.GetValue(IsSelectedProperty); }
			set { this.SetValue(IsSelectedProperty, value); }
		}

		/// <summary>
		/// Gets or sets the type of the data this <see cref="Cell"/> represents.
		/// </summary>
		/// <value>The type of the data this <see cref="Cell"/> represent.</value>
		public virtual Type DataType
		{
			get { return (Type) this.GetValue(dataTypeProperty); }
		}


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.GoToSpecialView();
			if (this.IsSelected)
			{
				this.GoToSelected();
			}
			if (this.IsInEditMode)
			{
				this.GoToEdit();
			}
		}

		/// <summary>
		/// Called before the <see cref="UIElement.MouseLeftButtonDown"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			this.Focus();
			e.Handled = true;
		}

		/// <summary>
		/// Called before the <see cref="UIElement.GotFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.HasFocus = true;
			base.OnGotFocus(e);
			VisualStateManager.GoToState(this, "Focused", false);
		}

		/// <summary>
		/// Called before the <see cref="UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (!this.HasFocus())
			{
				this.HasFocus = false;
				base.OnLostFocus(e);
				VisualStateManager.GoToState(this, "Unfocused", false);
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
			return new CellAutomationPeer(this);
		}

		/// <summary>
		/// Determines whether the <see cref="Cell"/> is automatically sized according to its contents.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the <see cref="Cell"/> is automatically sized according to its contents; otherwise, <c>false</c>.
		/// </returns>
		protected override bool IsAutoSized()
		{
			return this.Column.Width.SizeMode == SizeMode.ToData || base.IsAutoSized();
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).OnValueChanged(e);
		}

		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			this.UpdateDataType();
		}

		public virtual void BindValue()
		{
			this.SetBinding(ValueProperty, this.Column.Binding);
			this.SetBinding(ContentProperty, valueBinding);
			this.UpdateDataType();
		}

		private static void OnHasFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).OnHasFocusChanged(e);
		}

		/// <summary>
		/// Called when the property indicating if the <see cref="Cell"/> has focus changes.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnHasFocusChanged(DependencyPropertyChangedEventArgs e)
		{
			if ((bool) e.NewValue)
			{
				this.Focus();
			}
		}

		protected virtual void OnIsInEditModeChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.IsReadOnly && this.IsInEditMode)
			{
				this.IsInEditMode = false;
				return;
			}
			if (this.AlwaysInEditMode)
			{
				if (!this.IsInEditMode)
				{
					this.IsInEditMode = true;
				}
				return;
			}
			DependencyPropertyChangedEventHandler handler = this.IsInEditModeChanged;
			if (handler != null)
			{
				handler(this, e);
			}
			if (this.IsInEditMode)
			{
				this.GoToEdit();
			}
			else
			{
				this.GoToView();
			}
		}

		protected virtual void OnAlwaysInEditModeChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.IsReadOnly && this.AlwaysInEditMode)
			{
				this.AlwaysInEditMode = false;
				return;
			}
			if (this.AlwaysInEditMode && !this.IsInEditMode)
			{
				this.IsInEditMode = true;
			}
		}


		private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).OnIsReadOnlyChanged(e);
		}

		protected virtual void OnIsReadOnlyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.IsReadOnly)
			{
				if (this.IsInEditMode)
				{
					this.IsInEditMode = false;
				}
				if (this.AlwaysInEditMode)
				{
					this.AlwaysInEditMode = false;
				}
			}
		}

		private static void OnIsInEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).OnIsInEditModeChanged(e);
		}

		private static void OnAlwaysInEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).OnAlwaysInEditModeChanged(e);
		}

		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Cell) d).GoToSelected();
		}

		/// <summary>
		/// Sets the <see cref="Cell"/> in edit mode.
		/// </summary>
		private void GoToEdit()
		{
			if (!this.GoToSpecialView())
			{
				VisualStateManager.GoToState(this, "Editor", false);
			}
		}

		/// <summary>
		/// Exits the edit mode of the <see cref="Cell"/>.
		/// </summary>
		private void GoToView()
		{
			if (!this.GoToSpecialView())
			{
				VisualStateManager.GoToState(this, "View", false);
			}
		}

		private void GoToSelected()
		{
			VisualStateManager.GoToState(this, this.IsSelected ? "Selected" : "Deselected", false);
		}

		private bool GoToSpecialView()
		{
			return this.GoToBoolean() || this.GoToImage();
		}

		private bool GoToBoolean()
		{
			if (this.DataType == typeOfBoolean || this.DataType == typeOfNullableBoolean)
			{
				return VisualStateManager.GoToState(this, "Boolean", false);
			}
			return false;
		}

		private bool GoToImage()
		{
			if (this.DataType == typeOfByteArray)
			{
				return VisualStateManager.GoToState(this, "Image", false);
			}
			if (this.DataType == typeOfUri && this.Value != null)
			{
				string uri = this.Value.ToString();
				if ((from imageExtension in new[] { ".png", ".jpg", ".jpeg" }
				     where string.Compare(Path.GetExtension(uri), imageExtension, StringComparison.OrdinalIgnoreCase) == 0
				     select imageExtension).Any())
				{
					return VisualStateManager.GoToState(this, "Image", false);
				}
			}
			return false;
		}

		private void UpdateDataType()
		{
			if (this.Column != null && this.Column.DataType == typeOfObject)
			{
				using (PropertyPathWalker propertyPathWalker = new PropertyPathWalker(this.Column.Binding.Path.Path, true))
				{
					propertyPathWalker.Update(this.DataContext);
					if (!propertyPathWalker.IsPathBroken)
					{
						this.Column.DataType = propertyPathWalker.FinalNode.ValueType;
					}
				}
			}
		}
	}
}
