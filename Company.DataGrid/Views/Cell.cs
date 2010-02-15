using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Company.DataGrid.Controllers;
using Company.DataGrid.Models;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents an element that displays and manipulates a piece of a data object.
	/// </summary>
	public class Cell : CellBase
	{
		/// <summary>
		/// Identifies the property which gets or sets the value contained in a <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(Cell),
										new PropertyMetadata(OnValueChanged));

		/// <summary>
		/// Identifies the property which gets or sets the type of the data this <see cref="Cell"/> represents..
		/// </summary>
		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(Type), typeof(Cell), new PropertyMetadata(typeof(object)));

		/// <summary>
		/// Identifies the property which gets or sets value contained in the editor of the <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty EditorValueProperty =
			DependencyProperty.Register("EditorValue", typeof(object), typeof(Cell), new PropertyMetadata(null));

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether the content of the <see cref="Cell"/> is editable
		/// </summary>
		public static readonly DependencyProperty IsEditableProperty =
			DependencyProperty.Register("IsEditable", typeof(bool), typeof(Cell), new PropertyMetadata(true));		

		/// <summary>
		/// Identifies the property which gets or sets a value indicating whether this <see cref="Cell"/> is in edit mode.
		/// </summary>
		public static readonly DependencyProperty IsInEditModeProperty =
			DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(Cell),
			                            new PropertyMetadata(OnIsInEditModeChanged));


		private bool isFocused;
		private bool cancelled;


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
		/// Gets or sets a value indicating whether the content of the <see cref="Cell"/> is editable.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the content of the <see cref="Cell"/> is editable; otherwise, <c>false</c>.
		/// </value>
		public bool IsEditable
		{
			get
			{
				return (bool) this.GetValue(IsEditableProperty);
			}
			set
			{
				this.SetValue(IsEditableProperty, value);
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
				return (Type) this.GetValue(DataTypeProperty);
			}
			set
			{
				this.SetValue(DataTypeProperty, value);
			}
		}


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.GoToSpecialView();
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
					this.IsInEditMode = false;
					break;
				case Key.Escape:
					this.IsInEditMode = false;
					this.ClearValue(EditorValueProperty);
					this.EditorValue = this.Value;
					this.cancelled = true;
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
				if (this.cancelled)
				{
					this.cancelled = false;
				}
				else
				{
					this.CommitEdit();
				}
				this.isFocused = false;
			}
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

		/// <summary>
		/// Sets the <see cref="Cell"/> in edit mode.
		/// </summary>
		protected virtual bool GoToEditState()
		{
			return VisualStateManager.GoToState(this, "CustomEditor", false) || this.GoToEditorForType();
		}

		/// <summary>
		/// Exits the edit mode of the <see cref="Cell"/>.
		/// </summary>
		protected virtual bool GoToViewState()
		{
			return this.GoToSpecialView() || VisualStateManager.GoToState(this, "View", false);
		}

		private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			if (cell.Value != null)
			{
				cell.DataType = cell.Value.GetType();
			}
			cell.EditorValue = cell.Value;
			if (!cell.IsInEditMode)
			{
				cell.Content = cell.Value;
				cell.GoToSpecialView();
			}
		}

		private static void OnIsInEditModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Cell cell = (Cell) dependencyObject;
			bool editMode = (bool) e.NewValue;
			if (!cell.IsEditable && editMode)
			{
				cell.IsInEditMode = false;
				return;
			}
			if (editMode)
			{
				cell.GoToEditState();
			}
			else
			{
				cell.GoToViewState();
			}
		}

		private bool GoToSpecialView()
		{
			return this.GoToBoolean() || this.GoToImage();
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

		private bool GoToImage()
		{
			if (this.DataType == typeof(byte[]))
			{
				return VisualStateManager.GoToState(this, "Image", false);
			}
			if (this.DataType == typeof(Uri) && this.Value != null)
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

		private void CommitEdit()
		{
			object oldValue = this.Value;
			this.Value = this.EditorValue;
			if (Validation.GetHasError(this))
			{
				this.Value = oldValue;
			}
		}
	}
}