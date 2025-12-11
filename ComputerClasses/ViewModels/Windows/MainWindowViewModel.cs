using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.ViewModels.Abstractions;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        Navigator<PageBaseViewModel> _navigatorPage;
        [ObservableProperty]
        Navigator<PopupBaseViewModel> _navigatorPopup;


        public MainWindowViewModel(Navigator<PageBaseViewModel> navigatorPage, Navigator<PopupBaseViewModel> navigatorPopup)
        {
            NavigatorPage = navigatorPage;
            NavigatorPopup = navigatorPopup;
        }

    }
}
