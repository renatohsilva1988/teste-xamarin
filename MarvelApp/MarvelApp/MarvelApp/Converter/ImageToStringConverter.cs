using MarvelApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MarvelApp.Converter
{
    public class ImageToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                         object parameter, CultureInfo culture)
        {
            if ((MarvelImageUrl)value == null)
                return "";

            return ((MarvelImageUrl)value).ToString();
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            MarvelImageUrl image = new MarvelImageUrl();

            if (value != null)
            {
                List<String> palavrasImagem = value.ToString().Split('.').ToList();

                var extensao = palavrasImagem.LastOrDefault();
                palavrasImagem.Remove(extensao);

                var path = String.Join(".", palavrasImagem);

                image = new MarvelImageUrl(extensao, path);
            }



            return image;
        }
    }
}
