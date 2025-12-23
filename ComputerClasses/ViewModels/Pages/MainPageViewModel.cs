using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.Services.Notifications;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Microsoft.Win32;
using Mvvm.Navigation;
using System.Collections.ObjectModel;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class MainPageViewModel : PageBaseViewModel
    {

        [ObservableProperty]
        private List<MenuButtonItem> operationButtons = new();
        [ObservableProperty]
        private bool isExistFile;
        [ObservableProperty]
        private ObservableCollection<Row> rows = new();
        [ObservableProperty]
        private string searchText;
        [ObservableProperty]
        private Row? selectedRow;

        private List<Row> baseRows = new();

        private readonly Navigator<PopupBaseViewModel> _navigator;
        private readonly GetLocalImage _imageService;
        private readonly WorkFileService _workFileService;
        private readonly NotificationsService _notificationsService;

        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator, GetLocalImage imageService, WorkFileService workFileService, NotificationsService notificationsService)
        {
            _navigator = navigator;
            _imageService = imageService;
            _workFileService = workFileService;
            _notificationsService = notificationsService;
            LoadData();
        }

        private async void LoadData()
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
                    IsExistFile = _workFileService.HasFile();
                    
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


        public void LoadFile()
        {
            baseRows = _workFileService.ImportData.Items;
            Rows = [.._workFileService.ImportData.Items];
        }




        private void DoSearch()
        {
            var text = SearchText.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
            {
                Rows = [..baseRows.Where(i => i.AudienceName.Name.ToLower().Contains(text) || i.ApplicationList.Name.ToLower().Contains(text))];
            }
            else if (baseRows.Count > Rows.Count)
            {
                Rows = [..baseRows];
            }
        }

        [RelayCommand]
        private void Add()
        {
            SelectedRow = null;
            _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Add, SelectedRow);
        }

        [RelayCommand]
        private void Remove()
        {
            if (SelectedRow != null)
            {
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Remove, SelectedRow);
            }
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedRow != null)
            {
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Edit, SelectedRow);
            }
        }

        [RelayCommand]
        private void CreateFile()
        {
            _workFileService.CreateNewWorkFile();
        }
    }
}
