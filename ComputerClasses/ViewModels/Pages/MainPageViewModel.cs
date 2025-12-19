using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class MainPageViewModel : PageBaseViewModel
    {
        [ObservableProperty]
        private string hi = "hi";
        [ObservableProperty]
        private List<MenuButtonItem> operationButtons = new();
        [ObservableProperty]
        private MenuButtonItem searchButton;

        private readonly Navigator<PopupBaseViewModel> _navigator;
        private readonly GetLocalImage _imageService;
        
        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator, GetLocalImage imageService)
        {
            _navigator = navigator;
            _imageService = imageService;
            LoadData();
        }

        private void LoadData()
        {
            SearchButton = new MenuButtonItem("Найти", _imageService.GetImage("Loading.gif"), DoSearchCommand);
            OperationButtons = new List<MenuButtonItem>()
            {
                new MenuButtonItem("Добавить",_imageService.GetImage("Add.gif"),AddCommand),
                new MenuButtonItem("Редактировать",_imageService.GetImage("Edit.gif"),EditCommand),
                new MenuButtonItem("Удалить",_imageService.GetImage("Delete.gif"),RemoveCommand)
            };
        }

        [RelayCommand]
        private void OpenPopup()
        {
            _navigator.Navigate<TestPopupViewModel>();
        }

        [RelayCommand]
        private void DoSearch() { }

        [RelayCommand]
        private void Add()
        {

        }

        [RelayCommand]
        private void Remove() { }

        [RelayCommand]
        private void Edit()
        {

        }
    }
}
