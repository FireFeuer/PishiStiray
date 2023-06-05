using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace PishiStiray.Servieces
{
    public class StringToBrushOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as int?;
            if (str == null) { return new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#30d5c8")); }
            else if (str >= 4) { return new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#76e383")); }
            else { return new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#30d5c8")); }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
