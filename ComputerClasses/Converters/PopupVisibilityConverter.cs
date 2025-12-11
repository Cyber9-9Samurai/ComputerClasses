using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;

namespace ComputerClasses.Converters
{
    public class PopupVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || value?.GetType() == typeof(EmptyPopupViewModel))
            {
                return Visibility.Collapsed;
            }
            else if (value is PopupBaseViewModel popup)
            {
                if (popup == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
