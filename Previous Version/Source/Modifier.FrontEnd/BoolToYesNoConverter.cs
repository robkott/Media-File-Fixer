using System;
using System.Globalization;
using System.Windows.Data;

namespace Modifier.FrontEnd
{
    public class BoolToYesNoConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (value is bool?) == false)
            {
                return string.Empty;
            }

            var converted = (bool?) value;
            return converted.Value ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}