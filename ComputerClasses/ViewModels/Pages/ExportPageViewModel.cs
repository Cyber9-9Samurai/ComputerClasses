using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.DAL.Excel.Export;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Logs;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Microsoft.Win32;
using Mvvm.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Test_Import_and_Export.Export;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ExportPageViewModel : PageBaseViewModel
    {
        [ObservableProperty]
        private List<string> exportVar = new();
        [ObservableProperty]
        private string selectedExportVar;
        private readonly WorkFileService _workFileService;
        private readonly ExcelRowExporter _excelRowExporter;
        private readonly CsvRowExporter _csvRowExporter;
        private readonly PdfRowExporter _pdfRowExporter;
        private readonly Navigator<PopupBaseViewModel> _navigator;
        private readonly LogService _logService;
        private readonly ChangesMagazineViewModel _changesMagazineViewModel;
        private readonly GetLocalImage _getLocalImage;

        [ObservableProperty]
        private MenuButtonItem exportButton;

        public ExportPageViewModel(WorkFileService workFileService,
            ExcelRowExporter excelRowExporter,
            CsvRowExporter csvRowExporter,
            PdfRowExporter pdfRowExporter,
            Navigator<PopupBaseViewModel> navigator,
            LogService logService,
            ChangesMagazineViewModel changesMagazineViewModel,
            GetLocalImage getLocalImage)
        {
            _changesMagazineViewModel = changesMagazineViewModel;
            _workFileService = workFileService;
            _excelRowExporter = excelRowExporter;
            _csvRowExporter = csvRowExporter;
            _pdfRowExporter = pdfRowExporter;
            _navigator = navigator;
            _logService = logService;
            _getLocalImage = getLocalImage;
            LoadData();
            SelectedExportVar = ExportVar.FirstOrDefault() ?? string.Empty;
        }

        private void LoadData()
        {
            ExportButton = new MenuButtonItem("",_getLocalImage.GetImage("Export.png"),ExportCommand,null);
            var variants = typeof(ExportVariants).GetFields();
            foreach (var field in variants)
            {
                if (field.GetValue(null) is object value && value is not null)
                {
                    ExportVar.Add(value.ToString()!);
                }
            }
        }

        private bool ExportFile(string filter, string defaultExtesion, out string path)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExtesion
            };
            bool result = saveFileDialog.ShowDialog() ?? false;
            if (result)
            {
                path = saveFileDialog.FileName;
            }
            else
            {
                path = string.Empty;
            }
            return result;
        }


        //метод для экспорта данных в классе логики страницы экспорта
        [RelayCommand]
        private async Task Export()
        {
            //получаем информацию о текущем файле из соответсвующего сервиса
            var file = _workFileService.GetCurrentWorkFile();
            //получаем данные для экспрота из сервиса , если пустой то новую коллекцию
            var data = _workFileService.ImportData.Items ?? new ObservableCollection<Domain.Row>();
            string globPath = "";
            //проверка пустая ли информация о текущем файле
            if (file is null)
            {
                //открываем модальное окно для вывода ошибок и передаем сообщение
                _navigator.Navigate<ErrorPopupViewModel>()
                .SetDescription("Невозможно экспортировать файл, так как он не выбран.");
                return;
            }
            //пытаемся экспротировать
            try
            {
                //проверяем какой вариант экспорта выбрал пользователь
                switch (SelectedExportVar)
                {
                    //если в этот же файл
                    case ExportVariants.ThisFile:
                        {
                            //с помощью сервиса экспорта сохраняем данные в этот же файл
                            globPath = file;
                            _excelRowExporter.ExportToXlsx(data, File.OpenWrite(file));
                            _navigator.Navigate<SuccessPopupViewModel>().SetDescription("Файл был экспортирован!");
                            break;
                        }
                    //если в новый .xlsx файл
                    case ExportVariants.ExelXLSX:
                        {
                            //открываем окно выбора пути для сохранения 
                            //и запоминаем результат
                            var result = ExportFile("Excel Files (*.xlsx)|*.xlsx", "xlsx", out string path);
                            //проверка если пользователь выбрал путь
                            if (result == true)
                            {
                                //с помощью сервиса экспорта 
                                //сохраняем данные в .xlsx файл
                                globPath = path;
                                using var stream = File.OpenWrite(path);
                                _excelRowExporter.ExportToXlsx(data, stream);
                                _navigator.Navigate<SuccessPopupViewModel>().SetDescription("Файл был экспортирован!");
                            }
                            break;
                        }
                    //если в csv файл
                    case ExportVariants.Csv:
                        {
                            //открываем окно выбора пути для сохранения 
                            //и запоминаем результат
                            var result = ExportFile("CSV Files (*.csv)|*.csv", "csv", out string path);
                            //проверка если пользователь выбрал путь
                            if (result == true)
                            {
                                //с помощью сервиса экспорта 
                                //сохраняем данные .csv файл
                                using var stream = File.OpenWrite(path);
                                _csvRowExporter.ExportToCsv(data, stream);
                                _navigator.Navigate<SuccessPopupViewModel>().SetDescription("Файл был экспортирован!");
                            }
                            break;
                        }
                    //если в pdf файл
                    case ExportVariants.PDF:
                        {
                            //открываем окно выбора пути для сохранения 
                            //и запоминаем результат
                            var result = ExportFile("PDF Files (*.pdf)|*.pdf", "pdf", out string path);
                            //проверка если пользователь выбрал путь
                            if (result == true)
                            {
                                //с помощью сервиса экспорта 
                                //сохраняем данные в .pdf
                                using var stream = File.OpenWrite(path);
                                _pdfRowExporter.ExportToPdf(data, stream);
                                _navigator.Navigate<SuccessPopupViewModel>().SetDescription("Файл был экспортирован!");
                            }
                            break;
                        }
                }
                if (!string.IsNullOrWhiteSpace(globPath))
                {
                    //вычисляем хэш экспортированного файла
                    var hashFile = await _logService.HashFile(File.OpenRead(globPath));
                    //сохраняем логи для этого файла по хэш-ключу
                    await _logService.SaveLog(hashFile, _changesMagazineViewModel.Logs.ToList());
                }
            }
            //если возникло исключение
            catch (Exception ex)
            {
                //открываем коно для вывода сообщений об ошибках
                //и передаем текст сообщения
                _navigator.Navigate<ErrorPopupViewModel>()
                .SetDescription("Не удалось экспортировать файл по неизвестным причинам. Может быть файл занят другой программой. Попробуйте снова позднее.");
                }
            }

    }
}
