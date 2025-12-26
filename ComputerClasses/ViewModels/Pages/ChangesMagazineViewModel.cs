using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.Services;
using ComputerClasses.Services.Logs;
using ComputerClasses.ViewModels.Abstractions;
using System.Collections.ObjectModel;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ChangesMagazineViewModel : PageBaseViewModel
    {
        private readonly LogService _logService;
        private readonly WorkFileService _workFileService;
        [ObservableProperty]
        private ObservableCollection<string> logs = new();
        public ChangesMagazineViewModel(LogService logService, WorkFileService workFileService)
        {
            _logService = logService;
            _workFileService = workFileService;
        }

    }
}
