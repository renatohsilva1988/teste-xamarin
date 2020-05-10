using System;
using System.Globalization;
using Xamarin.Forms;

namespace MarvelApp.Converter
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = (parameter as string) ?? "u";

            switch (param.ToUpper())
            {
                case "U":
                    return ((string)value).ToUpper();
                case "L":
                    return ((string)value).ToLower();
                default:
                    return ((string)value);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
