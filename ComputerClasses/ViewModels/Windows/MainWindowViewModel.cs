using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Navigation;
using ComputerClasses.ViewModels.Notifications;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        Navigator<PageBaseViewModel> _navigatorPage;
        [ObservableProperty]
        Navigator<PopupBaseViewModel> _navigatorPopup;
        [ObservableProperty]
        private NavigationMenuViewModel _navigationMenu;
        [ObservableProperty]
        private NotificationsViewModel _notificationViewModel;

        public MainWindowViewModel(Navigator<PageBaseViewModel> navigatorPage,
            Navigator<PopupBaseViewModel> navigatorPopup,
            NavigationMenuViewModel navigationMenu,
            NotificationsViewModel notificationViewModel)
        {
            NavigatorPage = navigatorPage;
            NavigatorPopup = navigatorPopup;
            NavigationMenu = navigationMenu;
            NotificationViewModel = notificationViewModel;
        }

    }
}
