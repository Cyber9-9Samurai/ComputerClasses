using CommunityToolkit.Mvvm.ComponentModel;

namespace ComputerClasses.Models
{
    public partial class NotificationItem : ObservableObject
    {
        [ObservableProperty]
        private string message;
        [ObservableProperty]
        private bool isVisible;

        public NotificationItem(string message, bool isVisible)
        {
            Message = message;
            IsVisible = isVisible;
        }
    }
}
