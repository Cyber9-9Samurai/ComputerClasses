using System.Windows.Controls;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;

namespace ComputerClasses.Views.Popups
{
    /// <summary>
    /// Логика взаимодействия для TestPopup.xaml
    /// </summary>
    [ViewFor<TestPopupViewModel>]
    public partial class TestPopup : UserControl
    {
        public TestPopup()
        {
            InitializeComponent();
        }
    }
}
