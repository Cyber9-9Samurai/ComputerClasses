using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.IO;

namespace ComputerClasses.Services
{
    public partial class RowsDataSettingsService : ObservableObject
    {
        private string _path = @"pack://application:,,,/";
        private string _localPath = @"Configurations/RowsValue.json";
        [ObservableProperty]
        private List<string> faculties = new();
        [ObservableProperty]
        private List<string> departmentsOrInstitutes = new();
        [ObservableProperty]
        private List<string> frames = new();
        [ObservableProperty]
        private List<string> statuses = new();
        [ObservableProperty]
        private List<string> ramTypes = new();
        [ObservableProperty]
        private List<string> operatingSystemType = new();
        [ObservableProperty]
        private List<string> responsible = new();

        private WorkFileService _workFileService;

        public RowsDataSettingsService()
        {
            _path = Path.Combine(_path, _localPath);
        }

        public async void GetFileService(WorkFileService workFileService)
        {

            _workFileService = workFileService;
             await GetData();
            _workFileService.PropertyChanged += async (s, e) =>
            {
                if(e.PropertyName == _workFileService.fileChanged)
                {
                    await GetData();
                }
            };
        }

        public async Task GetData()
        {
            if(_workFileService is null)
            {
                return;
            }
            if (!_workFileService.HasFile())
            {
                return;
            }
            await IsFirstStart();
            var settings = await CheckUniqueValue();
            if (settings is not null)
            {
                Faculties = settings.Faculties;
                DepartmentsOrInstitutes = settings.DepartmentsOrInstitutes;
                Frames = settings.Frames;
                Statuses = settings.Statuses;
                RamTypes = settings.RamTypes;
                OperatingSystemType = settings.OperatingSystemType;
                Responsible = settings.Responsible;
            }
        }

        private async Task<RowsDataSettingsService> ReadData()
        {
            var jsonData = await File.ReadAllTextAsync(_localPath);
            return JsonConvert.DeserializeObject<RowsDataSettingsService>(jsonData);
        }

        public async Task SaveSettings(RowsDataSettingsService rowsData)
        {
            var jsonData = JsonConvert.SerializeObject(rowsData);
            await File.WriteAllTextAsync(_localPath, jsonData);
        }


        private async Task IsFirstStart()
        {
            if (!File.Exists(_localPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_localPath)!);
                File.Create(_localPath).Close();
                await SaveSettings(new RowsDataSettingsService());
            }
            
        }



        private async Task<RowsDataSettingsService> CheckUniqueValue()
        {
            var settings = await ReadData();
            if (settings is not null)
            {
                foreach (var item in _workFileService.ImportData.Items)
                {
                    if (!settings.Faculties.Contains(item.Facultie.Name) &&
                         !string.IsNullOrWhiteSpace(item.Facultie.Name) &&
                         item.Facultie.Name != "Нет данных")
                    {
                        settings.Faculties.Add(item.Facultie.Name);
                    }
                    if (!settings.DepartmentsOrInstitutes.Contains(item.DepartmentOrInstitute.Name) &&
                         !string.IsNullOrWhiteSpace(item.DepartmentOrInstitute.Name) &&
                         item.DepartmentOrInstitute.Name != "Нет данных")
                    {
                        settings.DepartmentsOrInstitutes.Add(item.DepartmentOrInstitute.Name);
                    }
                    if (!settings.Frames.Contains(item.Frame.Name) &&
                        !string.IsNullOrWhiteSpace(item.Frame.Name) &&
                        item.Frame.Name != "Нет данных")
                    {
                        settings.Frames.Add(item.Frame.Name);
                    }
                    if (!settings.Statuses.Contains(item.Status.Name) &&
                        !string.IsNullOrWhiteSpace(item.Status.Name) &&
                        item.Status.Name != "Нет данных")
                    {
                        settings.Statuses.Add(item.Status.Name);
                    }
                    if (!settings.RamTypes.Contains(item.RamType.Name) &&
                        !string.IsNullOrWhiteSpace(item.RamType.Name) &&
                        item.RamType.Name != "Нет данных")
                    {
                        settings.RamTypes.Add(item.RamType.Name);
                    }
                    if(!settings.OperatingSystemType.Contains(item.OperatingSystem.Name) &&
                        !string.IsNullOrWhiteSpace(item.OperatingSystem.Name) &&
                        item.OperatingSystem.Name != "Нет даных")
                    {
                        settings.OperatingSystemType.Add(item.OperatingSystem.Name);
                    }
                    if (!settings.Responsible.Contains(item.ResponsiblePerson.Name) &&
                        !string.IsNullOrWhiteSpace(item.ResponsiblePerson.Name) &&
                        item.ResponsiblePerson.Name != "Нет даных")
                    {
                        settings.Responsible.Add(item.ResponsiblePerson.Name);
                    }
                }
                
                
            }
            else
            {
                settings = new RowsDataSettingsService();
            }
            await SaveSettings(settings);
            return settings ;
        }
    }
}
