using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Synchronizes the offsets of a <see cref="ScrollViewer"/> with the <see cref="ScrollExtensions"/> class which enables the proper binding to the offsets.
	/// </summary>
	public class SyncScrollBehavior : Behavior<ScrollViewer>
	{
		private static readonly DependencyProperty verticalOffsetListenerProperty =
			DependencyProperty.Register("verticalOffsetListener", typeof(double), typeof(ScrollViewer),
										new PropertyMetadata(OnVerticalOffsetChanged));

		private static readonly DependencyProperty horizontalOffsetListenerProperty =
			DependencyProperty.Register("horizontalOffsetListener", typeof(double), typeof(ScrollViewer),
										new PropertyMetadata(OnHorizontalOffsetChanged));

		private static readonly Binding verticalOffsetBinding = new Binding("VerticalOffset")
		                                                        {
		                                                        	RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                                        };

		private static readonly Binding horizontalOffsetBinding = new Binding("HorizontalOffset")
		                                                          {
		                                                          	  RelativeSource = new RelativeSource(RelativeSourceMode.Self)
		                                                          };



		/// <summary>
		/// Called after the behaviour is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.SetBinding(verticalOffsetListenerProperty, verticalOffsetBinding);
			this.AssociatedObject.SetBinding(horizontalOffsetListenerProperty, horizontalOffsetBinding);
		}

		/// <summary>
		/// Called when the behaviour is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			this.ClearValue(verticalOffsetListenerProperty);
			this.ClearValue(horizontalOffsetListenerProperty);

			base.OnDetaching();
		}


		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollExtensions.SetVerticalOffset((ScrollViewer) d, (double) e.NewValue);
		}

		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollExtensions.SetHorizontalOffset((ScrollViewer) d, (double) e.NewValue);
		}
	}
}
