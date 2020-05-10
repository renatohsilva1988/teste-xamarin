using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using MarvelApp.Controls;
using MarvelApp.Droid.Renders;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AApplication = Android.App.Application;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MarvelApp.Droid.Renders
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private const string LOAD_FROM_ASSETS_REGEX = @"\w+\.((ttf)|(otf))\#\w*";

        private CustomEntry CustomElement => Element as CustomEntry;

        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var gradientDrawable = new GradientDrawable();

                if (Control.Enabled)
                {
                    gradientDrawable.SetColor(CustomElement.BackgroundColor.ToAndroid());
                }
                else
                {
                    gradientDrawable.SetColor(new Android.Graphics.Color(243, 244, 246));
                    Control.SetTextColor(new Android.Graphics.Color(58, 58, 58));
                }

                gradientDrawable.SetShape(ShapeType.Rectangle);
                gradientDrawable.SetStroke(Convert.ToInt32(CustomElement.BorderWidth), CustomElement.BorderColor.ToAndroid());
                gradientDrawable.SetCornerRadius(DpToPixels(Context, Convert.ToSingle(CustomElement.CornerRadius)));

                Control.SetBackground(gradientDrawable);
                Control.SetPadding((int)DpToPixels(Context, Convert.ToSingle(15)), Control.PaddingTop, (int)DpToPixels(Context, Convert.ToSingle(15)), Control.PaddingBottom);

                UpdatePlaceholderFont();
            }
        }

        private void UpdatePlaceholderFont()
        {
            if (CustomElement.AltFontFamily == default)
            {
                Control.HintFormatted = null;
                Control.Hint = CustomElement.Placeholder;
                Control.SetHintTextColor(CustomElement.PlaceholderColor.ToAndroid());
                return;
            }

            var placeholderFontSize = (int)CustomElement.FontSize;
            var placeholderSpan = new SpannableString(CustomElement.Placeholder);
            placeholderSpan.SetSpan(new AbsoluteSizeSpan(placeholderFontSize, true), 0, placeholderSpan.Length(), SpanTypes.InclusiveExclusive); // Set Fontsize

            var typeFace = FindFont(CustomElement.AltFontFamily);
            var typeFaceSpan = new CustomTypefaceSpan(typeFace);
            placeholderSpan.SetSpan(typeFaceSpan, 0, placeholderSpan.Length(), SpanTypes.InclusiveExclusive); //Set Fontface

            Control.HintFormatted = placeholderSpan;
        }

        private Typeface FindFont(string fontFamily)
        {
            if (!string.IsNullOrWhiteSpace(fontFamily))
            {
                if (Regex.IsMatch(fontFamily, LOAD_FROM_ASSETS_REGEX))
                {
                    var typeface = Typeface.CreateFromAsset(AApplication.Context.Assets, FontNameToFontFile(fontFamily));
                    return typeface;
                }
                return Typeface.Create(fontFamily, TypefaceStyle.Normal);
            }

            return Typeface.Create(Typeface.Default, TypefaceStyle.Normal);
        }

        private string FontNameToFontFile(string fontFamily)
        {
            int hashtagIndex = fontFamily.IndexOf('#');
            if (hashtagIndex >= 0)
            {
                return fontFamily.Substring(0, hashtagIndex);
            }

            throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
        }

        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }
    }
}