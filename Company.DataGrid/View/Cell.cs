using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Represents an element that displays and manipulates a piece of a data object.
	/// </summary>
	public class Cell : Control
	{
		/// <summary>
		/// Identifies the property which gets or sets value contained in the editor of the <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty EditorValueProperty =
			DependencyProperty.Register("EditorValue", typeof(object), typeof(Cell), new PropertyMetadata(null));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether this <see cref="Cell"/> is in edit mode.
		/// </summary>
		public static readonly DependencyProperty IsInEditModeProperty =
			DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(Cell),
			                            new PropertyMetadata(OnIsInEditModeChanged));

		/// <summary>
		/// Identifies the property which gets or sets the value contained in a <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(Cell),
			                            new PropertyMetadata(OnValueChanged));

		/// <summary>
		/// Represents an element that displays and manipulates a piece of a data object.
		/// </summary>
		public Cell()
		{
			this.DefaultStyleKey = typeof(Cell);
		}

		/// <summary>
		/// Gets or sets value contained in the editor of the <see cref="Cell"/>.
		/// </summary>
		/// <value>The value contained in the editor of the <see cref="Cell"/>.</value>
		public object EditorValue
		{
			get
			{
				return this.GetValue(EditorValueProperty);
			}
			set
			{
				this.SetValue(EditorValueProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value contained in the <see cref="Cell"/>.
		/// </summary>
		/// <value>The value contained in the <see cref="Cell"/>.</value>
		public object Value
		{
			get
			{
				return this.GetValue(ValueProperty);
			}
			set
			{
				this.SetValue(ValueProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is in edit mode.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this <see cref="Cell"/> is in edit mode; otherwise, <c>false</c>.
		/// </value>
		public bool IsInEditMode
		{
			get
			{
				return (bool) this.GetValue(IsInEditModeProperty);
			}
			set
			{
				this.SetValue(IsInEditModeProperty, value);
			}
		}

		/// <summary>
		/// Called before the <see cref="UIElement.MouseLeftButtonUp"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			this.IsInEditMode = true;
		}

		/// <summary>
		/// Called before the <see cref="UIElement.KeyUp"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			switch (e.Key)
			{
				case Key.Enter:
					this.CommitEdit();
					break;
				case Key.Escape:
					this.CancelEdit();
					break;
			}
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (!this.IsFocusWithin())
			{
				this.CommitEdit();
			}
		}

		/// <summary>
		/// Sets the <see cref="Cell"/> in edit mode.
		/// </summary>
		protected virtual bool EnterEditMode()
		{
			this.Focus();
			if (VisualStateManager.GoToState(this, "CustomEditor", false))
			{
				return true;
			}
			// TODO: find a way to check the type and go to the proper state regardless of the null value
			if (this.Value == null)
			{
				return VisualStateManager.GoToState(this, "EditText", false);
			}
			Type valueType = this.Value.GetType();
			// TODO: the cell should represent a check box if the value is a (nullable) boolean
			// TODO: when the date picker works properly, add support for DateTime
			// TODO: when there is a numeric up-down add support for numeric types
			if (valueType == typeof(string) || TypeController.IsNumeric(this.Value) ||
			    valueType == typeof(bool) || valueType == typeof(bool?) ||
			    valueType == typeof(DateTime) || valueType == typeof(DateTime?))
			{
				return VisualStateManager.GoToState(this, "EditText", false);
			}
			return false;
		}

		/// <summary>
		/// Exits the edit mode of the <see cref="Cell"/>.
		/// </summary>
		protected virtual bool ExitEditMode()
		{
			return VisualStateManager.GoToState(this, "NoEdit", false);
		}

		private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			cell.EditorValue = cell.Value;
		}

		private static void OnIsInEditModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			if ((bool) e.NewValue)
			{
				cell.EnterEditMode();
			}
			else
			{
				cell.ExitEditMode();
			}
		}

		private void CommitEdit()
		{
			this.Value = this.EditorValue;
			this.IsInEditMode = false;
		}

		private void CancelEdit()
		{
			this.EditorValue = this.Value;
			this.IsInEditMode = false;
		}
	}
}