using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Domain.Entities;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class ChangeDataPopupViewModel : PopupBaseViewModel
    {
        private DataChangesActions _actions;
        [ObservableProperty]
        private Row data;
        private int lastId = 0;

        private readonly WorkFileService _workFileService;
        private readonly Navigator<PopupBaseViewModel> _navigatorPopup;
        private readonly Navigator<PageBaseViewModel> _navigatorPage;
        public ChangeDataPopupViewModel(WorkFileService workFileService, Navigator<PopupBaseViewModel> navigatorPopup,Navigator<PageBaseViewModel> navigatorPage)
        {
            _workFileService = workFileService;
            _navigatorPopup = navigatorPopup;
            _navigatorPage = navigatorPage;
            lastId = _workFileService.ImportData.Items.Count > 0 ? _workFileService.ImportData.Items.Max(i => i.Id) + 1 : 1;
        }

        public void GetData(DataChangesActions actions, Row? row)
        {
            _actions = actions;
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
                    Id =row.Id,
                    Facultie = new Facultie() { Id = row.Facultie.Id, Name = row.Facultie.Name },
                    DepartmentOrInstitute = new DepartmentOrInstitute() { Id = row.DepartmentOrInstitute.Id,Name = row.DepartmentOrInstitute.Name},
                    Frame = new Domain.Entities.Frame() { Id = row.Frame.Id, Name = row.Frame.Name },
                    AudienceNumber = new AudienceNumber() { Id = row.AudienceNumber.Id, Name = row.AudienceNumber.Name},
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
        }

        [RelayCommand]
        public void Close()
        {
            _navigatorPopup.BackStack.Clear();
            _navigatorPopup.Navigate<EmptyPopupViewModel>();
            _navigatorPage.BackStack.Clear();
            _navigatorPage.Navigate<MainPageViewModel>().LoadFile();
        }

        [RelayCommand]
        public void AgreeChanges()
        {
            switch (_actions)
            {
                case DataChangesActions.Add:
                    {
                        _workFileService.ImportData.Items.Add(Data);
                        break;
                    }
                case DataChangesActions.Edit:
                    {
                        var item = _workFileService.ImportData.Items.FirstOrDefault(i => i.Id == Data.Id);
                        if (item != null)
                        {
                            var index = _workFileService.ImportData.Items.IndexOf(item);
                            _workFileService.ImportData.Items[index] = Data;
                        }
                        break;
                    }
                case DataChangesActions.Remove:
                    {
                        var item = _workFileService.ImportData.Items.FirstOrDefault(i => i.Id == Data.Id);
                        if (item != null)
                        {
                            _workFileService.ImportData.Items.Remove(item);
                        }
                        break;
                    }
            }
            Close();
        }
    }
}
