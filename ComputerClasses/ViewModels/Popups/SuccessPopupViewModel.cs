using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class SuccessPopupViewModel : PopupBaseViewModel
    {
        private readonly Navigator<PopupBaseViewModel> _navigatorPopup;
        [ObservableProperty]
        private string description = "Операция прошла успешно!";
        public SuccessPopupViewModel(Navigator<PopupBaseViewModel> navigator)
        {
            _navigatorPopup = navigator;
        }

        public void SetDescription(string description)
        {
            Description += "\n" + description;
        }

        [RelayCommand]
        private void Close()
        {
            _navigatorPopup.Navigate<EmptyPopupViewModel>();
            _navigatorPopup.BackStack.Clear();
        }
    }
}
