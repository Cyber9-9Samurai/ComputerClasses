using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Navigation
{
    public partial class NavigationMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<MenuButtonItem> navigationItems = new List<MenuButtonItem>();

        private readonly Navigator<PageBaseViewModel> _navigator;
        private readonly GetLocalImage _imageService;
        public NavigationMenuViewModel(Navigator<PageBaseViewModel> navigator, GetLocalImage imageService)
        {
            _navigator = navigator;
            _imageService = imageService;
            LoadData();
        }

        public void LoadData()
        {
            NavigationItems = new List<MenuButtonItem>
            {
                new MenuButtonItem("Главная",_imageService.GetImage("MainPage.gif"),ToMainCommand,null),
                new MenuButtonItem("Импорт",_imageService.GetImage("Import.gif"),ToImportCommand,null),
                new MenuButtonItem("Экспорт",_imageService.GetImage("Export.gif"), ToExportCommand,null),
                new MenuButtonItem("Журнал изменений", _imageService.GetImage("ChangesLog.gif"), ToChangesCommand,null)
            };

        }

        [RelayCommand]
        private void ToMain()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<MainPageViewModel>();
        }

        [RelayCommand]
        private void ToImport()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ImportPageViewModel>();
        }

        [RelayCommand]
        private void ToExport()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ExportPageViewModel>();
        }

        [RelayCommand]
        private void ToChanges()
        {
            _navigator.BackStack.Clear();
            _navigator.Navigate<ChangesMagazineViewModel>();
        }

        [RelayCommand]
        private async Task LoadImage(RoutedEventArgs args)
        {
            if (args is not null)
            {
                if (args.Source is Image imageControl)
                {
                    if (imageControl.DataContext is MenuButtonItem item)
                    {
                       item.Animator = ImageBehavior.GetAnimationController(imageControl);
                    }
                }
            }
        }
            
        [RelayCommand]
         private void Play(MenuButtonItem item)
         {
           item.Animator?.Play();
           if (item.Animator != null && !item.IsSubscribe)
           {
              item.IsSubscribe = true;
              item.Animator.CurrentFrameChanged += (s, e) =>
                  {
                if (item.Animator.CurrentFrame == item.Animator.FrameCount - 1)
                 {
                     item.Animator.Pause();
                     item.Animator.GotoFrame(0);
                 }
                  };
           }
         }

    }
}
