using System.Windows;
using System.Windows.Media;

namespace XamlGrid.Models.Export
{
	public struct CellInfo
	{
		public static readonly CellInfo Default = new CellInfo(null, SystemColors.WindowColor, SystemColors.WindowColor, new Thickness(), Font.Default);

		public object Value { get; set; }

		/// <summary>
		/// Gets or sets the border color of the exported cell.
		/// </summary>
		/// <value>
		/// The border color to use.
		/// </value>
		public Color Border { get; set; }

		public Color Background { get; set; }

		/// <summary>
		/// Gets or sets the border sizes (top, bottom, left, right) of the cell.
		/// </summary>
		/// <value>
		/// The borders of the cell.
		/// </value>
		public Thickness BorderSizes { get; set; }

		public Font Font { get; set; }

		public CellInfo(object value, Color border, Color background, Thickness borderSizes, Font font) : this()
		{
			this.Value = value;
			this.Background = background;
			this.BorderSizes = borderSizes;
			this.Font = font;
			this.Border = border;
		}

		public CellInfo(object value, Brush background, Brush border, Thickness borderSizes, Font font)
			: this(value, FromBrush(border), FromBrush(background), borderSizes, font)
		{

		}

		private static Color FromBrush(Brush brush)
		{
			SolidColorBrush solidColorBrush = brush as SolidColorBrush;
			return solidColorBrush != null ? solidColorBrush.Color : Default.Background;
		}
	}
}
