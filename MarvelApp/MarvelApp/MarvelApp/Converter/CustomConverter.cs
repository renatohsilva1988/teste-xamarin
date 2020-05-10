using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MarvelApp.Converter
{
    public class CustomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object eventArgs = null;
            if (value is ItemTappedEventArgs)
            {
                eventArgs = value as ItemTappedEventArgs;
            }
            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
