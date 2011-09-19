using System;
using System.ComponentModel;
using XamlGrid.Controllers;

namespace XamlGrid.Models.Export
{
	/// <summary>
	/// Provides options to specify which additional properties except data itself to export.
	/// </summary>
	[Flags, TypeConverter(typeof(StringToExportOptionsConverter))]
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
		/// Include the border colors of cells.
		/// </summary>
		Border = 2,
		/// <summary>
		/// Include the border colors of cells.
		/// </summary>
		BorderSizes = 4,
		/// <summary>
		/// Include the background colors of cells. 
		/// </summary>
		Background = 8,
		/// <summary>
		/// Include the fonts of the cells.
		/// </summary>
		Font = 16
	}
}
