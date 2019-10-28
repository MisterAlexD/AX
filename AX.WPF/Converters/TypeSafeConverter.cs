using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            {
                var result = Convert((SourceType)value, (ParameterType)parameter, culture);
                return result.ResultType == ConverterResultType.Valid ? result.ValidResult :
                       result.ResultType == ConverterResultType.UnsetValue ? DependencyProperty.UnsetValue :
                       null;
            }
            else
                return null;
        }

        public abstract ConverterResult<TargetType> Convert(SourceType value, ParameterType parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (CanConvertBack)
            {
                var result = ConvertBack((TargetType)value, (ParameterType)parameter, culture);
                return result.ResultType == ConverterResultType.Valid ? result.ValidResult :
                       result.ResultType == ConverterResultType.UnsetValue ? DependencyProperty.UnsetValue :
                       null;
            }
            else
                return null;
        }

        public abstract ConverterResult<SourceType> ConvertBack(TargetType value, ParameterType parameter, CultureInfo culture);
    }

    public abstract class TypeSafeConverter<SourceType, TargetType> : TypeSafeConverter<SourceType, TargetType, object>
    { }
}
