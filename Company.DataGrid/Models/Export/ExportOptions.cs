using System;
using Company.Widgets.Views;

namespace Company.Widgets.Models.Export
{
	/// <summary>
	/// Provides options to specify which additional properties except data itself to export.
	/// </summary>
	[Flags]
	public enum ExportOptions
	{
		/// <summary>
		/// Include only the data (values).
		/// </summary>
		None = 0,
		/// <summary>
		/// Include a header, if any, with metadata.
		/// </summary>
		Header = 1,
		/// <summary>
		/// Include the border colors of the <see cref="Cell"/>s.
		/// </summary>
		Border = 2,
		/// <summary>
		/// Include the background colors of the <see cref="Cell"/>s. 
		/// </summary>
		Background = 4,
		/// <summary>
		/// Include the fonts of the <see cref="Cell"/>s.
		/// </summary>
		Font = 8
	}
}
