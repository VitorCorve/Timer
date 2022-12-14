using System;
using System.Globalization;
using System.Windows.Data;

namespace Timer.Converters
{
    public class HourToDegreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int time && time <= 59)
            {
                return time * 30;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
