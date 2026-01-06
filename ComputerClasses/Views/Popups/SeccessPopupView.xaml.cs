using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Popups
{
    /// <summary>
    /// Логика взаимодействия для SeccessPopupView.xaml
    /// </summary>
    [ViewFor<SuccessPopupViewModel>]
    public partial class SeccessPopupView : UserControl
    {
        public SeccessPopupView()
        {
            InitializeComponent();
        }
    }
}
