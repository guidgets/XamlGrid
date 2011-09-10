using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Company.Widgets.Views
{
	public abstract class RowBase : ItemsControl
	{
		public event DependencyPropertyChangedEventHandler VisibilityChanged;

		private static readonly DependencyProperty visibilityListenerProperty =
			DependencyProperty.Register("visibilityListener", typeof(Visibility), typeof(RowBase),
										new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

		private static readonly Binding visibilityBinding = new Binding("Visibility")
		                                                    {
		                                                    	RelativeSource = new RelativeSource(RelativeSourceMode.Self),
		                                                    	Mode = BindingMode.OneWay
		                                                    };


		protected RowBase()
		{
			this.SetBinding(visibilityListenerProperty, visibilityBinding);
		}


		/// <summary>
		/// Undoes the effects of the <see cref="ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> method.
		/// </summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DataGridFacade.Instance.RemoveController(element.GetHashCode().ToString());
		}

		private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RowBase) d).OnVisibilityChanged(e);
		}

		protected virtual void OnVisibilityChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler handler = this.VisibilityChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
