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
	public class Cell : ContentControl
	{
		private enum IsInEditModeSource
		{
			API = 0,
			CommitEdit = 1,
			CancelEdit = 2
		}

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

		private Type dataType;
		private IsInEditModeSource isInEditModeSource;
		private bool isFocused;

		/// <summary>
		/// Represents an element that displays and manipulates a piece of a data object.
		/// </summary>
		public Cell()
		{
			this.DefaultStyleKey = typeof(Cell);
			this.isInEditModeSource = IsInEditModeSource.API;
			this.dataType = typeof(object);
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
		/// Gets or sets the type of the data this <see cref="Cell"/> represents.
		/// </summary>
		/// <value>The type of the data this <see cref="Cell"/> represent.</value>
		public Type DataType
		{
			get
			{
				if (this.dataType == typeof(object) && this.Value != null)
				{
					this.dataType = this.Value.GetType();
				}
				return this.dataType;
			}
			set
			{
				this.dataType = value;
			}
		}

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (!this.GoToBoolean())
			{
				this.GoToViewState();
			}
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			e.Handled = true;
		}

		/// <summary>
		/// Called before the <see cref="UIElement.MouseLeftButtonUp"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			if (this.isFocused)
			{
				this.IsInEditMode = true;				
			}
			else
			{
				this.isFocused = this.Focus();
			}
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
					this.isInEditModeSource = IsInEditModeSource.CommitEdit;
					this.IsInEditMode = false;
					break;
				case Key.Escape:
					this.isInEditModeSource = IsInEditModeSource.CancelEdit;
					this.IsInEditMode = false;
					break;
			}
		}

		/// <summary>
		/// Called before the <see cref="UIElement.GotFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			this.isFocused = true;
		}

		/// <summary>
		/// Called before the <see cref="UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (!this.IsFocusWithin())
			{
				this.IsInEditMode = false;
				this.isFocused = false;
			}
		}

		private void CompleteEditing()
		{
			this.GoToViewState();
			switch (this.isInEditModeSource)
			{
				case IsInEditModeSource.API:
				case IsInEditModeSource.CommitEdit:
					this.CommitEdit();
					break;
				case IsInEditModeSource.CancelEdit:
					this.CancelEdit();
					break;
			}
			this.isInEditModeSource = IsInEditModeSource.API;
		}

		/// <summary>
		/// Sets the <see cref="Cell"/> in edit mode.
		/// </summary>
		protected virtual bool GoToEditState()
		{
			if (VisualStateManager.GoToState(this, "CustomEditor", false))
			{
				this.HandleEditorLoading(true);
				return true;
			}
			bool result = this.GoToEditorForType();
			if (result)
			{
				this.HandleEditorLoading(true);
			}
			return result;
		}

		/// <summary>
		/// Exits the edit mode of the <see cref="Cell"/>.
		/// </summary>
		protected virtual bool GoToViewState()
		{
			this.HandleEditorLoading(false);
			if (this.GoToBoolean())
			{
				return true;
			}
			return VisualStateManager.GoToState(this, "View", false);
		}

		private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			if (cell.Value != null)
			{
				Type valueType = cell.Value.GetType();
				if (!cell.DataType.IsAssignableFrom(valueType))
				{
					cell.DataType = valueType;
				}
			}
			cell.EditorValue = cell.Value;
			if (cell.Template != null)
			{
				cell.GoToBoolean();
			}
		}

		private static void OnIsInEditModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			if ((bool) e.NewValue)
			{
				cell.GoToEditState();
			}
			else
			{
				cell.CompleteEditing();
			}
		}

		private bool GoToBoolean()
		{
			if (this.DataType == typeof(bool))
			{
				return VisualStateManager.GoToState(this, "Boolean", false);
			}
			if (this.DataType == typeof(bool?))
			{
				return VisualStateManager.GoToState(this, "NullableBoolean", false);
			}
			return false;
		}

		private bool GoToEditorForType()
		{
			if (this.DataType == typeof(string))
			{
				return VisualStateManager.GoToState(this, "EditText", false);
			}
			if (this.DataType.IsNumeric())
			{
				return VisualStateManager.GoToState(this, "EditNumber", false);
			}
			if (this.DataType == typeof(DateTime) || this.DataType == typeof(DateTime?))
			{
				return VisualStateManager.GoToState(this, "EditDate", false);
			}
			return false;
		}

		private void HandleEditorLoading(bool addHandler)
		{
			if (this.Content is Control)
			{
				Control editor = (Control) this.Content;
				RoutedEventHandler loadedHandler = (sender, e) =>
				                                   	{
				                                   		editor.Focus();
				                                   		object focusedElement = FocusManager.GetFocusedElement();
				                                   		if (focusedElement is TextBox)
				                                   		{
				                                   			((TextBox) focusedElement).SelectAll();
				                                   		}
				                                   	};
				if (addHandler)
				{
					editor.Loaded += loadedHandler;
				}
				else
				{
					editor.Loaded -= loadedHandler;
				}
			}
		}

		private void CommitEdit()
		{
			object oldValue = this.Value;
			this.Value = this.EditorValue;
			if (Validation.GetHasError(this))
			{
				this.Value = oldValue;
			}
			this.ClearEditorValue();
		}

		private void CancelEdit()
		{
			this.ClearEditorValue();
		}

		private void ClearEditorValue()
		{
			this.ClearValue(EditorValueProperty);
			this.EditorValue = this.Value;
		}
	}
}