using System.Windows;
using System.Windows.Media;

namespace Company.Widgets.Views
{
	/// <summary>
	/// Contains a mechanism which enables the color of a <see cref="SolidColorBrush"/> 
	/// to be set to a color from <see cref="SystemColors"/> through XAML.
	/// </summary>
	public static class OSBrush
	{
		private static readonly Color[] systemColors;

		static OSBrush()
		{
			systemColors = new[]
			               	{
								(Color) SolidColorBrush.ColorProperty.GetMetadata(typeof(SolidColorBrush)).DefaultValue,
			               		SystemColors.ActiveBorderColor, SystemColors.ActiveCaptionColor, SystemColors.ActiveCaptionTextColor,
			               		SystemColors.AppWorkspaceColor, 
								SystemColors.DesktopColor,
								SystemColors.ControlColor, SystemColors.ControlDarkColor,
			               		SystemColors.ControlDarkDarkColor, SystemColors.ControlLightColor,
			               		SystemColors.ControlLightLightColor, SystemColors.ControlTextColor, 
			               		SystemColors.GrayTextColor, 
								SystemColors.HighlightColor, SystemColors.HighlightTextColor,
			               		SystemColors.InactiveBorderColor, SystemColors.InactiveCaptionColor, SystemColors.InactiveCaptionTextColor,  
								SystemColors.InfoColor, SystemColors.InfoTextColor, 
								SystemColors.MenuColor, SystemColors.MenuTextColor,
			               		SystemColors.ScrollBarColor, 
								SystemColors.WindowColor, SystemColors.WindowFrameColor, SystemColors.WindowTextColor
			               	};
		}

		/// <summary>
		/// Identifies the OSColor attached property.
		/// </summary>
		public static readonly DependencyProperty OSColorProperty =
			DependencyProperty.RegisterAttached("OSColor", typeof(OSColors), typeof(OSBrush),
			                                    new PropertyMetadata(OSColors.None, OnOSColorChanged));

		private static void OnOSColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SolidColorBrush) d).Color = systemColors[(int) e.NewValue];
		}

		/// <summary>
		/// Gets the corresponding to a system color <see cref="OSColors"/> value of the specified <see cref="SolidColorBrush"/>.
		/// </summary>
		/// <param name="brush">The brush to get the value from.</param>
		/// <returns>The <see cref="OSColors"/> value of the specified brush.</returns>
		public static OSColors GetOSColor(SolidColorBrush brush)
		{
			return (OSColors) brush.GetValue(OSColorProperty);
		}

		/// <summary>
		/// Sets the specified <see cref="OSColors"/> value to the specified <see cref="SolidColorBrush"/>.
		/// </summary>
		/// <param name="brush">The brush to which to set the value.</param>
		/// <param name="osColor">The corresponding to a system color value from the <see cref="OSColors"/> enumeration.</param>
		public static void SetOSColor(SolidColorBrush brush, OSColors osColor)
		{
			brush.SetValue(OSColorProperty, osColor);
		}
	}
}