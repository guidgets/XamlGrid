using System.Windows;
using System.Windows.Controls;

namespace Company.Widgets.Controllers
{
	public class ScrollExtensions
	{
		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnHorizontalOffsetChanged));


		public static double GetHorizontalOffset(ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(HorizontalOffsetProperty);
		}

		public static void SetHorizontalOffset(ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(HorizontalOffsetProperty, value);
		}

		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToHorizontalOffset((double) e.NewValue);
		}
	}
}
