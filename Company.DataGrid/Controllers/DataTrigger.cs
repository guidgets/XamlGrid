using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Company.Widgets.Views;

namespace Company.Widgets.Controllers
{
	public class DataTrigger : TriggerBase<FrameworkElement>
	{
		private readonly BindingListener listener;
		private bool templateApplied;

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(DataTrigger), new PropertyMetadata(null, HandleValueChanged));
		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(Binding), typeof(DataTrigger), new PropertyMetadata(null, HandleBindingChanged));

		private static readonly DependencyProperty templatedParentProperty =
			DependencyProperty.RegisterAttached("TemplatedParent", typeof(Control), typeof(DataTrigger), null);

		public DataTrigger()
		{
			this.listener = new BindingListener(this.HandleBindingValueChanged);
		}

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

		public Binding DataBinding
		{
			get
			{
				return (Binding) this.GetValue(BindingProperty);
			}
			set
			{
				this.SetValue(BindingProperty, value);
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			Binding binding = new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) };
			this.AssociatedObject.SetBinding(templatedParentProperty, binding);
			this.listener.Element = (Control) this.AssociatedObject.GetValue(templatedParentProperty);
			((ITemplateNotify) this.listener.Element).TemplateApplied += this.Target_TemplateApplied;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			((ITemplateNotify) this.listener.Element).TemplateApplied -= this.Target_TemplateApplied;
			this.listener.Element = null;
		}

		private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e)
		{
			this.CheckState();
		}

		private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((DataTrigger) sender).OnBindingChanged(e);
		}

		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e)
		{
			this.DataBinding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
			this.listener.Binding = this.DataBinding;
		}


		private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((DataTrigger) sender).OnValueChanged(e);
		}

		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			this.CheckState();
		}

		protected virtual void OnTargetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.listener.Element = (FrameworkElement) e.NewValue;
		}

		private void CheckState()
		{
			if (this.Value == null || this.listener.Value == null)
			{
				this.IsTrue = Equals(this.listener.Value, this.Value);
			}
			else
			{
				this.IsTrue = Equals(this.listener.Value.ToString(), this.Value);
			}
		}

		private bool isTrue;
		private bool IsTrue
		{
			get { return this.isTrue; }
			set
			{
				if (this.isTrue != value)
				{
					this.isTrue = value;
					if (this.isTrue && this.templateApplied)
					{
						this.InvokeActions(this.listener.Element);
					}
				}
			}
		}

		private void Target_TemplateApplied(object sender, EventArgs e)
		{
			if (this.IsTrue)
			{
				this.InvokeActions(this.listener.Element);				
			}
			this.templateApplied = true;
		}
	}
}
