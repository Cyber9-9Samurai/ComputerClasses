using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ComputerClasses.BuildExtensions;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using ComputerClasses.ViewModels.Windows;
using ComputerClasses.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mvvm.Navigation;

namespace ComputerClasses
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host = CreateBuilder().Build();
        private static IHostBuilder CreateBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .BuildServices()
                .BuildNotification()
                .BuildNavigation();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var MainWindow = _host.Services.GetRequiredService<MainWindow>();
            var navigator = _host.Services.GetRequiredService<Navigator<PageBaseViewModel>>();
            navigator.Navigate<MainPageViewModel>();
            MainWindow.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
