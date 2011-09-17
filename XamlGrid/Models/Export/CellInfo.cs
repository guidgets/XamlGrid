using System.Windows;
using System.Windows.Media;

namespace XamlGrid.Models.Export
{
	public struct CellInfo
	{
		public static readonly CellInfo Default = new CellInfo(null, SystemColors.WindowColor, SystemColors.WindowColor, Font.Default);

		public object Value { get; set; }
		public Color Border { get; set; }
		public Color Background { get; set; }
		public Font Font { get; set; }

		public CellInfo(object value, Color border, Color background, Font font) : this()
		{
			this.Value = value;
			this.Background = background;
			this.Font = font;
			this.Border = border;
		}

		public CellInfo(object value, Brush background, Brush border, Font font)
			: this(value, FromBrush(border, SystemColors.WindowColor), FromBrush(background, SystemColors.WindowColor), font)
		{

		}

		private static Color FromBrush(Brush brush, Color fallBack)
		{
			SolidColorBrush solidColorBrush = brush as SolidColorBrush;
			return solidColorBrush != null ? solidColorBrush.Color : fallBack;
		}
	}
}
