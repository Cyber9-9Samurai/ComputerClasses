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


        [RelayCommand]
        private async Task Export()
        {
            var file = _workFileService.GetCurrentWorkFile();
            var data = _workFileService.ImportData.Items ?? new ObservableCollection<Domain.Row>();
            if (file is null)
            {
                _navigator.Navigate<ErrorPopupViewModel>().SetDescription("Невозможно экспортировать файл, так как он не выбран.");
                return;
            }
            try
            {
                switch (SelectedExportVar)
                {
                    case ExportVariants.ThisFile:
                        {
                            _excelRowExporter.ExportToXlsx(data, File.OpenWrite(file));
                            break;
                        }
                    case ExportVariants.ExelXLS:
                        {
                            var result = ExportFile("Excel Files (*.xls)|*.xls", "xls", out string path);
                            if (result == true)
                            {
                                using var stream = File.OpenWrite(path);
                                _excelRowExporter.ExportToXlsx(data, stream);
                            }
                            break;
                        }
                    case ExportVariants.ExelXLSX:
                        {
                            var result = ExportFile("Excel Files (*.xlsx)|*.xlsx", "xlsx", out string path);
                            if (result == true)
                            {
                                using var stream = File.OpenWrite(path);
                                _excelRowExporter.ExportToXlsx(data, stream);
                            }
                            break;
                        }
                    case ExportVariants.Csv:
                        {
                            var result = ExportFile("CSV Files (*.csv)|*.csv", "csv", out string path);
                            if (result == true)
                            {
                                using var stream = File.OpenWrite(path);
                                _csvRowExporter.ExportToCsv(data, stream);
                            }
                            break;
                        }
                    case ExportVariants.PDF:
                        {
                            var result = ExportFile("PDF Files (*.pdf)|*.pdf", "pdf", out string path);
                            if (result == true)
                            {
                                using var stream = File.OpenWrite(path);
                                _pdfRowExporter.ExportToPdf(data, stream);
                            }
                            break;
                        }
                }
                _navigator.Navigate<SuccessPopupViewModel>().SetDescription("Файл был экспортирован!");
                var hashFile = await _logService.HashFile(File.OpenRead(_workFileService.GetCurrentWorkFile()));
                await _logService.SaveLog(hashFile , _changesMagazineViewModel.Logs.ToList());
            }
            catch (Exception ex)
            {
                _navigator.Navigate<ErrorPopupViewModel>().SetDescription("Не удалось экспортировать файл по неизвестным причинам. Может быть файл занят другой программой. Попробуйте снова позднее.");
                }
            }

    }
}
