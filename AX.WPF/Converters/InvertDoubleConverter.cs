using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AX.WPF.Converters
{
    public class InvertDoubleConverter : TypeSafeConverter<double, double>
    {
        public override bool CanConvert => true;

        public override bool CanConvertBack => true;

        public override ConverterResult<double> Convert(double value, object parameter, CultureInfo culture)
        {
            return new ConverterResult<double>(1 / value);
        }

        public override ConverterResult<double> ConvertBack(double value, object parameter, CultureInfo culture)
        {
            return new ConverterResult<double>(1 / value);
        }
    }
}
