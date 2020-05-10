using Xamarin.Forms;

namespace MarvelApp.Controls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(
                nameof(BorderWidth),
                typeof(int),
                typeof(CustomEntry),
                GetBorderWidth());

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                nameof(BorderColor),
                typeof(Color),
                typeof(CustomEntry),
                Color.Gray);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(double),
                typeof(CustomEntry),
                GetCornerRadius());

        public static readonly BindableProperty AltFontFamilyProperty
            = BindableProperty.Create(nameof(AltFontFamily),
                                      typeof(string),
                                      typeof(CustomEntry),
                                      default(string));

        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string AltFontFamily
        {
            get => (string)GetValue(AltFontFamilyProperty);
            set => SetValue(AltFontFamilyProperty, value);
        }


        private static int GetBorderWidth()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return 1;
                default:
                    return 2;
            }
        }

        private static double GetCornerRadius()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return 6;
                default:
                    return 7;
            }
        }
    }
}
