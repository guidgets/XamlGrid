using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using XamlGrid.Aspects;

namespace XamlGrid.Controllers
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


		[Validate]
		public static double GetVerticalOffset([NotNull] ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(VerticalOffsetProperty);
		}

		[Validate]
		public static void SetVerticalOffset([NotNull] ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(VerticalOffsetProperty, value);
		}

		[Validate]
		public static double GetHorizontalOffset([NotNull] ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(HorizontalOffsetProperty);
		}

		[Validate]
		public static void SetHorizontalOffset([NotNull] ScrollViewer scrollViewer, double value)
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

		[Validate]
		public static IScrollInfo GetScrollInfo([NotNull] this ScrollViewer scroll)
		{
			ItemsPresenter itemsPresenter = scroll.Content as ItemsPresenter;
			if (itemsPresenter != null)
			{
				return VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo;
			}
			return scroll.Content as IScrollInfo;
		}
	}
}
