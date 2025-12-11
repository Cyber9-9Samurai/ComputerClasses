using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class MainPageViewModel : PageBaseViewModel
    {
        [ObservableProperty]
        private string hi = "hi";

        private readonly Navigator<PopupBaseViewModel> _navigator;

        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator)
        {
            _navigator = navigator;
        }

        [RelayCommand]
        private void OpenPopup()
        {
            _navigator.Navigate<TestPopupViewModel>();
        }
    }
}
