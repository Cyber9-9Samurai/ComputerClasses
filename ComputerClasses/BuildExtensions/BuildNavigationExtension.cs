using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.ViewModels.Abstractions;
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
                services.AddScoped<MainWindowViewModel>();
                services.AddScoped(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });
                services.AddScoped<Navigator<PageBaseViewModel>>();
                services.AddScoped<Navigator<PopupBaseViewModel>>();
                services.AddMvvmNavigation();

            });

            return builder;
        }
    }
}
