using System;
using System.Globalization;
using System.Windows.Data;

namespace Timer.Converters
{
    public class DateTimeShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset date)
            {
                return date.ToString("yyyy.MM.dd");
            }

            return "No data";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
