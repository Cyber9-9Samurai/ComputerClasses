using ComputerClasses.Domain.Import;
using ComputerClasses.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildServicesExtension
    {
        public static IHostBuilder BuildServices(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<GetLocalImage>();
                services.AddSingleton<WorkFileService>();
                services.AddTransient<ExcelRowImporter>();
            });
            return builder;
        }
    }
}
