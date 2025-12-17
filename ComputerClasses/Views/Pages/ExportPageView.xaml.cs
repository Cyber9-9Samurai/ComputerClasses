using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ExportPageView.xaml
    /// </summary>
    [ViewFor<ExportPageViewModel>]
    public partial class ExportPageView : UserControl
    {
        public ExportPageView()
        {
            InitializeComponent();
        }
    }
}
