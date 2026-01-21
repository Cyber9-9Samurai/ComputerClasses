using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Collections.ObjectModel;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class FilterPopupViewModel : PopupBaseViewModel
    {
        private Navigator<PopupBaseViewModel> _navigatorPopup;
        private Navigator<PageBaseViewModel> _pageNavigator;
        
        private readonly RowsDataSettingsService _rowsDataSettingsService;

        [ObservableProperty]
        private ObservableCollection<string> faculties = new();
        [ObservableProperty]
        private ObservableCollection<string> statuses = new();
        [ObservableProperty]
        private ObservableCollection<string> operatingSystems = new();
        [ObservableProperty]
        private ObservableCollection<string> resposibles = new();
        [ObservableProperty]
        private ObservableCollection<string> ramTypes = new();
        [ObservableProperty]
        private ObservableCollection<string> frames = new();

        [ObservableProperty]
        private string? statusesSelected;
        [ObservableProperty]
        private string? opertaingSystemSelected;
        [ObservableProperty]
        private string? resposibleSelected;
        [ObservableProperty]
        private string? facultySelected;
        [ObservableProperty]
        private string? ramTypeSelected;
        [ObservableProperty]
        private string? frameSelected;

        [ObservableProperty]
        private bool isClose;

        private bool isCleared;

        [ObservableProperty]
        private string? ramsQuantityText;
        [ObservableProperty]
        private string? computersQuantityText;

        public FilterPopupViewModel(Navigator<PopupBaseViewModel> navigatorPopup,
            Navigator<PageBaseViewModel> navigatorPage,
            RowsDataSettingsService rowsDataSettingsService) 
        {
            _navigatorPopup = navigatorPopup;
            _pageNavigator = navigatorPage;
            _rowsDataSettingsService = rowsDataSettingsService;
            _rowsDataSettingsService.PropertyChanged += async (s, e) =>
            {
                await SetCollections();
            };
            _ = SetCollections();
        }

        [RelayCommand]
        private async Task Apply()
        {
            bool valid = await Validate();
            if (valid)
            {
                Close(true);
            }
            else 
            { 
                Close(false);
            }
        }

        [RelayCommand]
        private async Task Clear()
        {
            StatusesSelected = string.Empty;
            OpertaingSystemSelected = string.Empty;
            ResposibleSelected = string.Empty;
            FacultySelected = string.Empty;
            RamTypeSelected = string.Empty;
            FrameSelected = string.Empty;
            RamsQuantityText = string.Empty;
            ComputersQuantityText = string.Empty;
            isCleared = true;
        }
        [RelayCommand]
        private async Task Close(bool isApply)
        {
            _navigatorPopup.BackStack.Clear();
            _navigatorPopup.Navigate<EmptyPopupViewModel>();
            _pageNavigator.BackStack.Clear();
            if (isApply)
            {   
                string?[] comboboxFilters = new string?[]
                {StatusesSelected,
                OpertaingSystemSelected,
                ResposibleSelected,
                FacultySelected,
                RamTypeSelected,
                FrameSelected};
                string?[] textboxFilters = new string?[]
                {
                  RamsQuantityText,
                  ComputersQuantityText
                 };
                await _pageNavigator.Navigate<MainPageViewModel>().ApplyFilter(comboboxFilters, textboxFilters,isCleared);
                isCleared = false;
            }
        }

        private async Task SetCollections()
        {
            Faculties = [.. _rowsDataSettingsService.Faculties];
            Statuses = [.. _rowsDataSettingsService.Statuses];
            OperatingSystems = [.. _rowsDataSettingsService.OperatingSystemType];
            Resposibles = [.. _rowsDataSettingsService.Responsible];
            RamTypes = [.. _rowsDataSettingsService.RamTypes];
            Frames = [.. _rowsDataSettingsService.Frames];

            Faculties.Insert(0, string.Empty);
            Statuses.Insert(0, string.Empty);
            OperatingSystems.Insert(0, string.Empty);
            Resposibles.Insert(0, string.Empty);
            RamTypes.Insert(0, string.Empty);
            Frames.Insert(0, string.Empty);
        }

        private async Task<bool> Validate()
        {
            if (!string.IsNullOrWhiteSpace(RamsQuantityText) && !int.TryParse(RamsQuantityText,out int _))
            {
                return false;
            }
            if(!string.IsNullOrWhiteSpace(ComputersQuantityText) && !int.TryParse(ComputersQuantityText,out int _))
            {
                return false;
            }
            return true;
        }
    }
}
