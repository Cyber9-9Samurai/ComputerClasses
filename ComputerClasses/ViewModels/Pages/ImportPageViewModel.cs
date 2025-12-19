using CommunityToolkit.Mvvm.Input;
using ComputerClasses.ViewModels.Abstractions;
using System.IO;
using System.Windows;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ImportPageViewModel : PageBaseViewModel
    {
        [RelayCommand]
        private void Import(DragEventArgs args)
        {

            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var file = (string[])args.Data.GetData(DataFormats.FileDrop);
                if (file != null || file?.Count() > 0)
                {
                    var filepath = file.FirstOrDefault(f => Path.GetExtension(f) == ".xlsx" || Path.GetExtension(f) == ".xls");
                    if (filepath == null)
                    {
                        MessageBox.Show("Файл должен быть следующего формата: .xlsx|.xls");
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(Path.GetFileName(filepath));
                        MessageBox.Show(fileInfo.Name);
                    }

                }
            }
        }
    }


}
