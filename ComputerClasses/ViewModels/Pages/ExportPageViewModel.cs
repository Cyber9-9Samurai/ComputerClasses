using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Models;
using ComputerClasses.ViewModels.Abstractions;

namespace ComputerClasses.ViewModels.Pages
{
    public partial class ExportPageViewModel : PageBaseViewModel
    {
        [ObservableProperty]
        private List<string> exportVar = new();
        [ObservableProperty]
        private string selectedExportVar;

        public ExportPageViewModel()
        {
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

        [RelayCommand]
        private void Export()
        {
            switch (SelectedExportVar)
            {
                case ExportVariants.ThisFile:
                    {

                        break;
                    }
                case ExportVariants.ExelXLS:
                    {
                        break;
                    }
                case ExportVariants.ExelXLSX:
                    {
                        break;
                    }
                case ExportVariants.Csv:
                    {
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
