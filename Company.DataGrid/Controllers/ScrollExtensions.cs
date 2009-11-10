using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Company.DataGrid.Controllers
{
	public class ScrollExtensions
	{
		public static readonly DependencyProperty MouseWheelProperty =
			DependencyProperty.RegisterAttached("MouseWheel", typeof(bool), typeof(ScrollExtensions),
												new PropertyMetadata(false, OnMouseWheelChanged));

		public static readonly DependencyProperty HandleArrowKeysProperty =
			DependencyProperty.RegisterAttached("HandleArrowKeys", typeof(bool), typeof(ScrollExtensions),
												new PropertyMetadata(true, OnHandleArrowKeysChanged));

		public static bool GetMouseWheel(ScrollViewer scrollViewer)
		{
			return (bool) scrollViewer.GetValue(MouseWheelProperty);
		}

		public static void SetMouseWheel(ScrollViewer scrollViewer, bool value)
		{
			scrollViewer.SetValue(MouseWheelProperty, value);
		}

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

		private static void OnMouseWheelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = (ScrollViewer) d;
			if ((bool) e.NewValue)
			{
				scrollViewer.MouseWheel += OnMouseWheel;
			}
			else
			{
				scrollViewer.MouseWheel -= OnMouseWheel;
			}

		}

		private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scrollViewer = (ScrollViewer) sender;
			if (!e.Handled)
			{
				double offset = Math.Max(Math.Min(scrollViewer.VerticalOffset - e.Delta, scrollViewer.ExtentHeight), 0.0);
				scrollViewer.ScrollToVerticalOffset(offset);
				e.Handled = true;
			}
		}
	}
}
