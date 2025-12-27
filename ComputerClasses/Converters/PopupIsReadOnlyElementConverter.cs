using ComputerClasses.Services.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    public class PopupIsReadOnlyElementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DataChangesActions actions)
            {
                return actions switch
                {
                    DataChangesActions.Add => false,
                    DataChangesActions.Edit => false,
                    DataChangesActions.Remove => true,
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
