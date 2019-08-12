using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AX.WPF.Converters
{
    public abstract class TypeSafeConverter<SourceType, TargetType, ParameterType> : IValueConverter
    {
        public abstract bool CanConvert { get; }
        public abstract bool CanConvertBack { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (CanConvert)
                return Convert((SourceType)value, (ParameterType)parameter, culture);
            else
                return null;
        }

        public abstract TargetType Convert(SourceType value, ParameterType parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (CanConvertBack)
                return ConvertBack((TargetType)value, (ParameterType)parameter, culture);
            else
                return null;
        }

        public abstract SourceType ConvertBack(TargetType value, ParameterType parameter, CultureInfo culture);
    }

    public abstract class TypeSafeConverter<SourceType, TargetType> : TypeSafeConverter<SourceType, TargetType, object>
    { }
}
