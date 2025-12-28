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
using System.Windows;
using System.Windows.Controls;
using WpfAnimatedGif;

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
                    new MenuButtonItem("Добавить",_imageService.GetImage("Add.gif"),AddCommand,null),
                    new MenuButtonItem("Редактировать",_imageService.GetImage("Edit.gif"),EditCommand,null),
                    new MenuButtonItem("Удалить",_imageService.GetImage("Delete.gif"),RemoveCommand,null)
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

            var filtered = baseRows.Where(row =>
            {
                for (int j = 0; j < comboboxfilters.Length; j++)
                {
                    if (string.IsNullOrEmpty(comboboxfilters[j])) continue;

                    bool matches = j switch
                    {
                        0 => row.Status?.Name.Contains(comboboxfilters[j]) ?? false,
                        1 => row.OperatingSystem?.Name.Contains(comboboxfilters[j]) ?? false,
                        2 => row.ResponsiblePerson?.Name.Contains(comboboxfilters[j]) ?? false,
                        3 => row.Facultie?.Name.Contains(comboboxfilters[j]) ?? false,
                        4 => row.RamType?.Name.Contains(comboboxfilters[j]) ?? false,
                        5 => row.Frame?.Name.Contains(comboboxfilters[j]) ?? false,
                        _ => true
                    };

                    if (!matches) return false; 
                }

                for (int j = 0; j < textboxfilters.Length; j++)
                {
                    if (string.IsNullOrEmpty(textboxfilters[j])) continue;

                    bool matches = j switch
                    {
                        0 => int.TryParse(row.Ram?.Name, out int ram) &&
                             ram > int.Parse(textboxfilters[j]),
                        1 => int.TryParse(row.Frame?.Name, out int frame) &&
                             int.TryParse(row.AudienceNumber?.Name, out int number) &&
                             comps.ContainsKey((frame, number)) &&
                             comps[(frame, number)] >= int.Parse(textboxfilters[j]),
                        _ => true
                    };

                    if (!matches) return false;
                }

                return true;
            }).ToList();

            Rows = [.. filtered]; 
        }




        public void LoadFile()
        {
            baseRows = [.._workFileService.ImportData.Items];
            Rows = _workFileService.ImportData.Items;
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

        [RelayCommand]
        private void LoadImage(RoutedEventArgs args)
        {
            if (args is not null)
            {
                if (args.Source is Image imageControl)
                {
                    if (imageControl.DataContext is MenuButtonItem item)
                    {
                        item.Animator = ImageBehavior.GetAnimationController(imageControl);
                    }
                }
            }
        }

        [RelayCommand]
        private void Play(MenuButtonItem item)
        {
            item.Animator?.Play();
            if(item.Animator != null && !item.IsSubscribe)
            {
                item.IsSubscribe = true;
                item.Animator.CurrentFrameChanged += (s, e) =>
                {
                    if (item.Animator.CurrentFrame == item.Animator.FrameCount - 1)
                    {
                        item.Animator.Pause();
                        item.Animator.GotoFrame(0);
                    }
                };
            }
        }
    }
}
