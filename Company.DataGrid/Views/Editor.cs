using System;
using System.Windows;
using System.Windows.Controls;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Views
{
	/// <summary>
	/// Represents a visual element that provides means of editing a supplied value.
	/// </summary>
	public class Editor : ContentControl
	{
		/// <summary>
		/// Identifies the dependency property which gets or sets the type of the data an <see cref="Editor"/> edits.
		/// </summary>
		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(Type), typeof(Editor), new PropertyMetadata(typeof(object)));

		/// <summary>
		/// Identifies the dependency property which gets or sets the value edited by an <see cref="Editor"/>.
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(Editor), new PropertyMetadata(null, OnValueChanged));

		/// <summary>
		/// Identifies the dependency property which gets or sets the changes in the value that is being edited in an <see cref="Editor"/>.
		/// </summary>
		public static readonly DependencyProperty EditedValueProperty =
			DependencyProperty.Register("EditedValue", typeof(object), typeof(Editor), new PropertyMetadata(null));


		private bool cancelled;


		/// <summary>
		/// Represents a visual element that provides means of editing a supplied value.
		/// </summary>
		public Editor()
		{
			DefaultStyleKey = typeof(Editor);

			DataGridFacade.Instance.RegisterController(new EditorController(this));
		}


		/// <summary>
		/// Gets or sets the type of the data a <see cref="Editor"/> edits.
		/// </summary>
		/// <value>The type of the data a <see cref="Editor"/> edits.</value>
		public Type DataType
		{
			get
			{
				return (Type) GetValue(DataTypeProperty);
			}
			set
			{
				SetValue(DataTypeProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the value edited by an <see cref="Editor"/>.
		/// </summary>
		/// <value>The value edited by an <see cref="Editor"/>.</value>
		public object Value
		{
			get
			{
				return this.GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the changes in the value that is being edited in this <see cref="Editor"/>.
		/// </summary>
		/// <value>The changes in the value that is being edited in this <see cref="Editor"/>.</value>
		public object EditedValue
		{
			get
			{
				return this.GetValue(EditedValueProperty);
			}
			set
			{
				this.SetValue(EditedValueProperty, value);
			}
		}


		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application, but see Remarks for more information.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (VisualStateManager.GoToState(this, "CustomEdit", false))
			{
				return;
			}
			if (this.DataType == typeof(string))
			{
				VisualStateManager.GoToState(this, "EditText", false);
			}
			if (this.DataType.IsNumeric())
			{
				VisualStateManager.GoToState(this, "EditNumber", false);
			}
			if (this.DataType == typeof(DateTime) || this.DataType == typeof(DateTime?))
			{
				VisualStateManager.GoToState(this, "EditDate", false);
			}
		}

		/// <summary>
		/// Called before the <see cref="E:System.Windows.UIElement.LostFocus"/> event occurs.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (!this.cancelled)
			{
				this.Save();
			}
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Editor) d).OnValueChanged(e.OldValue, e.NewValue);
		}

		/// <summary>
		/// Called when the value of this <see cref="Editor"/> is changed.
		/// </summary>
		/// <param name="oldValue">The old value of the editor.</param>
		/// <param name="newValue">The new value of the editor.</param>
		protected virtual void OnValueChanged(object oldValue, object newValue)
		{
			this.EditedValue = this.Value;
		}

		/// <summary>
		/// Saves the changes to the value of this <see cref="Editor"/>.
		/// </summary>
		public void Save()
		{
			object oldValue = this.Value;
			this.Value = this.EditedValue;
			if (Validation.GetHasError(this))
			{
				this.Value = oldValue;
			}
		}

		/// <summary>
		/// Cancels the changes to the value of this <see cref="Editor"/>.
		/// </summary>
		public void Cancel()
		{
			this.cancelled = true;
		}
	}
}
