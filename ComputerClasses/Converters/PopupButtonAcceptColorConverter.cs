using ComputerClasses.Services.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputerClasses.Converters
{
    public class PopupButtonAcceptColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataChangesActions actions)
            {
                return actions switch
                {
                    DataChangesActions.Add => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#69A5FF")),
                    DataChangesActions.Edit => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#69A5FF")),
                    DataChangesActions.Remove => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5858")),
                    _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#69A5FF"))
                };
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5858"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
