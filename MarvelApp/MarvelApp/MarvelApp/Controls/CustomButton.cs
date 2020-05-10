using System.Collections.Generic;
using Xamarin.Forms;

namespace MarvelApp.Controls
{
    public class CustomButton : Button
    {
        public List<Color> Colors { get; set; }

        public static readonly BindableProperty AltBackgroundColorProperty  =
            BindableProperty.Create(
                nameof(AltBackgroundColor),
                typeof(Color),
                typeof(CustomButton),
                Color.Default
            );

        public static readonly BindableProperty AltTextColorProperty  =
            BindableProperty.Create(
                nameof(AltTextColor),
                typeof(Color),
                typeof(CustomButton),
                Color.Default
            );

        public static readonly BindableProperty AltBorderColorProperty =
            BindableProperty.Create(
                nameof(AltBorderColor),
                typeof(Color),
                typeof(CustomButton),
                Color.Default
            );
        
        public Color AltBackgroundColor
        {
            get => (Color)GetValue(AltBackgroundColorProperty );
            set => SetValue(AltBackgroundColorProperty , value);
        }

        public Color AltTextColor
        {
            get => (Color)GetValue(AltTextColorProperty );
            set => SetValue(AltTextColorProperty , value);
        }

        public Color AltBorderColor
        {
            get => (Color)GetValue(AltBorderColorProperty);
            set => SetValue(AltBorderColorProperty, value);
        }

    }
}
