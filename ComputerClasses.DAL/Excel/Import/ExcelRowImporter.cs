using ComputerClasses.Domain.Entities.Attributes;
using ComputerClasses.Domain.Entities.Interfaces;
using NPOI.SS.UserModel;
using System.IO;
using System.Reflection;

namespace ComputerClasses.Domain.Import
{
    public sealed class ExcelRowImporter
    {
        public ImportResult<Row> Import(Stream excelStream, int sheetIndex = 0, int headerRowIndex = 0)
        {
            var result = new ImportResult<Row>();

            IWorkbook wb = WorkbookFactory.Create(excelStream);
            var sheet = wb.GetSheetAt(sheetIndex) ?? throw new InvalidOperationException("Лист не найден.");

            var headerRow = sheet.GetRow(headerRowIndex);
            if (headerRow == null)
            {
                result.Errors.Add(new ImportError(headerRowIndex + 1, "", "Отсутствует строка заголовков."));
                return result;
            }

            var fileHeaders = ReadHeaders(wb, headerRow, RowsName.Names.Count);

            ValidateHeaders(fileHeaders, result);
            if (result.HasErrors) return result;

            var map = BuildPropertyMap(typeof(Row), fileHeaders, result);
            if (result.HasErrors) return result;

            var ids = new HashSet<int>();
            for (int r = headerRowIndex + 1; r <= sheet.LastRowNum; r++)
            {
                var row = sheet.GetRow(r);
                if (row == null) continue;

                if (IsRowEmpty(row, fileHeaders.Count)) continue;

                var item = new Row();
                bool rowHasError = false;

                foreach (var col in map)
                {
                    int colIndex = col.Key;
                    string colName = fileHeaders[colIndex];
                    var prop = col.Value;

                    try
                    {
                        var cell = row.GetCell(colIndex);
                        SetPropertyFromCell(wb, item, prop, cell, r + 1, colName, result);
                    }
                    catch (Exception ex)
                    {
                        rowHasError = true;
                        result.Errors.Add(new ImportError(r + 1, colName, $"Ошибка обработки: {ex.Message}"));
                    }
                }

                if (item.Id <= 0)
                {
                    rowHasError = true;
                    result.Errors.Add(new ImportError(r + 1, RowsName.Names[0], "Id должен быть положительным целым числом."));
                }
                else if (!ids.Add(item.Id))
                {
                    rowHasError = true;
                    result.Errors.Add(new ImportError(r + 1, RowsName.Names[0], $"Дубликат Id={item.Id}."));
                }

                if (!rowHasError)
                    result.Items.Add(item);
            }

            return result;
        }

        private static List<string> ReadHeaders(IWorkbook wb, IRow headerRow, int expectedCount)
        {
            var formatter = new DataFormatter();

            var headers = new List<string>(expectedCount);

            for (int c = 0; c < expectedCount; c++)
            {
                var cell = headerRow.GetCell(c);
                var text = (cell == null) ? "" : formatter.FormatCellValue(cell)?.Trim();
                headers.Add(text ?? "");
            }
            return headers;
        }

        private static void ValidateHeaders(List<string> fileHeaders, ImportResult<Row> result)
        {
            if (fileHeaders.Count != RowsName.Names.Count)
            {
                result.Errors.Add(new ImportError(1, "", $"Неверное число столбцов. Ожидалось: {RowsName.Names.Count}, получено: {fileHeaders.Count}."));
                return;
            }

            for (int i = 0; i < RowsName.Names.Count; i++)
            {
                var expected = RowsName.Names[i];
                var actual = fileHeaders[i];

                if (!string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase))
                    result.Errors.Add(new ImportError(1, expected, $"Неверный заголовок в столбце {i + 1}. Ожидалось: '{expected}', получено: '{actual}'."));
            }
        }

