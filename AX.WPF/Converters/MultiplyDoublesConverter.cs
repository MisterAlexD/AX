using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AX.WPF.Converters
{
    public class MultiplyDoublesConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = 1d;
            foreach (var value in values)
            {
                result *= (double)value;
            }
            return result;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MultiplyDoublesConverter does not support ConvertBack");
        }
    }
}
