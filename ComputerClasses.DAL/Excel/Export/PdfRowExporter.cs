using ComputerClasses.Domain;
using ComputerClasses.Domain.Entities.Attributes;
using ComputerClasses.Domain.Entities.Interfaces;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using System.Reflection;

namespace ComputerClasses.DAL.Excel.Export
{
    public class PdfRowExporter
    {
        public void ExportToPdf(IReadOnlyList<Row> rows, Stream output)
        {
            using var writer = new PdfWriter(output);
            using var pdf = new PdfDocument(writer);

            pdf.SetDefaultPageSize(PageSize.A3.Rotate());

            string fontPath = @"C:\Windows\Fonts\arial.ttf";
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            using var document = new Document(pdf);
            document.SetMargins(10, 10, 10, 10);

            document.SetFont(font);

            AddTitle(document, font);
            AddTable(document, rows, font);
        }

        public void ExportToPdfFile(IReadOnlyList<Row> rows, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу пуст.", nameof(filePath));

            try
            {
                string? directory = System.IO.Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                using (var stream = File.Create(filePath))
                {
                    ExportToPdf(rows, stream);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении PDF файла: {ex.Message}", ex);
            }
        }


        private void AddTitle(Document document, PdfFont font)
        {
            var title = new Paragraph("Отчёт по компьютерной технике")
                .SetFont(font)
                .SetFontSize(18)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(5);

            document.Add(title);

            var subtitle = new Paragraph($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm:ss}")
                .SetFont(font)
                .SetFontSize(9)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(10);

            document.Add(subtitle);
        }

        private void AddTable(Document document, IReadOnlyList<Row> rows, PdfFont font)
        {
            var properties = GetOrderedProperties();

            var table = new Table(RowsName.Names.Count);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetKeepTogether(false);

            foreach (var header in RowsName.Names)
            {
                var cell = new Cell()
                    .Add(new Paragraph(header)
                        .SetFont(font)
                        .SetFontSize(7))
                    .SetBackgroundColor(new DeviceRgb(70, 130, 180))
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(2);

                table.AddHeaderCell(cell);
            }

            foreach (var row in rows)
            {
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(row);
                    string stringValue = ConvertValueToString(value);

                    var cell = new Cell()
                        .Add(new Paragraph(stringValue)
                            .SetFont(font)
                            .SetFontSize(6))
                        .SetPadding(1)
                        .SetTextAlignment(TextAlignment.LEFT);

                    table.AddCell(cell);
                }
            }

            document.Add(table);
        }

        private PropertyInfo[] GetOrderedProperties()
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

        private string ConvertValueToString(object? value)
        {
            if (value == null)
                return "—";

            if (value is INamedEntity entity)
                return entity.Name ?? "—";

            if (value is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd");

            return value.ToString() ?? "—";
        }
    }
}
