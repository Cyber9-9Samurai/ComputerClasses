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

            //проверка если есть импортированный файл
            if (_workFileService.HasFile())
            {
                //загружаем данные
                LoadData();
            }
            //подписываемся на событие об изменении эеземпляра
            //сервиса хранения текцщего файла
            _workFileService.PropertyChanged += async (s, e) =>
            {
                //если изменился файл
                if (e.PropertyName == _workFileService.fileChanged)
                {
                    //загружаем данные
                    LoadData();
                }
            };
        }

        //метод загрузки данных
        private async void LoadData()
        {
            //получаем логи с помощью сервиса логирования по хэш-ключу
            Logs = [.. await _logService.
            GetLog(File.OpenRead(_workFileService.GetCurrentWorkFile()))];
        }

    }
}