        private static Dictionary<int, System.Reflection.PropertyInfo> BuildPropertyMap(
            Type rowType,
            List<string> fileHeaders,
            ImportResult<Row> result)
        {
            var props = rowType
                .GetProperties()
                .Where(p => p.CanWrite)
                .ToArray();

            var map = new Dictionary<int, System.Reflection.PropertyInfo>();

            var idProp = props.FirstOrDefault(p => p.Name == nameof(Row.Id) && p.PropertyType == typeof(int));
            if (idProp == null)
            {
                result.Errors.Add(new ImportError(1, RowsName.Names[0], "В Row отсутствует свойство int Id."));
                return map;
            }
            map[0] = idProp;

            var nameToProp = props.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            for (int c = 1; c < fileHeaders.Count; c++)
            {
                string header = fileHeaders[c];

                if (!HeaderToProperty.TryGetValue(header, out var propName))
                {
                    result.Errors.Add(new ImportError(1, header, $"Не задано соответствие заголовка '{header}' свойству Row."));
                    continue;
                }

                if (!nameToProp.TryGetValue(propName, out var prop))
                {
                    result.Errors.Add(new ImportError(1, header, $"В Row отсутствует свойство '{propName}' для заголовка '{header}'."));
                    continue;
                }

                map[c] = prop;
            }

            return map;
        }

        private static readonly Dictionary<string, string> HeaderToProperty = GenerateHeaderToPropertyMap();

        private static Dictionary<string, string> GenerateHeaderToPropertyMap()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var properties = typeof(Row)
                .GetProperties();

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<ExcelColumnAttribute>();

                if (attr != null)
                {
                    map[attr.ColumnName] = prop.Name;
                }
            }

            return map;
        }

        private static void SetPropertyFromCell(
            IWorkbook wb,
            Row target,
            System.Reflection.PropertyInfo prop,
            ICell? cell,
            int excelRowNumber,
            string columnName,
            ImportResult<Row> result)
        {
            var formatter = new DataFormatter();
            string text = cell == null ? "Нет данных" : (formatter.FormatCellValue(cell)?.Trim() ?? "Нет данных");

            if (prop.PropertyType == typeof(int))
            {
                if (string.IsNullOrWhiteSpace(text) || !int.TryParse(text, out var id))
                {
                    result.Errors.Add(new ImportError(excelRowNumber, columnName, "Ожидалось целое число."));
                    return;
                }
                prop.SetValue(target, id);
                return;
            }

            if (typeof(INamedEntity).IsAssignableFrom(prop.PropertyType))
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    result.Errors.Add(new ImportError(excelRowNumber, columnName, "Пустое значение."));
                    //return;
                }

                var obj = (INamedEntity)Activator.CreateInstance(prop.PropertyType)!;
                obj.Name = text ?? "Нет данных";
                prop.SetValue(target, obj);
                return;
            }

            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(target, text);
                return;
            }

            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    if (prop.PropertyType == typeof(DateTime?)) { prop.SetValue(target, null); return; }
                    result.Errors.Add(new ImportError(excelRowNumber, columnName, "Пустая дата."));
                    return;
                }

                DateTime dt;
                if (cell != null && cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                    dt = (DateTime)cell.DateCellValue;
                else if (!DateTime.TryParse(text, out dt))
                {
                    result.Errors.Add(new ImportError(excelRowNumber, columnName, "Некорректная дата."));
                    return;
                }

                prop.SetValue(target, dt);
                return;
            }

            result.Errors.Add(new ImportError(excelRowNumber, columnName, $"Неподдерживаемый тип свойства: {prop.PropertyType.Name}."));
        }

        private static bool IsRowEmpty(IRow row, int colCount)
        {
            for (int c = 0; c < colCount; c++)
            {
                var cell = row.GetCell(c);
                if (cell == null) continue;
                if (cell.CellType != CellType.Blank && !string.IsNullOrWhiteSpace(new DataFormatter().FormatCellValue(cell)))
                    return false;
            }
            return true;
        }
    }
}
