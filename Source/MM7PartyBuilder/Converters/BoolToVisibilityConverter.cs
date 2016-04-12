using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MM7ClassCreatorWPF.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility True { get; set; }
        public Visibility False { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var convertedValue = (bool)value;
            if (convertedValue)
                return True;
            else
                return False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
