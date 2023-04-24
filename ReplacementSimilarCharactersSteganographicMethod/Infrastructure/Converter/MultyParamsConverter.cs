using System;
using System.Globalization;
using System.Windows.Data;

namespace RSCSteganographicMethod.Infrastructure.Converter
{
    public class MultyParamsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return (object[])((object[])value).Clone();
        }
    }
}
