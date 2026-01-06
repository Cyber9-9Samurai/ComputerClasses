using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Popups
{
    /// <summary>
    /// Логика взаимодействия для ChangeDataPopupView.xaml
    /// </summary>
    [ViewFor<ChangeDataPopupViewModel>]
    public partial class ChangeDataPopupView : UserControl
    {
        public ChangeDataPopupView()
        {
            InitializeComponent();
        }
    }
}
