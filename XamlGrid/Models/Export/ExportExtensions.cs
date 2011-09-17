using XamlGrid.Aspects;
using XamlGrid.Views;

namespace XamlGrid.Models.Export
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
		[Validate]
		public static void Copy([NotNull] this DataGrid dataGrid)
		{
			ClipboardExporter clipboardExporter = new ClipboardExporter();
			clipboardExporter.Options = dataGrid.CopyOptions;
			clipboardExporter.ExportSelected(dataGrid);
		}
	}
}
