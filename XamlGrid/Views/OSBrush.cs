// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.
// 
// File:	OSBrush.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System.Windows;
using System.Windows.Media;
using XamlGrid.Aspects;

namespace XamlGrid.Views
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
		[Validate]
		public static OSColors GetOSColor([NotNull] SolidColorBrush brush)
		{
			return (OSColors) brush.GetValue(OSColorProperty);
		}

		/// <summary>
		/// Sets the specified <see cref="OSColors"/> value to the specified <see cref="SolidColorBrush"/>.
		/// </summary>
		/// <param name="brush">The brush to which to set the value.</param>
		/// <param name="osColor">The corresponding to a system color value from the <see cref="OSColors"/> enumeration.</param>
		[Validate]
		public static void SetOSColor([NotNull] SolidColorBrush brush, OSColors osColor)
		{
			brush.SetValue(OSColorProperty, osColor);
		}
	}
}
