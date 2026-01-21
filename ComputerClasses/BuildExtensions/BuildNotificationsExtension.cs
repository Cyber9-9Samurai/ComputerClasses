using ComputerClasses.ViewModels.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildNotificationsExtension
    {
        //Extension метод для регистрации ViewModel уведомлений
        public static IHostBuilder BuildNotification(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                //регистрация ViewModel
                services.AddScoped<NotificationsViewModel>();
            });
            return hostBuilder;
        }
    }
}
