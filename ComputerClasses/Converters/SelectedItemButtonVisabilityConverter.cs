using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    internal class SelectedItemButtonVisabilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string current && values[1] is string selected)
            {
                
                if (string.Equals(current, selected, StringComparison.OrdinalIgnoreCase))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else if (values[1] is null)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
