using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReVersion.Utilities.Converters
{
    internal class VisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = bool.Parse(value?.ToString()??"false");
            return booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
