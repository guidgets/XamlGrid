using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using AODL.Document;
using AODL.Document.Content.Tables;
using AODL.Document.Content.Text;
using AODL.Document.Export.OpenDocument;
using AODL.Document.SpreadsheetDocuments;
using AODL.IO;
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
