using ComputerClasses.Services.Data;
using System.Globalization;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    public class PopupIsEditableOrEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DataChangesActions action)
            {
                return action switch
                {
                    DataChangesActions.Add => true,
                    DataChangesActions.Remove => false,
                    DataChangesActions.Edit => true,
                    _ => false
                };
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
