using ComputerClasses.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            });
            return builder;
        }
    }
}
