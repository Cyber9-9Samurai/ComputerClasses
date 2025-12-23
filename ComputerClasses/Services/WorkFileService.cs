using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.Domain;
using ComputerClasses.Domain.Import;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Test_Import_and_Export.Export;

namespace ComputerClasses.Services
{
    public partial class WorkFileService : ObservableObject
    {
        private string _currentFile;
        private readonly ExcelRowImporter _excelRowImporter;
        private readonly ExcelRowExporter _excelRowExporter;
        [ObservableProperty]
        private ImportResult<Row> importData = new();
        public readonly string fileChanged;
        public WorkFileService(ExcelRowImporter excelRowImporter,ExcelRowExporter excelRowExporter)
        {
            _excelRowImporter = excelRowImporter;
            _excelRowExporter = excelRowExporter;
            fileChanged = nameof(_currentFile);
        }

        public string GetCurrentWorkFile()
        {
            return _currentFile;
        }

        private void SetCurrentWorkFile(string path)
        {
            _currentFile = path;
            OnPropertyChanged(fileChanged);
        }

        public void StartImport(string path)
        {
            ImportData = _excelRowImporter.Import(File.OpenRead(path));
            if (ImportData.HasErrors)
            {
                string text = "";
                foreach (var error in ImportData.Errors)
                {
                    text += "/n" + error.Message;
                }
                MessageBox.Show(text);
            }
            else
            {
                SetCurrentWorkFile(path);
            }
        }

        public void CreateNewWorkFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файлы Exel(*.xlsx)|*.xlsx";
            var result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                _excelRowExporter.ExportToXlsx(new List<Row>(),File.OpenWrite(saveFileDialog.FileName));
                StartImport(saveFileDialog.FileName);
            }
        }

        public bool HasFile()
        {
            return File.Exists(_currentFile);
        }

    }
}
