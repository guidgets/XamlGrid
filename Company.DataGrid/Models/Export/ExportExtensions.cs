using Company.Widgets.Views;

namespace Company.Widgets.Models.Export
{
	/// <summary>
	/// Contains convenient methods to export the data of a <see cref="DataGrid"/>.
	/// </summary>
	public static class ExportExtensions
	{
		/// <summary>
		/// Copies the selected items of the specified data grid.
		/// </summary>
		/// <param name="dataGrid">The data grid to copy the selected items of.</param>
		public static void Copy(this DataGrid dataGrid)
		{
			ClipboardExporter clipboardExporter = new ClipboardExporter();
			if (!dataGrid.CopyHeaderToClipboard)
			{
				clipboardExporter.Options &= ~ExportOptions.Header;
			}
			clipboardExporter.ExportSelected(dataGrid);
		}
	}
}
