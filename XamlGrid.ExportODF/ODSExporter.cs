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
// File:	ODSExporter.cs
// Authors:	Dimitar Dobrev <dpldobrev at yahoo dot com>

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using AODL.Document;
using AODL.Document.Content.Tables;
using AODL.Document.Content.Text;
using AODL.Document.Export.OpenDocument;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.SpreadsheetDocuments.Tables.Style;
using AODL.IO;
using XamlGrid.Controllers;
using XamlGrid.Models.Export;

namespace XamlGrid.ExportODF
{
	public class ODSExporter : Exporter
	{
		public override void Export(IEnumerable<List<CellInfo>> exportInfo)
		{
			SpreadsheetDocument spreadsheetDocument = new SpreadsheetDocument();
			spreadsheetDocument.New();
			Table table = new Table(spreadsheetDocument, "DataGrid", string.Empty);
			int rowIndex = 0;
			foreach (List<CellInfo> cellInfos in exportInfo)
			{
				for (int columnIndex = 0; columnIndex < cellInfos.Count; columnIndex++)
				{
					CellInfo cellInfo = cellInfos[columnIndex];
					Cell cell = table.CreateCell();
					Paragraph paragraph = ParagraphBuilder.CreateSpreadsheetParagraph(spreadsheetDocument);
					SimpleText fText = new SimpleText(spreadsheetDocument, (cellInfo.Value ?? string.Empty).ToString());
					paragraph.TextContent.Add(fText);
					cell.Content.Add(paragraph);
					table.InsertCellAt(rowIndex, columnIndex, cell);
				}
				rowIndex++;
			}
			spreadsheetDocument.TableCollection.Add(table);
			Save(spreadsheetDocument);
		}

		private static string GetODSType(Type type)
		{
			if (type == typeof(DateTime) || type == typeof(DateTime?))
			{
				return OfficeValueTypes.Date;
			}
			if (type == typeof(bool) || type == typeof(bool?))
			{
				return OfficeValueTypes.Boolean;
			}
			if (type.IsNumeric())
			{
				return OfficeValueTypes.Float;
			}
			return OfficeValueTypes.String;
		}

		private static void Save(IDocument spreadsheetDocument)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultFileName = "DataGrid";
			saveFileDialog.DefaultExt = ".ods";
			if (saveFileDialog.ShowDialog() == true)
			{
				using (Stream stream = saveFileDialog.OpenFile())
				{
					using (InMemoryPackageWriter inMemoryPackageWriter = new InMemoryPackageWriter(stream))
					{
						OpenDocumentTextExporter openDocumentTextExporter = new OpenDocumentTextExporter(inMemoryPackageWriter);
						spreadsheetDocument.Save(saveFileDialog.SafeFileName, openDocumentTextExporter);
					}
				}
			}
		}
	}
}
