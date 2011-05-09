using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Linq;
using Company.Widgets.Controllers;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using org.in2bits.MyXls;
using Cell = NPOI.SS.UserModel.Cell;
using Row = NPOI.SS.UserModel.Row;

namespace Company.Widgets.Models.Export
{
	public class ExcelXlsExporter : Exporter
	{
		public override void Export(IEnumerable<List<CellInfo>> exportInfo)
		{
			HSSFWorkbook workbook = new HSSFWorkbook();
			Sheet sheet = workbook.CreateSheet("DataGrid");
			HSSFPalette customPalette = workbook.GetCustomPalette();
			int i = 0;
			int j = 0;
			foreach (List<CellInfo> rowInfo in exportInfo)
			{
				Row row = sheet.CreateRow(i);
				foreach (CellInfo cellInfo in rowInfo.Where(c => c.Value != null && c.Value.GetType().IsSimple()))
				{
					Cell cell = row.CreateCell(j++);
					cell.SetCellValue(cellInfo.Value.ToString());
					CellStyle cellStyle = workbook.CreateCellStyle();
					//cell.CellStyle.FillForegroundColor = customPalette.FindColor(cellInfo.Font.Foreground.R, cellInfo.Font.Foreground.G, cellInfo.Font.Foreground.B).GetIndex();
					cellStyle.FillBackgroundColor = HSSFColor.RED.index;
					cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
					cell.CellStyle = cellStyle;
					//cell.Style = new CellStyle();
					//cell.Style.BackColor = cellInfo.Background;
					//worksheet.Cells[i, j++] = cell;
					//Color border = new Color(cellInfo.Border.R, cellInfo.Border.G, cellInfo.Border.B);
					//cell.TopLineColor = cell.BottomLineColor = cell.LeftLineColor = cell.RightLineColor = border;
					//cell.PatternColor = new Color(cellInfo.Background.R, cellInfo.Background.G, cellInfo.Background.B);
				}
				++i;
				j = 0;
			}
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultFileName = "Excel.xls";
			if (saveFileDialog.ShowDialog() == true)
			{
				using (Stream stream = saveFileDialog.OpenFile())
				{
					workbook.Write(stream);
				}
			}
		}

		//public override void Export(IEnumerable<List<CellInfo>> exportInfo)
		//{
		//    XlsDocument xlsDocument = new XlsDocument();
		//    Worksheet worksheet = xlsDocument.Workbook.Worksheets.Add("DataGrid");
		//    int i = 1;
		//    int j = 1;
		//    foreach (List<CellInfo> rowInfo in exportInfo)
		//    {
		//        foreach (CellInfo cellInfo in rowInfo.Where(c => c.Value != null && c.Value.GetType().IsSimple()))
		//        {
		//            Cell cell = worksheet.Cells.Add(i, j++, cellInfo.Value);
		//            cell.UseBackground = true;
		//            cell.Pattern = FillPattern.Solid;
		//            Color border = new Color(cellInfo.Border.R, cellInfo.Border.G, cellInfo.Border.B);
		//            cell.TopLineColor = cell.BottomLineColor = cell.LeftLineColor = cell.RightLineColor = border;
		//            cell.PatternColor = new Color(cellInfo.Background.R, cellInfo.Background.G, cellInfo.Background.B);
		//        }
		//        ++i;
		//        j = 1;
		//    }
		//    SaveFileDialog saveFileDialog = new SaveFileDialog();
		//    saveFileDialog.DefaultFileName = "Excel.xls";
		//    if (saveFileDialog.ShowDialog() == true)
		//    {
		//        using (Stream stream = saveFileDialog.OpenFile())
		//        {
		//            xlsDocument.Save(stream);
		//        }
		//    }
		//}

		//public override void Export(IEnumerable<List<CellInfo>> exportInfo)
		//{
		//    Workbook workbook = new Workbook();
		//    Worksheet worksheet = new Worksheet("DataGrid");
		//    workbook.Worksheets.Add(worksheet);
		//    int i = 0;
		//    int j = 0;
		//    foreach (List<CellInfo> rowInfo in exportInfo)
		//    {
		//        foreach (CellInfo cellInfo in rowInfo.Where(c => c.Value != null && c.Value.GetType().IsSimple()))
		//        {
		//            Cell cell = new Cell(cellInfo.Value);
		//            cell.Style = new CellStyle();
		//            cell.Style.BackColor = cellInfo.Background;
		//            worksheet.Cells[i, j++] = cell;
		//            //Color border = new Color(cellInfo.Border.R, cellInfo.Border.G, cellInfo.Border.B);
		//            //cell.TopLineColor = cell.BottomLineColor = cell.LeftLineColor = cell.RightLineColor = border;
		//            //cell.PatternColor = new Color(cellInfo.Background.R, cellInfo.Background.G, cellInfo.Background.B);
		//        }
		//        ++i;
		//        j = 0;
		//    }
		//    SaveFileDialog saveFileDialog = new SaveFileDialog();
		//    saveFileDialog.DefaultFileName = "Excel.xls";
		//    if (saveFileDialog.ShowDialog() == true)
		//    {
		//        using (Stream stream = saveFileDialog.OpenFile())
		//        {
		//            workbook.Save(stream);
		//        }
		//    }
		//}
	}
}
