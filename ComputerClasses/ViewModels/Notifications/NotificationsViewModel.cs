using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Notifications;
using ComputerClasses.ViewModels.Pages;
using System.Collections.ObjectModel;

namespace ComputerClasses.ViewModels.Notifications
{
    public partial class NotificationsViewModel : ObservableObject
    {
        private readonly NotificationsService _notificationsService;
        private readonly WorkFileService _workFileService;
        private readonly MainPageViewModel _mainPageViewModel;
        private int _notHideCount = 0;

        [ObservableProperty]
        private ObservableCollection<NotificationItem> notifications = new();
        [ObservableProperty]
        private bool isOpen;
        [ObservableProperty]
        private bool hasNotifications;
        [ObservableProperty]
        private MenuButtonItem notificationButton;
        public NotificationsViewModel(NotificationsService notificationsService,
            WorkFileService workFileService,
            MainPageViewModel mainPageViewModel,
            GetLocalImage getLocalImageService)
        {
            _notificationsService = notificationsService;
            _workFileService = workFileService;
            _mainPageViewModel = mainPageViewModel;
            _workFileService.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == _workFileService.fileChanged)
                {
                    await LoadData();
                }
            };
            NotificationButton = new MenuButtonItem("",getLocalImageService.GetImage("Notifications.gif"), ChangeNotificationsVisabilityCommand);
        }

        private async Task LoadData()
        {
            Notifications.Clear();
            var notif = await _notificationsService.NotificationsServiceStart(_workFileService.ImportData.Items.ToList());
            if (notif != null && notif?.Count > 0)
            {
                HasNotifications = true;
                _notHideCount = notif.Count;
                foreach (var item in notif)
                {
                    Notifications.Add(new NotificationItem(item, true));
                }
            }
            else
            {
                HasNotifications = false;
                _notHideCount = 0;
            }
                _mainPageViewModel.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(MainPageViewModel.Rows))
                    {
                        await LoadData();
                    }

                };

        }

        [RelayCommand]
        private void HideNotification(NotificationItem notification)
        {
            notification.IsVisible = false;
            _notHideCount--;
            if (_notHideCount == 0)
            {
                HasNotifications = false;
            }
        }

        [RelayCommand]
        private void ChangeNotificationsVisability()
        {
            IsOpen = !IsOpen;
        }
    }
}
