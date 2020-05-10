using System;
using System.Collections.Generic;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MarvelApp.iOS.Renders;
using MarvelApp.Controls;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace MarvelApp.iOS.Renders
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        void Button_Released(object sender, EventArgs e)
        {
            var button = (CustomButton)sender;
            button.BackgroundColor = button.Colors[0];
            button.TextColor = button.Colors[1];
            button.BorderColor = button.Colors[2];
        }

        void Button_Pressed(object sender, EventArgs e)
        {
            var button = (CustomButton)sender;
            button.BackgroundColor = button.AltBackgroundColor;
            button.TextColor = button.AltTextColor;
            button.BorderColor = button.AltBorderColor;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (e.OldElement != null)
                {
                    var button = ((CustomButton)e.NewElement);
                    button.Colors = null;
                    button.BorderWidth = 1;
                    button.Released -= Button_Released;
                    button.Pressed -= Button_Pressed;
                }

                if (e.NewElement != null)
                {
                    var button = (CustomButton)e.NewElement;
                    button.Colors = new List<Color>();
                    button.BorderWidth = 1;

                    button.Colors.Add(button.BackgroundColor);
                    button.Colors.Add(button.TextColor);
                    button.Colors.Add(button.BorderColor);

                    button.Released += Button_Released;
                    button.Pressed += Button_Pressed;
                }
            }
        }
    }
}