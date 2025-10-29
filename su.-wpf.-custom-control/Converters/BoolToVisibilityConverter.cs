using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Su.WPF.CustomControl.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        // 将bool转换为Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed; // 默认返回 Collapsed
        }

        // 将Visibility转换回bool
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false; // 默认返回 false
        }
    }
}
