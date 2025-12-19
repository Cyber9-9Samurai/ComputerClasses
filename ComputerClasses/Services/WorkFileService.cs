using CommunityToolkit.Mvvm.ComponentModel;
using ComputerClasses.Domain;
using ComputerClasses.Domain.Import;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ComputerClasses.Services
{
    public partial class WorkFileService : ObservableObject
    {
        private FileInfo _currentFile;
        private readonly ExcelRowImporter _excelRowImporter;
        [ObservableProperty]
        private ImportResult<Row> importData = new();
        public readonly string fileChanged;
        public WorkFileService(ExcelRowImporter  excelRowImporter)
        { 
            _excelRowImporter = excelRowImporter;
            fileChanged = nameof(_currentFile);
        }

        public FileInfo GetCurrentWorkFile()
        {
            return _currentFile;
        }

        private void SetCurrentWorkFile(FileInfo file)
        { 
            _currentFile = file;
            OnPropertyChanged(fileChanged);
        }

        public void StartImport(string path,FileInfo file)
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
                var pathDir = Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),"Data"));
                var destPath = Path.Combine(pathDir.FullName, file.Name);
                File.Move(path,destPath);
                file = new FileInfo(destPath);
                Debug.WriteLine(file.FullName);
                SetCurrentWorkFile(file);
            }
        }

    }
}
