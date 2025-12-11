using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class TestPopupViewModel : PopupBaseViewModel
    {
        private readonly Navigator<PopupBaseViewModel> _navigator;
        public TestPopupViewModel(Navigator<PopupBaseViewModel> navigator)
        {
            _navigator = navigator;
        }

        [RelayCommand]
        private void ClosePopup()
        {
            _navigator.Navigate<EmptyPopupViewModel>();
        }
    }
}
