using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComputerClasses.Converters
{
    public class RectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return Rect.Empty;

            if (values[0] is not double w || values[1] is not double h)
                return Rect.Empty;

            if (w <= 0 || h <= 0)
                return Rect.Empty;

            return new Rect(0, 0, w, h);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
