using System;
using System.Windows;

namespace Company.Widgets.Controllers
{
	public class DataTrigger : Trigger<FrameworkElement>
	{
		public object BindingValue
		{
			get { return this.GetValue(BindingValueProperty); }
			set { SetValue(BindingValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BindingValue.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BindingValueProperty =
			DependencyProperty.Register("BindingValue", typeof(object), typeof(DataTrigger), new PropertyMetadata(HandleBindingValueChanged));

		
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(DataTrigger), new PropertyMetadata(null, HandleValueChanged));

		public string Value
		{
			get
			{
				return (string) this.GetValue(ValueProperty);
			}
			set
			{
				this.SetValue(ValueProperty, value);
			}
		}

		protected override void OnAttach()
		{
			base.OnAttach();

			//this.AssociatedObject.LayoutUpdated += this.AssociatedObject_LayoutUpdated;
		}

		void AssociatedObject_LayoutUpdated(object sender, System.EventArgs e)
		{
			this.AssociatedObject.LayoutUpdated -= this.AssociatedObject_LayoutUpdated;
			this.templateApplied = true;
			this.CheckState();
		}

		protected override void OnDetach()
		{
			this.ClearValue(BindingValueProperty);

			base.OnDetach();
		}

		private static void HandleBindingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataTrigger) d).CheckState();
		}


		private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((DataTrigger) sender).OnValueChanged(e);
		}

		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			this.CheckState();
		}

		private void CheckState()
		{
			if (!this.templateApplied)
			{
				return;
			}
			if (this.Value == null || this.BindingValue == null)
			{
				this.IsTrue = Equals(this.BindingValue, this.Value);
			}
			else
			{
				this.IsTrue = Equals(this.BindingValue.ToString(), this.Value);
			}
		}

		private bool isTrue;
		private bool templateApplied;

		private bool IsTrue
		{
			set
			{
				if (this.isTrue != value)
				{
					this.isTrue = value;
					if (this.isTrue)
					{
						this.InvokeActions(EventArgs.Empty);
					}
				}
			}
		}
	}
}
