using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Company.DataGrid.Controllers
{
	public class ScrollExtensions
	{
		public static readonly DependencyProperty HandleArrowKeysProperty =
			DependencyProperty.RegisterAttached("HandleArrowKeys", typeof(bool), typeof(ScrollExtensions),
												new PropertyMetadata(true, OnHandleArrowKeysChanged));

		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollExtensions),
			                                    new PropertyMetadata(0d, OnHorizontalOffsetChanged));


		public static bool GetHandleArrowKeys(ScrollViewer scrollViewer)
		{
			return (bool) scrollViewer.GetValue(HandleArrowKeysProperty);
		}

		public static void SetHandleArrowKeys(ScrollViewer scrollViewer, bool value)
		{
			scrollViewer.SetValue(HandleArrowKeysProperty, value);
		}

		public static double GetHorizontalOffset(ScrollViewer scrollViewer)
		{
			return (double) scrollViewer.GetValue(HorizontalOffsetProperty);
		}

		public static void SetHorizontalOffset(ScrollViewer scrollViewer, double value)
		{
			scrollViewer.SetValue(HorizontalOffsetProperty, value);
		}

		private static void OnHandleArrowKeysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = (ScrollViewer) d;
			if ((bool) e.NewValue)
			{
				scrollViewer.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnScrollViewerKeyDown));
			}
			else
			{
				scrollViewer.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnScrollViewerKeyDown), true);
			}
		}

		private static void OnScrollViewerKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = false;
		}

		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ScrollViewer) d).ScrollToHorizontalOffset((double) e.NewValue);
		}
	}
}
