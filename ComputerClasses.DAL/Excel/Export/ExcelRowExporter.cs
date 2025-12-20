using ComputerClasses.Domain;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Test_Import_and_Export.Entities.Interfaces;

namespace Test_Import_and_Export.Export
{
    public static class ExcelRowExporter
    {
        public static void ExportToXlsx(IReadOnlyList<Row> rows, Stream output)
        {
            IWorkbook wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("Data");

            var header = sheet.CreateRow(0);
            for (int c = 0; c < RowsName.Names.Count; c++)
                header.CreateCell(c).SetCellValue(RowsName.Names[c]);

            for (int i = 0; i < rows.Count; i++)
            {
                var r = sheet.CreateRow(i + 1);
                WriteRow(r, rows[i]);
            }

            for (int c = 0; c < RowsName.Names.Count; c++)
                sheet.AutoSizeColumn(c);

            wb.Write(output);
        }

        private static void WriteRow(IRow excelRow, Row row)
        {
            var properties = typeof(Row)
                .GetProperties()
                .Where(p => p.CanRead)
                .ToArray();

            for (int cellIndex = 0; cellIndex < properties.Length; cellIndex++)
            {
                var property = properties[cellIndex];
                var value = property.GetValue(row);

                var cell = excelRow.CreateCell(cellIndex);
                SetCellValue(cell, value);
            }
        }

        private static void SetCellValue(ICell cell, object? value)
        {
            if (value == null)
            {
                cell.SetCellValue("Нет данных");
                return;
            }

            if (value is INamedEntity entity)
            {
                cell.SetCellValue(entity.Name ?? "Нет данных");
                return;
            }

            switch (value)
            {
                case int intVal:
                    cell.SetCellValue(intVal);
                    break;

                case double doubleVal:
                    cell.SetCellValue(doubleVal);
                    break;

                case decimal decimalVal:
                    cell.SetCellValue((double)decimalVal);
                    break;

                case bool boolVal:
                    cell.SetCellValue(boolVal);
                    break;

                case DateTime dateVal:
                    cell.SetCellValue(dateVal);
                    break;

                default:
                    cell.SetCellValue(value.ToString() ?? "Нет данных");
                    break;
            }
        }
    }
}
