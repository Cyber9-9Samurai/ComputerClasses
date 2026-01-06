using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using Mvvm.Navigation;
using System.IO.Packaging;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class SuccessPopupViewModel : PopupBaseViewModel
    {
        private readonly Navigator<PopupBaseViewModel> _navigatorPopup;
        private readonly string defaultMessage = "Операция прошла успешно!";
        [ObservableProperty]
        private string description;
        public SuccessPopupViewModel(Navigator<PopupBaseViewModel> navigator)
        {
            _navigatorPopup = navigator;
        }

        public void SetDescription(string description)
        {
            Description = defaultMessage + "\n" + description;
        }

        [RelayCommand]
        private void Close()
        {
            _navigatorPopup.Navigate<EmptyPopupViewModel>();
            _navigatorPopup.BackStack.Clear();
        }
    }
}
