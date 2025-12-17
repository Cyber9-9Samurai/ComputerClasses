using ComputerClasses.ViewModels.Pages;
using Mvvm.Navigation;
using System.Windows.Controls;

namespace ComputerClasses.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangesMagazineView.xaml
    /// </summary>
    [ViewFor<ChangesMagazineViewModel>]
    public partial class ChangesMagazineView : UserControl
    {
        public ChangesMagazineView()
        {
            InitializeComponent();
        }
    }
}
