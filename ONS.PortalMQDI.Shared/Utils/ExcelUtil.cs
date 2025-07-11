using ClosedXML.Excel;
using System.Collections.Generic;

namespace ONS.PortalMQDI.Shared.Utils
{
    public static class ExcelUtil
    {
        public static IXLWorksheet CreateWorksheetWithHeaders(XLWorkbook workbook, string sheetName, List<string> headers)
        {
            var worksheet = workbook.Worksheets.Add(sheetName);

            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            return worksheet;
        }

        public static void FillWorksheetData(IXLWorksheet worksheet, List<string[]> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    worksheet.Cell(i + 2, j + 1).Value = data[i][j];
                }
            }

            worksheet.Columns().AdjustToContents();
        }

        public static void FillHighlightedRow(IXLWorksheet worksheet, int rowIndex, string[] data, XLColor highlightColor)
        {
            for (int j = 0; j < data.Length; j++)
            {
                var cell = worksheet.Cell(rowIndex, j + 1);
                cell.Value = data[j];
                cell.Style.Fill.BackgroundColor = highlightColor;
            }
        }

    }
}
