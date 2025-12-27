using ComputerClasses.Services.Data;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ComputerClasses.Converters
{
    public class PopupButtonCancelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataChangesActions actions)
            {
                return actions switch 
                {
                    DataChangesActions.Add => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5858")),
                    DataChangesActions.Edit => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5858")),
                    DataChangesActions.Remove => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#69A5FF")),
                    _ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5858"))
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
