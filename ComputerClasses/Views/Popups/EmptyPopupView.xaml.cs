using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;

namespace ComputerClasses.Views.Popups
{
    /// <summary>
    /// Логика взаимодействия для EmptyPopupView.xaml
    /// </summary>
    [ViewFor<EmptyPopupViewModel>]
    public partial class EmptyPopupView : UserControl
    {
        public EmptyPopupView()
        {
            InitializeComponent();
        }
    }
}
