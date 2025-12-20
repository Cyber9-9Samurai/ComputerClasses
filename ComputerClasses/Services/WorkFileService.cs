using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.Domain;
using ComputerClasses.Domain.Import;
using System.IO;
using System.Windows;

namespace ComputerClasses.Services
{
    public partial class WorkFileService : ObservableObject
    {
        private string _currentFile;
        private readonly ExcelRowImporter _excelRowImporter;
        [ObservableProperty]
        private ImportResult<Row> importData = new();
        public readonly string fileChanged;
        public WorkFileService(ExcelRowImporter excelRowImporter)
        {
            _excelRowImporter = excelRowImporter;
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

    }
}
