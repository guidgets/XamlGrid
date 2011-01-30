using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Company.Widgets.Controllers
{
	/// <summary>
	/// Contains methods that complement missing features of the <see cref="ScrollViewer"/>.
	/// </summary>
	public static class ScrollExtensions
	{
		public static readonly DependencyProperty VerticalOffsetProperty =
			DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnVerticalOffsetChanged));

		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnHorizontalOffsetChanged));


		public static double GetVerticalOffset(ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(VerticalOffsetProperty);
		}

		public static void SetVerticalOffset(ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(VerticalOffsetProperty, value);
		}

		public static double GetHorizontalOffset(ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(HorizontalOffsetProperty);
		}

		public static void SetHorizontalOffset(ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(HorizontalOffsetProperty, value);
		}

		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToVerticalOffset((double) e.NewValue);
		}

		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToHorizontalOffset((double) e.NewValue);
		}

		public static IScrollInfo GetScrollInfo(this ScrollViewer scroll)
		{
			ItemsPresenter itemsPresenter = scroll.Content as ItemsPresenter;
			if (itemsPresenter != null)
			{
				return VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo ??
				       scroll.Content as IScrollInfo;
			}
			return null;
		}
	}
}
