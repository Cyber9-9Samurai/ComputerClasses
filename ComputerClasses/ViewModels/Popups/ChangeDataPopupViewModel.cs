using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.ViewModels.Abstractions;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class ChangeDataPopupViewModel : PopupBaseViewModel
    {
        private DataChangesActions _actions;
        [ObservableProperty]
        private Row data;

        private readonly WorkFileService _workFileService;
        public ChangeDataPopupViewModel(WorkFileService workFileService)
        {
            _workFileService = workFileService;
        }

        public void GetData(DataChangesActions actions, Row? row)
        {
            _actions = actions;
            Data = row ?? new Row();
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
        }
    }
}
