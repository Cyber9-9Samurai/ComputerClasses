using ComputerClasses.Domain;
using ComputerClasses.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Test_Import_and_Export.Entities.Attributes;
using Test_Import_and_Export.Entities.Interfaces;

namespace Test_Import_and_Export.Export
{
    public static class CsvRowExporter
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);

        private static readonly CultureInfo DefaultCulture = new("ru-RU");

        public static void ExportToCsv(IReadOnlyList<Row> rows, Stream output, Encoding? encoding = null)
        {
            encoding ??= DefaultEncoding;

            if (rows == null || rows.Count == 0)
                throw new ArgumentException("Список записей пуст.", nameof(rows));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            try
            {
                using (var writer = new StreamWriter(output, encoding, leaveOpen: true))
                using (var csv = new CsvWriter(writer, GetCsvConfiguration()))
                {
                    WriteHeaders(csv);
                    WriteRecords(csv, rows);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при экспорте в CSV: {ex.Message}", ex);
            }
        }

        /// Экспорт списка Row в CSV файл по пути
        //public static void ExportToCsvFile(IReadOnlyList<Row> rows, string filePath, Encoding? encoding = null)
        //{
        //    if (string.IsNullOrWhiteSpace(filePath))
        //        throw new ArgumentException("Путь к файлу пуст.", nameof(filePath));

        //    try
        //    {
        //        string? directory = Path.GetDirectoryName(filePath);
        //        if (!string.IsNullOrEmpty(directory))
        //            Directory.CreateDirectory(directory);

        //        using (var stream = File.Create(filePath))
        //        {
        //            ExportToCsv(rows, stream, encoding);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException($"Ошибка при сохранении CSV файла: {ex.Message}", ex);
        //    }
        //}

        private static CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(DefaultCulture)
            {
                Delimiter = ";",
                Encoding = DefaultEncoding,
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                Quote = '"',
            };
        }

        private static void WriteHeaders(CsvWriter csv)
        {
            foreach (var header in RowsName.Names)
            {
                csv.WriteField(header);
            }

            csv.NextRecord();
        }

        private static void WriteRecords(CsvWriter csv, IReadOnlyList<Row> rows)
        {
            var properties = GetOrderedProperties();

            foreach (var row in rows)
            {
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(row);
                    string stringValue = ConvertValueToString(value);
                    csv.WriteField(stringValue);
                }

                csv.NextRecord();
            }
        }

        private static PropertyInfo[] GetOrderedProperties()
        {
            return typeof(Row)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<ExcelColumnAttribute>() != null)
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ExcelColumnAttribute>()!
                })
                .OrderBy(x => x.Attribute.Order)
                .Select(x => x.Property)
                .ToArray();
        }

        private static string ConvertValueToString(object? value)
        {
            if (value == null)
                return "";

            if (value is INamedEntity entity)
                return entity.Name ?? "";

            if (value is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd");

            return value.ToString() ?? "";
        }
    }
}
