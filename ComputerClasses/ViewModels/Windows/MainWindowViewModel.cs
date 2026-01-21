using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Navigation;
using ComputerClasses.ViewModels.Notifications;
using Mvvm.Navigation;

namespace ComputerClasses.ViewModels.Windows
{
    //логика работы главного окна
    public partial class MainWindowViewModel : ObservableObject
    {
        //отслеживаемое поле кторое хранит ссылку на навигатор для страниц
        [ObservableProperty]
        Navigator<PageBaseViewModel> _navigatorPage;
        //отслеживаемое поле кторое хранит ссылку на навигатор для модальных окон
        [ObservableProperty]
        Navigator<PopupBaseViewModel> _navigatorPopup;
        //отслеживаемое поле кторое хранит ссылку на ViewModel навигационного меню
        [ObservableProperty]
        private NavigationMenuViewModel _navigationMenu;
        [ObservableProperty]
        //отслеживаемое поле кторое хранит ссылку на ViewModel меню уведомлений
        private NotificationsViewModel _notificationViewModel;

        //получение ссылок на объекты всех зависимостей через конструктор с помощью DI
        public MainWindowViewModel(Navigator<PageBaseViewModel> navigatorPage,
            Navigator<PopupBaseViewModel> navigatorPopup,
            NavigationMenuViewModel navigationMenu,
            NotificationsViewModel notificationViewModel)
        {
            //передача ссылок
            NavigatorPage = navigatorPage;
            NavigatorPopup = navigatorPopup;
            NavigationMenu = navigationMenu;
            NotificationViewModel = notificationViewModel;
        }

    }
}
