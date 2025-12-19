using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;

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
                new MenuButtonItem("Главная",_imageService.GetImage("MainPage.gif"),ToMainCommand),
                new MenuButtonItem("Импорт",_imageService.GetImage("Import.gif"),ToImportCommand),
                new MenuButtonItem("Экспорт",_imageService.GetImage("Export.gif"), ToExportCommand),
                new MenuButtonItem("Журнал изменений", _imageService.GetImage("ChangesLog.gif"), ToChangesCommand)
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
