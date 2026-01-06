using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace ComputerClasses.Models
{
    public partial class MenuButtonItem : ObservableObject
    {
        [ObservableProperty]
        private string title;
        [ObservableProperty]
        private BitmapImage image;
        [ObservableProperty]
        private ICommand navCommand;
        [ObservableProperty]
        private ImageAnimationController? animator;
        [ObservableProperty]
        private bool isSubscribe;

        public MenuButtonItem(string title, BitmapImage image, ICommand navCommand, ImageAnimationController? controller)
        {
            Title = title;
            Image = image;
            NavCommand = navCommand;
            Animator = controller;
        }
    }
}
