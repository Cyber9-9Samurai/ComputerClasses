using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Services;
using ComputerClasses.ViewModels.Abstractions;
using System.IO;
using System.Windows;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ImportPageViewModel : PageBaseViewModel
    {
        private readonly WorkFileService _fileService;
        public ImportPageViewModel(WorkFileService fileService)
        {
            _fileService = fileService;
        }
        [RelayCommand]
        private void Import(DragEventArgs args)
        {

            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var file = (string[])args.Data.GetData(DataFormats.FileDrop);
                if (file != null || file?.Count() > 0)
                {
                    var filePath = file.FirstOrDefault(f => Path.GetExtension(f) == ".xlsx" || Path.GetExtension(f) == ".xls");
                    if (filePath == null)
                    {
                        MessageBox.Show("Файл должен быть следующего формата: .xlsx|.xls");
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(Path.GetFileName(filePath));
                        _fileService.StartImport(filePath,fileInfo);
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
                FileInfo fileInfo = new FileInfo(Path.GetFileName(openFileDialog.FileName));
                MessageBox.Show(openFileDialog.FileName + ' ' + fileInfo.FullName);
                _fileService.StartImport(openFileDialog.FileName, fileInfo);
            }
        }

    }


}
