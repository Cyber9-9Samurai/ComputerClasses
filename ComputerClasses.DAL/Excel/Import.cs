using ComputerClasses.Domain.Entities;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.DAL.Excel
{
    public class Import
    {
        public static async Task<List<Domain.Entities.Row>> LoadExcelAsync(string filePath)
        {
            // 1) Асинхронно читаем файл в память
            byte[] bytes;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, bufferSize: 4096, useAsync: true))
            {
                bytes = new byte[fs.Length];
                int offset = 0;

                while (offset < bytes.Length)
                {
                    int read = await fs.ReadAsync(bytes, offset, bytes.Length - offset);
                    if (read == 0) break;
                    offset += read;
                }
            } // файл на диске больше не держим открытым

            // 2) Создаём workbook из MemoryStream (это синхронно в NPOI)
            // WorkbookFactory.Create сам определит XLS/XLSX
            var ms = new MemoryStream(bytes); // ms НЕ dispose здесь, workbook может читать из потока
            IWorkbook workbook = WorkbookFactory.Create(ms);
            ISheet sheet = workbook.GetSheetAt(0);

            //var type = typeof(Domain.Entities.Row);
            //var props = type.GetFields();
            List<Domain.Entities.Row> rows = new();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue; // строки нет (пустая)
                Domain.Entities.Row dataRow = new();
                var props = dataRow.GetType().GetProperties();

                // Здесь row — очередная существующая строка
                // Например: прочитать ячейки в строке
                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    ICell cell = row.GetCell(j);
                    string value = cell.ToString() ?? "Нет данных";

                    var type = props[j].PropertyType;
                    var temp = Activator.CreateInstance(type);
                    if (temp.GetType() == typeof(int))
                    {
                        var property = temp?.GetType().GetProperty("Value");
                        property?.SetValue(dataRow, int.Parse(value));
                    }
                    else
                    {
                        var property = temp?.GetType().GetProperty("Name");
                        property.SetValue(temp, value);

                        if (temp is not null)
                        {
                            props[j].SetValue(dataRow, temp);
                        }
                    }
                }
                rows.Add(dataRow);
            }
            return rows;
        }

        //public static async Task<List<Domain.Entities.Row>> CreateNewExcel(string fileName,
        //    //int rowCount,
        //    CancellationToken token = default)
        //{
        //    // 1. Получаем путь к корню приложения
        //    string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        //    string fullPath = Path.Combine(rootPath, fileName);

        //    // Создаём книгу Excel
        //    IWorkbook workbook = new XSSFWorkbook();
        //    ISheet sheet = workbook.CreateSheet("Data");

        //    List<Domain.Entities.Row> rows = [];

        //    List<string> columnsName = new();
        //    var type = typeof(Domain.RowsName);
        //    var props = type.GetFields();
        //    foreach (var prop in props)
        //    {
        //        object value = prop.GetValue(null);
        //        columnsName.Add((string)value);
        //    }

        //    IRow header = sheet.CreateRow(0);


        //    for (int col = 0; col < columnsName.Count; col++)
        //    {
        //        header.CreateCell(col).SetCellValue(columnsName[col]);
        //    }

        //    // Заполняем данными
        //    //for (int i = 1; i <= rowCount; i++)
        //    //{
        //    //    // Важно: проверяем отмену внутри цикла генерации
        //    //    token.ThrowIfCancellationRequested();

        //    //    IRow row = sheet.CreateRow(i);
        //    //    row.CreateCell(0).SetCellValue(i);
        //    //    row.CreateCell(1).SetCellValue(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        //    //    row.CreateCell(2).SetCellValue($"Item #{i * 10}");
        //    //}

        //    // Автоматически подгоняем ширину колонок (опционально, ресурсоёмко!)
        //    //sheet.AutoSizeColumn(0);
        //    //sheet.AutoSizeColumn(1);
        //    return rows;
        //}
    }
}
