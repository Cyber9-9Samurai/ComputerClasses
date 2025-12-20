using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
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
        private List<MenuButtonItem> operationButtons = new();
        [ObservableProperty]
        private List<Row> rows = new();
        [ObservableProperty]
        private string searchText;

        private List<Row> baseRows = new();

        private readonly Navigator<PopupBaseViewModel> _navigator;
        private readonly GetLocalImage _imageService;
        private readonly WorkFileService _workFileService;

        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator, GetLocalImage imageService, WorkFileService workFileService)
        {
            _navigator = navigator;
            _imageService = imageService;
            _workFileService = workFileService;
            LoadData();
        }

        private void LoadData()
        {
            OperationButtons = new List<MenuButtonItem>()
            {
                new MenuButtonItem("Добавить",_imageService.GetImage("Add.gif"),AddCommand),
                new MenuButtonItem("Редактировать",_imageService.GetImage("Edit.gif"),EditCommand),
                new MenuButtonItem("Удалить",_imageService.GetImage("Delete.gif"),RemoveCommand)
            };
            _workFileService.PropertyChanged += (s, e) =>
            {
                if (_workFileService.fileChanged == e.PropertyName)
                {
                    LoadFile();
                }
            };

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SearchText))
                {
                    DoSearch();
                }
            };
        }

        private void LoadFile()
        {
            baseRows = _workFileService.ImportData.Items;
            Rows = _workFileService.ImportData.Items;
        }



        [RelayCommand]
        private void OpenPopup()
        {
            _navigator.Navigate<TestPopupViewModel>();
        }


        private void DoSearch()
        {
            var text = SearchText.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
            {
                Rows = baseRows.Where(i => i.AudienceName.Name.ToLower().Contains(text) || i.ApplicationList.Name.ToLower().Contains(text)).ToList();
            }
            else if(baseRows.Count > Rows.Count)
            {
                Rows = baseRows;
            }
        }

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
