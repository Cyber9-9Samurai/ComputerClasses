using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace ComputerClasses.ViewModels.Navigation
{
    public partial class NavigationMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<NavigationItem> navigationItems = new List<NavigationItem>();

        private readonly Navigator<PageBaseViewModel> _navigator;

        public NavigationMenuViewModel(Navigator<PageBaseViewModel> navigator)
        {
            _navigator = navigator;
            LoadData();
        }

        public void LoadData()
        {
            string build = @"pack://application:,,,/";
            string images = @"Resources/Assets/Images/";
            string path = Path.Combine(build,images);
            Debug.WriteLine(path);
            NavigationItems = new List<NavigationItem>
            {
                new NavigationItem("Главная",new BitmapImage(new Uri(Path.Combine(path,"MainPage.gif"))),ToMainCommand),
                new NavigationItem("Импорт",new BitmapImage(new Uri(Path.Combine(path,"Import.gif"))),ToImportCommand),
                new NavigationItem("Экспорт", new BitmapImage(new Uri(Path.Combine(path,"Export.gif"))), ToExportCommand),
                new NavigationItem("Журнал изменений", new BitmapImage(new Uri(Path.Combine(path,"ChangesLog.gif"))), ToChangesCommand)
            };

        }

        [RelayCommand]
        private void ToMain()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<MainPageViewModel>();
        }

        [RelayCommand]
        private void ToImport()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ImportPageViewModel>();
        }

        [RelayCommand]
        private void ToExport()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ExportPageViewModel>();
        }

        [RelayCommand]
        private void ToChanges()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ChangesMagazineViewModel>();
        }
    }
}
