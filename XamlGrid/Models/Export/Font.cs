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
// File:	Font.cs
// Authors:	Dimitar Dobrev <dpldobrev@gmail.com>

using System.Windows;
using System.Windows.Media;

namespace Guidgets.XamlGrid.Models.Export
{
	public struct Font
	{
		public static readonly Font Default = new Font(new FontFamily("PortableUserInterface"), 11, FontStretches.Normal,
		                                               FontStyles.Normal, FontWeights.Normal, SystemColors.ControlTextColor);

		public FontFamily Family { get; set; }
		public double Size { get; set; }
		public FontStretch Stretch { get; set; }
		public FontStyle Style { get; set; }
		public FontWeight Weight { get; set; }
		public Color Foreground { get; set; }

		public Font(FontFamily family, double size, FontStretch stretch, FontStyle style, FontWeight weight, Color foreground) : this()
		{
			this.Family = family;
			this.Size = size;
			this.Stretch = stretch;
			this.Style = style;
			this.Weight = weight;
			this.Foreground = foreground;
		}

		public Font(FontFamily family, double size, FontStretch stretch, FontStyle style, FontWeight weight, Brush foreground)
			: this(family, size, stretch, style, weight, FromBrush(foreground))
		{

		}

		private static Color FromBrush(Brush brush)
		{
			SolidColorBrush solidColorBrush = brush as SolidColorBrush;
			return solidColorBrush != null ? solidColorBrush.Color : SystemColors.ControlTextColor;
		}
	}
}
