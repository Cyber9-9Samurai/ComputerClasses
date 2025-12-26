using ComputerClasses.DAL.Excel.Export;
using ComputerClasses.Domain.Import;
using ComputerClasses.Services;
using ComputerClasses.Services.Logs;
using ComputerClasses.Services.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test_Import_and_Export.Export;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildServicesExtension
    {
        public static IHostBuilder BuildServices(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<GetLocalImage>();
                services.AddSingleton<RowsDataSettingsService>();
                services.AddSingleton<WorkFileService>();
                services.AddTransient<ExcelRowImporter>();
                services.AddScoped<NotificationsService>();
                services.AddTransient<ExcelRowExporter>();
                services.AddTransient<CsvRowExporter>();
                services.AddTransient<LogService>();
                services.AddTransient<PdfRowExporter>();
                services.AddSingleton<SessionService>();
            });
            return builder;
        }
    }
}
