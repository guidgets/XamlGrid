using System;
using System.Windows;
using System.Windows.Controls;
using XamlGrid.Controllers;

namespace XamlGrid.Views
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

		/// <summary>
		/// Identifies the dependency property which gets or sets a value indicating whether a <see cref="Editor"/> has focus.
		/// </summary>
		public static readonly DependencyProperty HasFocusProperty =
			DependencyProperty.Register("HasFocus", typeof(bool), typeof(Editor), new PropertyMetadata(OnHasFocusChanged));


		private bool cancelled;


		/// <summary>
		/// Represents a visual element that provides means of editing a supplied value.
		/// </summary>
		public Editor()
		{
			DefaultStyleKey = typeof(Editor);

			DataGridFacade.Instance.RegisterController(new EditorController(this));

			this.LayoutUpdated += this.Editor_LayoutUpdated;
		}


		/// <summary>
		/// Gets or sets the type of the data a <see cref="Editor"/> edits.
		/// </summary>
		/// <value>The type of the data a <see cref="Editor"/> edits.</value>
		public virtual Type DataType
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
		public virtual object Value
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
		public virtual object EditedValue
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
		/// Gets or sets a value indicating whether this <see cref="Editor"/> has focus.
		/// </summary>
		/// <value><c>true</c> if this <see cref="Editor"/> has focus; otherwise, <c>false</c>.</value>
		public bool HasFocus
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

		private static void OnHasFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Editor) d).OnHasFocusChanged(e);
		}

		/// <summary>
		/// Called when the property indicating if the <see cref="Cell"/> has focus changes.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows"/> instance containing the event data.</param>
		protected virtual void OnHasFocusChanged(DependencyPropertyChangedEventArgs e)
		{
			if ((bool) e.NewValue)
			{
				this.Focus();
			}
		}

		/// <summary>
		/// Saves the changes to the value of this <see cref="Editor"/>.
		/// </summary>
		public virtual void Save()
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
		public virtual void Cancel()
		{
			this.cancelled = true;
		}


		private void Editor_LayoutUpdated(object sender, EventArgs e)
		{
			this.LayoutUpdated -= this.Editor_LayoutUpdated;
			if (this.HasFocus)
			{
				this.Focus();
			}
		}
	}
}
