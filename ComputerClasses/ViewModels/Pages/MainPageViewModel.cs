using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
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
        private Dictionary<(int, int), int> comps = new();

        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator,
            GetLocalImage imageService,
            WorkFileService workFileService)
        {
            _navigator = navigator;
            _imageService = imageService;
            _workFileService = workFileService;
            LoadData();
        }

        private async void LoadData()
        {
            if (!IsExistFile)
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

                foreach (var item in baseRows)
                {
                    if (int.TryParse(item.Frame.Name, out int frame) && int.TryParse(item.AudienceNumber.Name, out int number))
                    {
                        if (comps.ContainsKey((frame, number)))
                        {
                            comps[(frame, number)] += 1;
                        }
                        else
                        {
                            comps.Add((frame, number), 1);
                        }
                    }
                }
            }


        }

        [RelayCommand]
        private void OpenFilter()
        {
            if (IsExistFile)
            {
                _navigator.BackStack.Clear();
                _navigator.Navigate<FilterPopupViewModel>();
            }
        }

        public void ApplyFilter(string?[] comboboxfilters, string?[] textboxfilters, bool isCleared = false)
        {
            if (isCleared)
            {
                Rows = [.. baseRows];
                return;
            }
            string?[] comboboxFiltersCopy = comboboxfilters;
            string?[] textboxFilterCopy = textboxfilters;
            for (int j = 0; j < comboboxfilters.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.Status.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                    case 1:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.OperatingSystem.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                    case 2:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.ResponsiblePerson.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                    case 3:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.Facultie.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                    case 4:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.RamType.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                    case 5:
                        if (comboboxfilters[j] is not null && comboboxfilters[j] != string.Empty && comboboxfilters[j] != comboboxFiltersCopy[j])
                        {
                            Rows = [.. Rows.Where(i => i.Frame.Name.Contains(comboboxfilters[j]!))];
                        }
                        break;
                }
            }

            for (int j = 0; j < textboxfilters.Length; j++)
            {
                switch (j)
                {
                    case 0:
                        if (textboxfilters[j] is not null && textboxfilters[j] != string.Empty && textboxfilters[j] != textboxFilterCopy[j])
                        {
                            Rows = [.. Rows.Where(i => int.TryParse(i.Ram.Name, out int a) && a > int.Parse(textboxfilters[j]!))];
                        }
                        break;
                    case 1:
                        if (textboxfilters[j] is not null && textboxfilters[j] != string.Empty && textboxfilters[j] != textboxFilterCopy[j])
                        {
                            var collection = comps.Where(i => i.Value >= int.Parse(textboxfilters[j]!));
                            Rows = [.. Rows.Where(i => int.TryParse(i.Frame.Name,out int frame)
                            && int.TryParse(i.AudienceNumber.Name,out int number) && comps.ContainsKey((frame,number)))];
                        }
                        break;

                }
            }
        }



        public void LoadFile()
        {
            baseRows = new(_workFileService.ImportData.Items);
            Rows = [.. baseRows];
        }

        private void DoSearch()
        {
            var text = SearchText.ToLower();
            if (!string.IsNullOrWhiteSpace(text))
            {
                Rows = [.. baseRows.Where(i => i.AudienceName.Name.ToLower().Contains(text) || i.ApplicationList.Name.ToLower().Contains(text))];
            }
            else if (baseRows.Count > Rows.Count)
            {
                Rows = [.. baseRows];
            }
        }

        [RelayCommand]
        private void Add()
        {
            if (IsExistFile)
            {
                SelectedRow = null;
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Add, SelectedRow);
            }
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
