using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Domain.Entities;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.Services.Logs;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class ChangeDataPopupViewModel : PopupBaseViewModel
    {
        [ObservableProperty]
        private DataChangesActions actions;
        [ObservableProperty]
        private Row data;
        private int lastId = 0;

        [ObservableProperty]
        private string popupTitle = string.Empty;
        
        private readonly RowsDataSettingsService _rowsSettings;

        [ObservableProperty]
        private string selectedFacultie;
        [ObservableProperty]
        private string selctedDepartmentOrInstitude;
        [ObservableProperty]
        private string selectedFrame;
        [ObservableProperty]
        private string selecetedStatus;
        [ObservableProperty]
        private string selecetedRamType;

        [ObservableProperty]
        private string facultieAddText;
        [ObservableProperty]
        private string departmentOrInstitudeAddText;
        [ObservableProperty]
        private string frameAddText;
        [ObservableProperty]
        private string statusAddText;
        [ObservableProperty]
        private string ramTypeAddText;

        [ObservableProperty]
        private ObservableCollection<string> faculties = new();
        [ObservableProperty]
        private ObservableCollection<string> departments = new();
        [ObservableProperty]
        private ObservableCollection<string> frames = new();
        [ObservableProperty]
        private ObservableCollection<string> ramTypes = new();
        [ObservableProperty]
        private ObservableCollection<string> statuses = new();

        [ObservableProperty]
        private ObservableCollection<string> errors = new();

        [ObservableProperty]
        private string facultiesName;
        [ObservableProperty]
        private string departmentName;
        [ObservableProperty]
        private string frameName;
        [ObservableProperty]
        private string statusName;
        [ObservableProperty]
        private string ramTypeName;

        private readonly WorkFileService _workFileService;
        private readonly Navigator<PopupBaseViewModel> _navigatorPopup;
        private readonly Navigator<PageBaseViewModel> _navigatorPage;
        private readonly LogService _logService;
        private readonly ChangesMagazineViewModel _changesMagazineViewModel;
        private readonly SessionService _session;


        public ChangeDataPopupViewModel(WorkFileService workFileService,
            Navigator<PopupBaseViewModel> navigatorPopup,
            Navigator<PageBaseViewModel> navigatorPage,
            RowsDataSettingsService rowsSettings,
            LogService logService,
            ChangesMagazineViewModel changesMagazineViewModel,
            SessionService session)
        {

            _changesMagazineViewModel = changesMagazineViewModel;
            _workFileService = workFileService;
            _navigatorPopup = navigatorPopup;
            _navigatorPage = navigatorPage;
            _rowsSettings = rowsSettings;
            _logService = logService;
            _session = session;
            
        }

        public void GetData(DataChangesActions actions, Row? row)
        {
            lastId = _workFileService.ImportData.Items.Count > 0 ? _workFileService.ImportData.Items.Max(i => i.Id) + 1 : 1;
            Actions = actions;
            if (row == null)
            {
                Data = new Row()
                {
                    Id = lastId,
                    Facultie = new Facultie(),
                    DepartmentOrInstitute = new DepartmentOrInstitute(),
                    Frame = new Domain.Entities.Frame(),
                    AudienceNumber = new AudienceNumber(),
                    AudienceName = new AudienceName(),
                    ResponsiblePerson = new ResponsiblePerson(),
                    InventoryNumber = new InventoryNumber(),
                    OperatingSystem = new Domain.Entities.OperatingSystem(),
                    Motherboard = new Motherboard(),
                    Cpu = new Cpu(),
                    VideoCard = new VideoCard(),
                    Disk = new Disks(),
                    Ram = new Ram(),
                    RamType = new RamType(),
                    ApplicationList = new InstalledApplications(),
                    Status = new Status(),
                    LastServiceDate = new LastServiceDate()
                };
            }
            else
            {
                Data = new Row()
                {
                    Id = row.Id,
                    Facultie = new Facultie() { Id = row.Facultie.Id, Name = row.Facultie.Name },
                    DepartmentOrInstitute = new DepartmentOrInstitute() { Id = row.DepartmentOrInstitute.Id, Name = row.DepartmentOrInstitute.Name },
                    Frame = new Domain.Entities.Frame() { Id = row.Frame.Id, Name = row.Frame.Name },
                    AudienceNumber = new AudienceNumber() { Id = row.AudienceNumber.Id, Name = row.AudienceNumber.Name },
                    AudienceName = new AudienceName() { Id = row.AudienceName.Id, Name = row.AudienceName.Name },
                    ResponsiblePerson = new ResponsiblePerson() { Id = row.ResponsiblePerson.Id, Name = row.ResponsiblePerson.Name },
                    InventoryNumber = new InventoryNumber() { Id = row.InventoryNumber.Id, Name = row.InventoryNumber.Name },
                    OperatingSystem = new Domain.Entities.OperatingSystem() { Id = row.OperatingSystem.Id, Name = row.OperatingSystem.Name },
                    Motherboard = new Motherboard() { Id = row.Motherboard.Id, Name = row.Motherboard.Name },
                    Cpu = new Cpu() { Id = row.Cpu.Id, Name = row.Cpu.Name },
                    VideoCard = new VideoCard() { Id = row.VideoCard.Id, Name = row.VideoCard.Name },
                    Disk = new Disks() { Id = row.Disk.Id, Name = row.Disk.Name },
                    Ram = new Ram() { Id = row.Ram.Id, Name = row.Ram.Name },
                    RamType = new RamType() { Id = row.RamType.Id, Name = row.RamType.Name },
                    ApplicationList = new InstalledApplications() { Id = row.ApplicationList.Id, Name = row.ApplicationList.Name },
                    Status = new Status() { Id = row.Status.Id, Name = row.Status.Name },
                    LastServiceDate = new LastServiceDate() { Id = row.LastServiceDate.Id, Name = row.LastServiceDate.Name }
                };
            }
            Faculties = [.. _rowsSettings.Faculties];
            Departments = [.. _rowsSettings.DepartmentsOrInstitutes];
            Frames = [.. _rowsSettings.Frames];
            RamTypes = [.. _rowsSettings.RamTypes];
            Statuses = [.. _rowsSettings.Statuses];

            SelectedFacultie = Data.Facultie.Name;
            SelctedDepartmentOrInstitude = Data.DepartmentOrInstitute.Name;
            SelectedFrame = Data.Frame.Name;
            SelecetedRamType = Data.RamType.Name;
            SelecetedStatus = Data.Status.Name;

            FacultiesName = nameof(Faculties);
            DepartmentName = nameof(Departments);
            FrameName = nameof(Frames);
            RamTypeName = nameof(RamTypes);
            StatusName = nameof(Statuses);

            Errors.Clear();

            PopupTitle = Actions switch
            {
                DataChangesActions.Add => "Добавление",
                DataChangesActions.Edit => "Редактирование",
                DataChangesActions.Remove => "Удаление",
                _ => ""
            };
        }

        [RelayCommand]
        public void Close()
        {
            _navigatorPopup.BackStack.Clear();
            _navigatorPopup.Navigate<EmptyPopupViewModel>();
            _navigatorPage.BackStack.Clear();
            _navigatorPage.Navigate<MainPageViewModel>();
        }

        //метод для сохранения изменений
        [RelayCommand]
        public async Task AgreeChanges()
        {
            //пытаемся применить изменения
            try
            {
                //проверяем какое действие выполняется
                switch (Actions)
                {
                    //если добавление
                    case DataChangesActions.Add:
                        {
                            //проверяем правильно ли введены данные
                            if (!IsValidData())
                            {
                                return;
                            }
                            //добавляем в коллекцию к импортированным данным
                            _workFileService.ImportData.Items.Add(Data);
                            //логируем действие
                            _changesMagazineViewModel.Logs = [..await _logService.
                            WriteLogLine(_changesMagazineViewModel.Logs.ToList(),DataChangesActions.Add, Data, _session.Name)];
                            await AddTemplateCommand.ExecuteAsync(nameof(_rowsSettings.OperatingSystemType));
                            await AddTemplateCommand.ExecuteAsync(nameof(_rowsSettings.Responsible));
                            break;
                        }
                    //если изменение
                    case DataChangesActions.Edit:
                        {
                            //проверяем правильно ли введены данные
                            if (!IsValidData())
                            {
                                return;
                            }
                            //получаем строку по id
                            var item = _workFileService.ImportData.Items.FirstOrDefault(i => i.Id == Data.Id);
                            //проверка если строка существует
                            if (item != null)
                            {
                                //получаем индекс элемента в коллекции
                                var index = _workFileService.ImportData.Items.IndexOf(item);
                                //изменяет заначение в импортированных данных по индексу
                                _workFileService.ImportData.Items[index] = Data;
                                //логируем действие
                                _changesMagazineViewModel.Logs = [..await _logService.
                            WriteLogLine(_changesMagazineViewModel.Logs.ToList(),DataChangesActions.Edit, Data, _session.Name)];
                            }
                            break;
                        }
                    //если удаление
                    case DataChangesActions.Remove:
                        {
                            //получаем строку по id
                            var item = _workFileService.ImportData.Items.FirstOrDefault(i => i.Id == Data.Id);
                            //проверка если строка существует
                            if (item != null)
                            {
                                //удаляем элемент из коллекции импортированных данных
                                _workFileService.ImportData.Items.Remove(item);
                                //логируем действие
                                _changesMagazineViewModel.Logs = [..await _logService.
                            WriteLogLine(_changesMagazineViewModel.Logs.ToList(),DataChangesActions.Remove, Data, _session.Name)];
                            }
                            break;
                        }
                }
                //закрываем текущее модальное окно
                Close();
            }
            //если произошло исключение
            catch (Exception)
            {
                //открываем модальное окно вывода сообщений об ошибках
                _navigatorPopup.Navigate<ErrorPopupViewModel>();
            }
        }

        private bool IsValidData()
        {
            Errors.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(SelectedFacultie))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.facultieIsNotSelected);
            }
            else
            {
                Data.Facultie.Name = SelectedFacultie;
            }

            if (string.IsNullOrWhiteSpace(SelctedDepartmentOrInstitude))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.departmentOrInstitudeIsNotSelected);
            }
            else
            {
                Data.DepartmentOrInstitute.Name = SelctedDepartmentOrInstitude;
            }

            if (string.IsNullOrWhiteSpace(SelectedFrame))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.frameIsNotSelected);
            }
            else
            {
                Data.Frame.Name = SelectedFrame;
            }

            if (!int.TryParse(Data.AudienceNumber.Name, out int _))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.audienceNumberIsNotRight);
            }

            if (string.IsNullOrWhiteSpace(Data.AudienceName.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.audienceNameIsNotSelected);
            }

            if (string.IsNullOrWhiteSpace(Data.ResponsiblePerson.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.resposibleIsNotSelected);
            }

            if (!Regex.IsMatch(Data.InventoryNumber.Name, @"^\d+$"))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.inventoryNumberIsNotRight);
            }

            if (string.IsNullOrWhiteSpace(Data.OperatingSystem.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.opertingSystemIsNotSelected);
            }

            if (string.IsNullOrWhiteSpace(Data.Motherboard.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.motherBoardIsNotSelected);
            }

            if (string.IsNullOrWhiteSpace(Data.Cpu.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.cpuIsNotSelected);
            }

            if (string.IsNullOrWhiteSpace(Data.VideoCard.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.videoCardIsNotSelected);
            }

            if (string.IsNullOrWhiteSpace(Data.Disk.Name))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.diskIsNotSelected);
            }

            if (!int.TryParse(Data.Ram.Name, out int _))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.ramIsNotRight);
            }

            if (string.IsNullOrWhiteSpace(SelecetedRamType))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.ramTypeIsNotSelected);
            }
            else
            {
                Data.RamType.Name = SelecetedRamType;
            }

            if (string.IsNullOrWhiteSpace(Data.ApplicationList.Name))
            {
                Data.ApplicationList.Name = "Нет данных";
            }

            if (string.IsNullOrWhiteSpace(SelecetedStatus))
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.statusIsNotSelected);
            }
            else
            {
                Data.Status.Name = SelecetedStatus;
            }

            if (!DateTime.TryParse(Data.LastServiceDate.Name, out DateTime date) || date > DateTime.Today)
            {
                valid = false;
                Errors.Add(ChangesDataErrorsMessages.lastSeviceDateIsNotSelected);
            }
            return valid;
        }


        [RelayCommand]
        private async Task DeleteFacultiesTemplate(string item)
        {
            Faculties.Remove(item);
            _rowsSettings.Faculties.Remove(item);
            await _rowsSettings.SaveSettings(_rowsSettings);
        }

        [RelayCommand]
        private async Task DeleteDepartmentsTemplate(string item)
        {
            Departments.Remove(item);
            _rowsSettings.DepartmentsOrInstitutes.Remove(item);
            await _rowsSettings.SaveSettings(_rowsSettings);
        }

        [RelayCommand]
        private async Task DeleteFramesTemplate(string item)
        {
            Frames.Remove(item);
            _rowsSettings.Frames.Remove(item);
            await _rowsSettings.SaveSettings(_rowsSettings);
        }

        [RelayCommand]
        private async Task DeleteRamTypesTemplate(string item)
        {
            RamTypes.Remove(item);
            _rowsSettings.RamTypes.Remove(item);
            await _rowsSettings.SaveSettings(_rowsSettings);
        }

        [RelayCommand]
        private async Task DeleteStatusesTeplate(string item)
        {
            Statuses.Remove(item);
            _rowsSettings.Statuses.Remove(item);
            await _rowsSettings.SaveSettings(_rowsSettings);
        }


        [RelayCommand]
        private async Task AddTemplate(string collection)
        {
            var coll = this.GetType().GetProperty(collection);
            if(coll == null)
            {
                coll = _rowsSettings.GetType().GetProperty(collection);
            }
            if (coll != null)
            {
                switch (collection)
                {
                    case nameof(Faculties):
                        {
                            if (!string.IsNullOrWhiteSpace(FacultieAddText))
                            {
                                Faculties.Add(FacultieAddText);
                                _rowsSettings.Faculties.Add(FacultieAddText);
                                FacultieAddText = "";
                            }
                            break;
                        }
                    case nameof(Departments):
                        {
                            if (!string.IsNullOrWhiteSpace(DepartmentOrInstitudeAddText))
                            {
                                Departments.Add(DepartmentOrInstitudeAddText);
                                _rowsSettings.DepartmentsOrInstitutes.Add(DepartmentOrInstitudeAddText);
                                DepartmentOrInstitudeAddText = "";
                            }
                            break;
                        }
                    case nameof(Frames):
                        {
                            if (!string.IsNullOrWhiteSpace(FrameAddText) && int.TryParse(FrameAddText,out int _))
                            {
                                Frames.Add(FrameAddText);
                                _rowsSettings.Frames.Add(FrameAddText);
                                FrameAddText = "";
                            }
                            break;
                        }
                    case nameof(RamTypes):
                        {
                            if (!string.IsNullOrWhiteSpace(RamTypeAddText))
                            {
                                RamTypes.Add(RamTypeAddText);
                                _rowsSettings.RamTypes.Add(RamTypeAddText);
                                RamTypeAddText = "";
                            }
                            break;
                        }
                    case nameof(Statuses):
                        {
                            if (!string.IsNullOrWhiteSpace(StatusAddText))
                            {
                                Statuses.Add(StatusAddText);
                                _rowsSettings.Statuses.Add(StatusAddText);
                                StatusAddText = "";
                            }       
                            break;
                        }
                    case nameof(_rowsSettings.OperatingSystemType):
                        {
                            if (!string.IsNullOrWhiteSpace(Data.OperatingSystem.Name))
                            {
                                _rowsSettings.OperatingSystemType.Add(Data.OperatingSystem.Name);
                                
                            }
                            break;
                        }
                    case nameof(_rowsSettings.Responsible):
                        {
                            if (!string.IsNullOrWhiteSpace(Data.ResponsiblePerson.Name))
                            {
                                _rowsSettings.Responsible.Add(Data.ResponsiblePerson.Name);
                            }
                            break;
                        }
                }
                await _rowsSettings.SaveSettings(_rowsSettings);
            }
        }
    }
}
