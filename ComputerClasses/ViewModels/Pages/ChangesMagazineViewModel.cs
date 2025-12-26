using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.Services;
using ComputerClasses.Services.Logs;
using ComputerClasses.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.IO;

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
            if (_workFileService.HasFile())
            {
                LoadData();
            }
            _workFileService.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == _workFileService.fileChanged)
                {
                    LoadData();
                }
            };
        }

        private async void LoadData()
        {
            Logs = [.. await _logService.GetLog(File.OpenRead(_workFileService.GetCurrentWorkFile()))];
        }

    }
}
