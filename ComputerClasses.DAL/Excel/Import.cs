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
            } 
            var ms = new MemoryStream(bytes); 
            IWorkbook workbook = WorkbookFactory.Create(ms);
            ISheet sheet = workbook.GetSheetAt(0);
            List<Domain.Entities.Row> rows = new();

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                Domain.Entities.Row dataRow = new();
                var props = dataRow.GetType().GetProperties();
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
    }
}
