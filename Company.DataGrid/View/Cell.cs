using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Company.DataGrid.View
{
	/// <summary>
	/// Represents an element that displays and manipulates a piece of a data object.
	/// </summary>
	public class Cell : Control
	{
		/// <summary>
		/// Identifies the property which gets or sets the value contained in a <see cref="Cell"/>.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(Cell), new PropertyMetadata(null));

		/// <summary>
		/// Represents an element that displays and manipulates a piece of a data object.
		/// </summary>
		public Cell()
		{
			this.DefaultStyleKey = typeof(Cell);
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
		/// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			this.EnterEditMode();
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.KeyUp"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			switch (e.Key)
			{
				case Key.Enter:
					VisualStateManager.GoToState(this, "NoEdit", false);
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
			VisualStateManager.GoToState(this, "NoEdit", false);
		}

		/// <summary>
		/// Sets the <see cref="Cell"/> in edit mode.
		/// </summary>
		protected virtual bool EnterEditMode()
		{
			if (this.Value == null)
			{
				return VisualStateManager.GoToState(this, "EditText", false);
			}
			Type valueType = this.Value.GetType();
			if (valueType == typeof(bool) || valueType == typeof(bool?))
			{
				return VisualStateManager.GoToState(this, "EditBoolean", false);
			}
			// TODO: when there is a numeric up-down add support for numeric types
			// TODO: when the date picker works properly, add support for DateTime
			// TODO: do not always go to editing text; explicitly enumerate all types that can be edited with a text box
			return VisualStateManager.GoToState(this, "EditText", false);
		}
	}
}