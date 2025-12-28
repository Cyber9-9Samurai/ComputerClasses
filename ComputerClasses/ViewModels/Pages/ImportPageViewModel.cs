using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ImportPageViewModel : PageBaseViewModel
    {
        private readonly WorkFileService _fileService;
        private readonly Navigator<PageBaseViewModel> _navigator;
        private readonly Navigator<PopupBaseViewModel> _navigatorPopup;
        private readonly SessionService _session;
        private readonly GetLocalImage _getLocalImage;
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private MenuButtonItem importButton;
        public ImportPageViewModel(WorkFileService fileService, 
            Navigator<PageBaseViewModel> navigator,
            Navigator<PopupBaseViewModel> navigatorPopup,
            SessionService session,
            GetLocalImage getLocalImage)
        {
            _fileService = fileService;
            _navigator = navigator;
            _navigatorPopup = navigatorPopup;
            _session = session;
            _getLocalImage = getLocalImage;
            ImportButton = new MenuButtonItem("", _getLocalImage.GetImage("Import.gif"), OpenFileDialogCommand, null);
        }
        [RelayCommand]
        private void Import(DragEventArgs args)
        {
            
            if (string.IsNullOrWhiteSpace(UserName))
            {
                _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Введите имя пользователя!");
                return;
            }
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var file = (string[])args.Data.GetData(DataFormats.FileDrop);
                if (file != null || file?.Count() > 0)
                {
                    var filePath = file.FirstOrDefault(f => Path.GetExtension(f) == ".xlsx" || Path.GetExtension(f) == ".xls");
                    if (filePath == null)
                    {
                        _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Файл должен быть следующего формата: .xlsx|.xls");
                    }
                    else
                    {
                        _fileService.StartImport(filePath);
                        _navigator.Navigate<MainPageViewModel>();
                        _session.ClearSession();
                        _session.SetUser(UserName);
                    }

                }
            }
        }

        [RelayCommand]
        private void OpenFileDialog()
        {

            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Введите имя пользователя!");
                    return;
                }
                try
                {
                    _fileService.StartImport(openFileDialog.FileName);
                    _navigator.Navigate<MainPageViewModel>();
                    _session.ClearSession();
                    _session.SetUser(UserName);
                }
                catch (Exception ex)
                {
                    _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Ошибка при импорте!");
                }
                
            }

        }

        [RelayCommand]
        private async Task LoadImage(RoutedEventArgs args)
        {
            if (args is not null && args.Source is Image imageControl)
            {
                await Task.Delay(100);
                ImportButton.Animator = ImageBehavior.GetAnimationController(imageControl);
            }
        }
        [RelayCommand]
        private void Play(MenuButtonItem buttonItem)
        {
            buttonItem.Animator?.Play();
            if (buttonItem.Animator != null && !buttonItem.IsSubscribe)
            {
                buttonItem.Animator.CurrentFrameChanged += (s, e) =>
                {
                    if (buttonItem.Animator.CurrentFrame == buttonItem.Animator.FrameCount - 1)
                    {
                        buttonItem.Animator.Pause();
                        buttonItem.Animator.GotoFrame(0);
                    }
                };
            }
        }

    }


}
