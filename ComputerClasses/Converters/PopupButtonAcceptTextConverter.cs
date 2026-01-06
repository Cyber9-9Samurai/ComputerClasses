using ComputerClasses.Services.Data;
using System.Globalization;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    public class PopupButtonAcceptTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataChangesActions action)
            {
                return action switch 
                {
                    DataChangesActions.Add => "Добавить",
                    DataChangesActions.Edit => "Изменить",
                    DataChangesActions.Remove => "Удалить",
                    _ => ""
                };
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
