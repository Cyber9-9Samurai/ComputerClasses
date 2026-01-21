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
            ImportButton = new MenuButtonItem("", _getLocalImage.GetImage("Import.png"), OpenFileDialogCommand, null);
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
                        _fileService.StartImport(filePath,out bool hasErrors,out string errorsText);
                        if (hasErrors)
                        {
                            _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription(errorsText);
                            return;
                        }
                        _navigator.Navigate<MainPageViewModel>();
                        _session.ClearSession();
                        _session.SetUser(UserName);
                    }

                }
            }
        }

        //метод для импорта данных в классе логики импорта
        [RelayCommand]
        private void OpenFileDialog()
        {
            //создаем диалоговое коно выбора файла
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //настраиваем фильтр для окна выбора файла
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            //открываем окно и запоминаем результат работы
            var result = openFileDialog.ShowDialog();
            //проверка если пользователь выбрал файл
            if (result == true)
            {
                //проверка если пользователь не ввел имя или логин
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    //открываем окно для отображения сообщений об ошибках
                    //и передаем сообщение ошибки
                    _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Введите имя пользователя!");
                    return;
                }
                //если пользователь ввел имя или логин,
                //то пробуем импортировать
                try
                {
                    //запускаем импорт с помошью сервиса хранения текущего файла
                    _fileService.StartImport(openFileDialog.FileName,out bool hasErrors,out string errorText);
                    if (hasErrors)
                    {
                        _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription(errorText);
                    }
                    //перенаправляем пользователя на главню страницу
                    _navigator.Navigate<MainPageViewModel>();
                    //отчищаем страую сессию и сохраняем новую
                    _session.ClearSession();
                    _session.SetUser(UserName);
                }
                //если возникло исключение
                catch (Exception)
                {
                    //открываем окно для отображения сообщений об ошибках
                    //и передаем сообщение ошибки
                    _navigatorPopup.Navigate<ErrorPopupViewModel>().SetDescription("Ошибка при импорте!");
                }
                
            }

        }


    }


}
