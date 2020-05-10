using System;
using Android.Content;
using Android.Graphics.Drawables;
using MarvelApp.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Xamarin.Forms.Button;
using Android.Animation;
using MarvelApp.Controls;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace MarvelApp.Droid.Renders
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                if (e.NewElement != null)
                {
                    var roundedButton = (CustomButton)e.NewElement;
                    var gradientDrawable = new GradientDrawable();
                    gradientDrawable.SetShape(ShapeType.Rectangle);
                    gradientDrawable.SetColor(roundedButton.BackgroundColor.ToAndroid());
                    gradientDrawable.SetStroke(Convert.ToInt32(roundedButton.BorderWidth),
                        roundedButton.BorderColor.ToAndroid());
                    gradientDrawable.SetCornerRadius(roundedButton.CornerRadius);
                    Control.SetBackground(gradientDrawable);
                    Control.SetTextColor(roundedButton.TextColor.ToAndroid());
                    Control.SetAllCaps(false);
                    Control.StateListAnimator = new StateListAnimator();
                    Control.Elevation = 0;
                    e.NewElement.Released += OnReleased;
                    e.NewElement.Pressed += OnPressed;
                }
                else if (e.OldElement != null)
                {
                    e.OldElement.Pressed -= OnPressed;
                    e.OldElement.Released -= OnReleased;
                }
            }
        }

        private void OnReleased(object sender, EventArgs e)
        {
            if (CheckNullValues())
            {
                return;
            }
            var roundedButton = (CustomButton)Element;
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetShape(ShapeType.Rectangle);
            gradientDrawable.SetColor(roundedButton.BackgroundColor.ToAndroid());
            gradientDrawable.SetStroke(Convert.ToInt32(roundedButton.BorderWidth), roundedButton.BorderColor.ToAndroid());
            gradientDrawable.SetCornerRadius(roundedButton.CornerRadius);
            Control.SetBackground(gradientDrawable);
            Control.SetTextColor(roundedButton.TextColor.ToAndroid());
        }

        private void OnPressed(object sender, EventArgs e)
        {
            if (CheckNullValues())
            {
                return;
            }
            var roundedButton = (CustomButton)Element;
            var gradientDrawable = new GradientDrawable();
            gradientDrawable.SetShape(ShapeType.Rectangle);
            gradientDrawable.SetColor(((CustomButton)Element).AltBackgroundColor.ToAndroid());
            gradientDrawable.SetStroke(Convert.ToInt32(roundedButton.BorderWidth), ((CustomButton)Element).AltBorderColor.ToAndroid());
            gradientDrawable.SetCornerRadius(roundedButton.CornerRadius);
            Control.SetBackground(gradientDrawable);
            Control.SetTextColor(((CustomButton)Element).AltTextColor.ToAndroid());
        }

        private bool CheckNullValues()
        {
            return (Control == null) || (Element == null);
        }
    }
}