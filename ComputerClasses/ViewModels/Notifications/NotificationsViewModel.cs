using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Notifications;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Notifications
{
    public partial class NotificationsViewModel : ObservableObject
    {
        private readonly NotificationsService _notificationsService;
        private readonly WorkFileService _workFileService;
        private ImageAnimationController _animator;
        private int _notHideCount = 0;
        private bool _isSubscribe;

        [ObservableProperty]
        private ObservableCollection<NotificationItem> notifications = new();
        [ObservableProperty]
        private bool isOpen;
        [ObservableProperty]
        private bool hasNotifications;
        [ObservableProperty]
        private MenuButtonItem notificationButton;
        [ObservableProperty]
        private bool isPaused;
        
        public NotificationsViewModel(NotificationsService notificationsService,
            WorkFileService workFileService,
            GetLocalImage getLocalImageService)
        {
            _notificationsService = notificationsService;
            _workFileService = workFileService;
            NotificationButton = new MenuButtonItem("",getLocalImageService.GetImage("Notifications.gif"), ChangeNotificationsVisabilityCommand,null);
            _workFileService.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == _workFileService.fileChanged || e.PropertyName == nameof(_workFileService.ImportData.Items))
                {
                    await LoadData();
                }
            };
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
