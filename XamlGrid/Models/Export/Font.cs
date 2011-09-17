using System.Windows;
using System.Windows.Media;

namespace XamlGrid.Models.Export
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
