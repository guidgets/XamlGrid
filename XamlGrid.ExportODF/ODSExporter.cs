using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using AODL.Document;
using AODL.Document.Content.Tables;
using AODL.Document.Content.Text;
using AODL.Document.Export.OpenDocument;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Styles;
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
			List<CellStyle> cellStyles = new List<CellStyle>();
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
					string backgroundColor = ToHTMLColorNoAlpha(cellInfo.Background);
					CellStyle cellStyle = (from style in cellStyles
					                       where style.CellProperties.BackgroundColor == backgroundColor
					                       select style).FirstOrDefault();
					if (cellStyle == null)
					{
						cellStyles.Add(cellStyle = new CellStyle(spreadsheetDocument, "cell" + rowIndex + "_" + columnIndex));
						cellStyle.CellProperties.BackgroundColor = backgroundColor;
						spreadsheetDocument.Styles.Add(cellStyle);
					}
					cell.CellStyle = cellStyle;
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

		private static string ToHTMLColorNoAlpha(Color color)
		{
			return ("#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2")).ToLowerInvariant();
		}
	}
}
