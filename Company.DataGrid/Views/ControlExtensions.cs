using System.Windows;
using System.Windows.Controls;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Contains methods that extend the functionality of <see cref="Control"/>.
	/// </summary>
	public static class ControlExtensions
	{
		/// <summary>
		/// Identifies an attached property which causes the <see cref="Control"/> it's set to to get focus when it's loaded.
		/// </summary>
		public static readonly DependencyProperty FocusOnLoadProperty =
			DependencyProperty.RegisterAttached("FocusOnLoad", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, OnFocusOnLoadChanged));

		/// <summary>
		/// Gets a value indicating if the specified <see cref="Control"/> will be focused when it's loaded.
		/// </summary>
		/// <param name="control">The control to be focused or not when it's loaded.</param>
		/// <returns><c>true</c>, if the <paramref name="control"/> will be focused; otherwise, <c>false</c>.</returns>
		public static bool GetFocusOnLoad(Control control)
		{
			return (bool) control.GetValue(FocusOnLoadProperty);
		}

		/// <summary>
		/// Sets a value indicating if the specified <see cref="Control"/> will be focused when it's loaded.
		/// </summary>
		/// <param name="control">The control to be focused or not when it's loaded.</param>
		/// <param name="value">if set to <c>true</c> the <paramref name="control"/> will be focused when it's loaded.</param>
		public static void SetFocusOnLoad(Control control, bool value)
		{
			control.SetValue(FocusOnLoadProperty, value);
		}

		private static void OnFocusOnLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool) e.NewValue)
			{
				((FrameworkElement) d).Loaded += Control_Loaded;
			}
			else
			{
				((FrameworkElement) d).Loaded -= Control_Loaded;
			}
		}

		private static void Control_Loaded(object sender, RoutedEventArgs e)
		{
			((Control) sender).Focus();
		}
	}
}
