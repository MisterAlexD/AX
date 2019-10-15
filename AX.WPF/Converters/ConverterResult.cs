using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AX.WPF.Converters
{
    public enum ConverterResultType
    {
        NotValid, UnsetValue, Valid
    }
    
    public struct ConverterResult<T>
    {
        public T ValidResult { get; set; }
        public ConverterResultType ResultType { get; set; }

        public ConverterResult(ConverterResultType resultType)
        {
            ResultType = resultType;
            ValidResult = default(T);
        }

        public ConverterResult(T validResult)
        {
            ResultType = ConverterResultType.Valid;
            ValidResult = validResult;
        }

        public object GetWPFResult()
        {
            return ResultType == ConverterResultType.Valid ? ValidResult :
                   ResultType == ConverterResultType.UnsetValue ?
                   DependencyProperty.UnsetValue : null;
        }
    }

    public static class ConverterResult
    {
        public static ConverterResult<T> Create<T>(ConverterResultType resultType)
        {
            return new ConverterResult<T>(resultType);
        }

        public static ConverterResult<T> Create<T>(T validResult)
        {
            return new ConverterResult<T>(validResult);
        }
    }

}
