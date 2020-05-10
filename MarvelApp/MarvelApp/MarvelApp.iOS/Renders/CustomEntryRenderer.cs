using System;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using MarvelApp.iOS.Renders;
using MarvelApp.Controls;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MarvelApp.iOS.Renders
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private CustomEntry CustomElement => Element as CustomEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                Control.LeftViewMode = UITextFieldViewMode.Always;

                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;

                Control.Layer.CornerRadius = Convert.ToSingle(CustomElement.CornerRadius);
                Control.Layer.BorderColor = CustomElement.BorderColor.ToCGColor();
                Control.Layer.BorderWidth = CustomElement.BorderWidth;
                Control.ClipsToBounds = true;

                if (!Control.Enabled)
                {
                    Control.Layer.BackgroundColor = new CGColor(new nfloat(243), new nfloat(244), new nfloat(246));
                    Control.TextColor = UIColor.FromRGB(new nfloat(58), new nfloat(58), new nfloat(58));
                }

                UpdatePlaceholderFont();
            }
        }

        private void UpdatePlaceholderFont()
        {
            if (CustomElement.Placeholder != null)
            {
                var paragraphStyle = new NSMutableParagraphStyle
                {
                    Alignment = UITextAlignment.Left
                };

                var placeholderFont = FindFont(CustomElement.AltFontFamily, (float)CustomElement.FontSize);

                Control.AttributedPlaceholder = new NSAttributedString(CustomElement.Placeholder, placeholderFont, paragraphStyle: paragraphStyle);
            }
        }

        private UIFont FindFont(string fontFamily, float fontSize)
        {
            if (fontFamily != null)
            {
                try
                {
                    if (UIFont.FamilyNames.Contains(fontFamily))
                    {
                        var descriptor = new UIFontDescriptor().CreateWithFamily(fontFamily);
                        return UIFont.FromDescriptor(descriptor, fontSize);
                    }

                    return UIFont.FromName(fontFamily, fontSize);
                }
                catch { }
            }

            return UIFont.SystemFontOfSize(fontSize);
        }
    }
}