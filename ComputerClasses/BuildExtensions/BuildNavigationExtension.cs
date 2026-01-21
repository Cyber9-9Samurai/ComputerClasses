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
        //extension метод для регистрации 
        public static IHostBuilder BuildNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //Регистрация всех View и соответсвующие им ViewModel
                services.AddMvvmNavigation();

                //Регистрация главной страницы
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });

                //Регистрация навигатора
                services.AddSingleton<Navigator<PageBaseViewModel>>();
                services.AddSingleton<Navigator<PopupBaseViewModel>>();

                //Регистрация ViewModel меню навигации
                services.AddSingleton<NavigationMenuViewModel>();

            });

            return builder;
        }
    }
}
