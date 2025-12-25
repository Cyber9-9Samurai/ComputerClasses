using ComputerClasses.ViewModels.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildNotificationsExtension
    {
        public static IHostBuilder BuildNotification(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context,services) =>
            {
                services.AddScoped<NotificationsViewModel>();
            });
            return hostBuilder;
        }
    }
}
