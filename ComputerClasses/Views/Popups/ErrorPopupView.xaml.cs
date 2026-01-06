using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Popups
{
    /// <summary>
    /// Логика взаимодействия для ErrorPopupView.xaml
    /// </summary>
    [ViewFor<ErrorPopupViewModel>]
    public partial class ErrorPopupView : UserControl
    {
        public ErrorPopupView()
        {
            InitializeComponent();
        }
    }
}
