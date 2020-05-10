using Android.Graphics;
using Android.Text;
using Android.Text.Style;


namespace MarvelApp.Droid.Renders
{
    public class CustomTypefaceSpan : TypefaceSpan
    {
        private readonly Typeface customTypeface;

        public CustomTypefaceSpan(Typeface type) : base("")
        {
            customTypeface = type;
        }

        public CustomTypefaceSpan(string family, Typeface type) : base(family)
        {
            customTypeface = type;
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            ApplyCustomTypeFace(ds, customTypeface);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            ApplyCustomTypeFace(paint, customTypeface);
        }

        private static void ApplyCustomTypeFace(Paint paint, Typeface tf)
        {
            paint.SetTypeface(tf);
        }
    }
}