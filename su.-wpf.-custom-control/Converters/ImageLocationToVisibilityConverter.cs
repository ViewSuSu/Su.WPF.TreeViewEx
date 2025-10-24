using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Su.WPF.CustomControl.TreeViewEx;

namespace Su.WPF.CustomControl.Converters
{
    internal class ImageLocationToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ImageSourceLocation currentLocation && parameter is string targetLocation)
            {
                // 将字符串参数转换为 ImageSourceLocation
                if (Enum.TryParse<ImageSourceLocation>(targetLocation, out var location))
                {
                    return currentLocation == location ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }
}
