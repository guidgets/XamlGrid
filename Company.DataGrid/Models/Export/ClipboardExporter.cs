using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using Company.Widgets.Views;

namespace Company.Widgets.Models.Export
{
	/// <summary>
	/// Represents an exporter to the system clipboard.
	/// </summary>
	public class ClipboardExporter : Exporter
	{
		/// <summary>
		/// Exports the specified information about the data, borders, colors, fonts, etc. of the <see cref="Cell"/>s of a <see cref="DataGrid"/>.
		/// </summary>
		/// <param name="exportInfo">The information to export.</param>
		public override void Export(IEnumerable<List<CellInfo>> exportInfo)
		{
			StringBuilder copy = new StringBuilder();
			foreach (List<CellInfo> rowInfo in exportInfo)
			{
				foreach (CellInfo cellInfo in rowInfo)
				{
					copy.Append(cellInfo.Value);
					copy.Append('\t');
				}
				copy.Append(Environment.NewLine);
			}
			try
			{
				Clipboard.SetText(copy.ToString());
			}
			catch (SecurityException)
			{
				// access to the clipboard may be denied by the user
			}
		}
	}
}
