using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Android.Graphics;
using Android.Text;
using MarvelApp.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using System.Text.RegularExpressions;
using AApplication = Android.App.Application;
using System;
using MarvelApp.Controls;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace MarvelApp.Droid.Renders
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            if (Element?.FormattedText == null)
            {
                return;
            }

            var extensionType = typeof(FormattedStringExtensions);
            var type = extensionType.GetNestedType("FontSpan", BindingFlags.NonPublic);
            var ss = new SpannableString(Control.TextFormatted);
            var spans = ss.GetSpans(0, ss.ToString().Length, Class.FromType(type));
            foreach (var span in spans)
            {
                var font = (Font)type.GetProperty("Font").GetValue(span, null);
                if ((font.FontFamily ?? Element.FontFamily) != null)
                {
                    var start = ss.GetSpanStart(span);
                    var end = ss.GetSpanEnd(span);
                    var flags = ss.GetSpanFlags(span);
                    ss.RemoveSpan(span);
                    var typeFace = FindFont(font.FontFamily);
                    var newSpan = new CustomTypefaceSpan(typeFace);
                    ss.SetSpan(newSpan, start, end, flags);
                }
            }
            Control.TextFormatted = ss;
        }

        const string LOAD_FROM_ASSETS_REGEX = @"\w+\.((ttf)|(otf))\#\w*";
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

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.FormattedTextProperty.PropertyName ||
                e.PropertyName == Label.TextProperty.PropertyName ||
                e.PropertyName == Label.FontAttributesProperty.PropertyName ||
                e.PropertyName == Label.FontProperty.PropertyName ||
                e.PropertyName == Label.FontSizeProperty.PropertyName ||
                e.PropertyName == Label.FontFamilyProperty.PropertyName ||
                e.PropertyName == Label.TextColorProperty.PropertyName)
            {
                UpdateFormattedText();
            }
        }
    }
}