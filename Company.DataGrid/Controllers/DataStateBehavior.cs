using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Collections;

namespace Company.Widgets.Controllers
{
	public class DataStateBehavior : Behavior<FrameworkElement>
	{


		public Control Target
		{
			get { return (Control) GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Control), typeof(DataStateBehavior), new PropertyMetadata(OnTargetChanged));

		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataStateBehavior) d).OnTargetChanged(e);
		}

		public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(Binding), typeof(DataStateBehavior), new PropertyMetadata(null, HandleBindingChanged));
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(DataStateBehavior), new PropertyMetadata(null, HandleValueChanged));
		public static readonly DependencyProperty TrueStateProperty = DependencyProperty.Register("TrueState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata(null));
		public static readonly DependencyProperty FalseStateProperty = DependencyProperty.Register("FalseState", typeof(string), typeof(DataStateBehavior), new PropertyMetadata(null));

		private BindingListener listener;

		public DataStateBehavior()
		{
			this.listener = new BindingListener(this.HandleBindingValueChanged);
		}

		public Binding DataBinding
		{
			get { return (Binding) this.GetValue(BindingProperty); }
			set { this.SetValue(BindingProperty, value); }
		}

		public string Value
		{
			get { return (string) this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		public string FalseState
		{
			get { return (string) this.GetValue(FalseStateProperty); }
			set { this.SetValue(FalseStateProperty, value); }
		}

		public string TrueState
		{
			get { return (string) this.GetValue(TrueStateProperty); }
			set { this.SetValue(TrueStateProperty, value); }
		}

		private static void HandleBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((DataStateBehavior) sender).OnBindingChanged(e);
		}

		protected virtual void OnBindingChanged(DependencyPropertyChangedEventArgs e)
		{
			this.listener.Binding = (Binding) e.NewValue;
		}

		private static void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((DataStateBehavior) sender).OnValueChanged(e);
		}

		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			this.CheckState();
		}

		protected virtual void OnTargetChanged(DependencyPropertyChangedEventArgs e)
		{
			this.listener.Element = (FrameworkElement) e.NewValue;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.listener.Element = null;
		}

		private void HandleBindingValueChanged(object sender, BindingChangedEventArgs e)
		{
			this.CheckState();
		}

		private void CheckState()
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.Value == null || this.listener.Value == null)
			{
				this.IsTrue = Equals(this.listener.Value, this.Value);
			}
			else
			{
				object convertedValue = ConverterHelper.ConvertToType(this.Value, this.listener.Value.GetType());
				this.IsTrue = object.Equals(this.listener.Value, ConverterHelper.ConvertToType(this.Value, this.listener.Value.GetType()));
			}
		}

		private bool? isTrue;
		private bool? IsTrue
		{
			get { return this.isTrue; }
			set
			{
				if (this.isTrue != value)
				{
					this.isTrue = value;

					if (this.IsTrue.HasValue)
					{
						if (this.IsTrue.Value)
							VisualStateManager.GoToState(this.Target, this.TrueState, true);
						else
							VisualStateManager.GoToState(this.Target, this.FalseState, true);
					}
				}
			}
		}

		private static Control FindTargetControl(FrameworkElement element)
		{
			FrameworkElement parent = element;
			bool foundVSM = false;

			while (parent != null)
			{
				if (!foundVSM)
				{
					IList vsgs = VisualStateManager.GetVisualStateGroups(parent);
					if (vsgs != null && vsgs.Count > 0)
						foundVSM = true;
				}

				if (foundVSM)
				{
					Control parentControl = parent as Control;
					if (parentControl != null)
						return parentControl;
				}
				parent = parent.Parent as FrameworkElement;
			}

			return null;
		}
	}
}
