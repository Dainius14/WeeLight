using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WeeLight.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Visibility))
            {
                throw new ArgumentOutOfRangeException(nameof(targetType));
            }

            bool isVisible = (bool)value;
            bool invert;
            bool.TryParse((string)parameter, out invert);

            return !(isVisible && invert) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
