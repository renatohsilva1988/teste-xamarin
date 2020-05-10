﻿using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using MarvelApp.iOS.Renders;
using MarvelApp.Controls;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace MarvelApp.iOS.Renders
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            var text = Control?.AttributedText as NSMutableAttributedString;
            if (text == null)
            {
                return;
            }

            var fontFamily = Element.FontFamily;
            text.BeginEditing();
            FixBackground(text);
            if (Element.FormattedText == null)
            {
                FixFontAtLocation(0, text, fontFamily, Element.FontAttributes);
            }
            else
            {
                var location = 0;
                foreach (var span in Element.FormattedText.Spans)
                {
                    var spanFamily = span.FontFamily ?? fontFamily;
                    FixFontAtLocation(location, text, spanFamily, span.FontAttributes);
                    location += span.Text.Length;
                }
            }
            text.EndEditing();
        }

        private void FixFontAtLocation(int location, NSMutableAttributedString text, string fontFamily, FontAttributes fontAttributes)
        {
            if (fontFamily == null)
            {
                return;
            }

            NSRange range;
            var font = (UIFont)text.GetAttribute(UIStringAttributeKey.Font, location, out range);
            var baseFontName = GetBaseFontName(fontFamily);

            if (font.Name.Contains("-") && font.Name.StartsWith(baseFontName))
            {
                return;
            }

            font = UIFont.FromName(fontFamily, font.PointSize);
            text.RemoveAttribute(UIStringAttributeKey.Font, range);
            text.AddAttribute(UIStringAttributeKey.Font, font, range);
        }

        private static void FixBackground(NSMutableAttributedString text)
        {
            var str = text.Value;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == '\n')
                {
                    text.RemoveAttribute(UIStringAttributeKey.BackgroundColor, new NSRange(i, 1));
                }
            }
        }

        private static string GetBaseFontName(string fontName)
        {
            var index = fontName.IndexOf('-');
            if (index >= 0)
            {
                fontName = fontName.Substring(0, index);
            }
            return fontName;
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