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
        //Extension метод для регистрации вспомогательных сервисов
        public static IHostBuilder BuildServices(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //Регистрация сервиса для получения изображения по названию
                services.AddScoped<GetLocalImage>();
                //Регистрация сервиса для получения данных для модального окна
                services.AddSingleton<RowsDataSettingsService>();
                //Регистрация сервиса для хранения информации о текущем открытом файле
                services.AddSingleton<WorkFileService>();
                //Регистрация сервиса для импорта данных из файла Exel
                services.AddTransient<ExcelRowImporter>();
                //Регистрация сервиса для создания уведомлений
                services.AddScoped<NotificationsService>();
                //Регистрация сервисва для экспорта в файл Exel
                services.AddTransient<ExcelRowExporter>();
                //Регистрация сервиса для экспорта в csv
                services.AddTransient<CsvRowExporter>();
                //Регистрация сервиса для отслеживания и создания логов
                services.AddTransient<LogService>();
                //Регистрация сервиса для экспорта в pdf
                services.AddTransient<PdfRowExporter>();
                //Регистрация сервиса для хранения информации о текущем пользователе
                services.AddSingleton<SessionService>();
            });
            return builder;
        }
    }
}
