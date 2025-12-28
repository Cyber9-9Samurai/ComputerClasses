using ComputerClasses.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    public class EventArgsAndDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length != 2)
            {
                return Binding.DoNothing;
            }
            else if (values[0] is MenuButtonItem buttonItem && values[1] is Image imageControl)
            {
                return (buttonItem,imageControl);
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
