using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReVersion.Utilities.Converters
{
    public class ReverseVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = bool.Parse(value?.ToString()??"false");
            return booleanValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
