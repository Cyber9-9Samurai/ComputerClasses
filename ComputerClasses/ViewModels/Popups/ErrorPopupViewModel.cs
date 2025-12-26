using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.ViewModels.Popups
{
    public partial class ErrorPopupViewModel : PopupBaseViewModel
    {
        private readonly Navigator<PopupBaseViewModel> _navigator;
        [ObservableProperty]
        private string errorMessage = "Ошибка.Что - то пошло не так!";
        public ErrorPopupViewModel(Navigator<PopupBaseViewModel> navigator)
        {
            _navigator = navigator;
        }

        public void SetDescription(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage += "\n" + errorMessage;
            }
        }

        [RelayCommand]
        private void Close()
        {
            _navigator.Navigate<EmptyPopupViewModel>();
            _navigator.BackStack.Clear();
        }
    }
}
