using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ComputerClasses.Models
{
    public partial class NavigationItem : ObservableObject
    {
        [ObservableProperty]
        private string title;
        [ObservableProperty]
        private BitmapImage image;
        [ObservableProperty]
        private ICommand navCommand;

        public NavigationItem(string title, BitmapImage image, ICommand navCommand)
        {
            Title = title;
            Image = image;
            NavCommand = navCommand;
        }
    }
}
