using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Navigation
{
    public partial class NavigationMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<MenuButtonItem> navigationItems = new List<MenuButtonItem>();

        private readonly Navigator<PageBaseViewModel> _navigator;
        private readonly GetLocalImage _imageService;
        public NavigationMenuViewModel(Navigator<PageBaseViewModel> navigator, GetLocalImage imageService)
        {
            _navigator = navigator;
            _imageService = imageService;
            LoadData();
        }

        public void LoadData()
        {
            NavigationItems = new List<MenuButtonItem>
            {
                new MenuButtonItem("Главная",_imageService.GetImage("MainPage.png"),ToMainCommand,null),
                new MenuButtonItem("Импорт",_imageService.GetImage("Import.png"),ToImportCommand,null),
                new MenuButtonItem("Экспорт",_imageService.GetImage("Export.png"), ToExportCommand,null),
                new MenuButtonItem("Журнал изменений", _imageService.GetImage("ChangesLog.png"), ToChangesCommand,null)
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
