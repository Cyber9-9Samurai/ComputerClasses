using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Navigation;
using ComputerClasses.ViewModels.Windows;
using ComputerClasses.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvvm.Navigation;

namespace ComputerClasses.BuildExtensions
{
    public static class BuildNavigationExtension
    {
        public static IHostBuilder BuildNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddMvvmNavigation();

                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });

                services.AddSingleton<Navigator<PageBaseViewModel>>();
                services.AddSingleton<Navigator<PopupBaseViewModel>>();

                services.AddSingleton<NavigationMenuViewModel>();

            });

            return builder;
        }
    }
}
