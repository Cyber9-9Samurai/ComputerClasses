using ComputerClasses.ViewModels.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildNotificationsExtension
    {
        public static IHostBuilder BuildNotification(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddScoped<NotificationsViewModel>();
            });
            return hostBuilder;
        }
    }
}
