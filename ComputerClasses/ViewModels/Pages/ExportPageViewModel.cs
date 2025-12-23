using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Test_Import_and_Export.Export;

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

        public ExportPageViewModel(WorkFileService workFileService,ExcelRowExporter excelRowExporter,CsvRowExporter csvRowExporter)
        {
            _workFileService = workFileService;
            _excelRowExporter = excelRowExporter;
            _csvRowExporter = csvRowExporter;
            LoadData();
            SelectedExportVar = ExportVar.FirstOrDefault() ?? string.Empty;
        }

        private void LoadData()
        {
            var variants = typeof(ExportVariants).GetFields();
            foreach (var field in variants)
            {
                if (field.GetValue(null) is object value && value is not null)
                {
                    ExportVar.Add(value.ToString()!);
                }
            }
        }

        private bool ExportFile(string filter,string defaultExtesion, out string path)
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
        private void Export()
        {
            var file = _workFileService.GetCurrentWorkFile();
            var data = _workFileService.ImportData.Items ?? new List<Domain.Row>();
            switch (SelectedExportVar)
            {
                case ExportVariants.ThisFile:
                    {
                        if(file is null)
                        {
                            MessageBox.Show("Текущий файл не выбран. Выберите другой вариант экспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            _excelRowExporter.ExportToXlsx(data, File.OpenWrite(file));
                        }                        
                        break;
                    }
                case ExportVariants.ExelXLS:
                    {
                        var result = ExportFile("Excel Files (*.xls)|*.xls", "xls",out string path);
                        if (result == true)
                        {
                            using var stream = File.OpenWrite(path);
                            _excelRowExporter.ExportToXlsx(data, stream);
                        }
                        break;
                    }
                case ExportVariants.ExelXLSX:
                    {
                        var result = ExportFile("Excel Files (*.xlsx)|*.xlsx", "xlsx",out string path);
                        if (result == true)
                        {
                            using var stream = File.OpenWrite(path);
                            _excelRowExporter.ExportToXlsx(data, stream);
                        }
                        break;
                    }
                case ExportVariants.Csv:
                    {
                        var result = ExportFile("CSV Files (*.csv)|*.csv", "csv",out string path);
                        if (result == true)
                        {
                            using var stream = File.OpenWrite(path);
                            _csvRowExporter.ExportToCsv(data, stream);
                        }
                        break;
                    }
                case ExportVariants.PDF:
                    {
                        break;
                    }
            }
        }
       }
    }
