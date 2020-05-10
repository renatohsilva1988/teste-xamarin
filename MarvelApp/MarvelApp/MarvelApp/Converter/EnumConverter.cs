using MarvelApp.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MarvelApp.Converter
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Enum enumValue = (Enum)value;
                return enumValue.ToDescriptionString();
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
