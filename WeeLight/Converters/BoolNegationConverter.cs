using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WeeLight.Converters
{
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType != typeof(Boolean) && targetType != typeof(Visibility))
            {
                throw new ArgumentOutOfRangeException(nameof(targetType));
            }

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
