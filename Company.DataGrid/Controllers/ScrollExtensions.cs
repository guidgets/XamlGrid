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

		public static bool GetHandleArrowKeys(DependencyObject obj)
		{
			return (bool) obj.GetValue(HandleArrowKeysProperty);
		}

		public static void SetHandleArrowKeys(DependencyObject obj, bool value)
		{
			obj.SetValue(HandleArrowKeysProperty, value);
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
	}
}
