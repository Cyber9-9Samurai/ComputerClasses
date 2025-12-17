using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ImportPageView.xaml
    /// </summary>
    [ViewFor<ImportPageViewModel>]
    public partial class ImportPageView : UserControl
    {
        public ImportPageView()
        {
            InitializeComponent();
        }
    }
}
