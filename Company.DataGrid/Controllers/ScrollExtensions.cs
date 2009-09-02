using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Company.DataGrid.Controllers
{
	public class ScrollExtensions
	{
		public static readonly DependencyProperty MouseWheelProperty =
			DependencyProperty.RegisterAttached("MouseWheel", typeof(bool), typeof(ScrollExtensions),
												new PropertyMetadata(false, OnMouseWheelChanged));

		public static bool GetMouseWheel(ScrollViewer scrollViewer)
		{
			return (bool) scrollViewer.GetValue(MouseWheelProperty);
		}

		public static void SetMouseWheel(ScrollViewer scrollViewer, bool value)
		{
			scrollViewer.SetValue(MouseWheelProperty, value);
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
