using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MarvelApp.Droid.Renders;
using Android.Content;

[assembly: ExportRenderer(typeof(Button), typeof(CustomFlatButtonRenderer))]
namespace MarvelApp.Droid.Renders
{
    public class CustomFlatButtonRenderer : ButtonRenderer
    {
        public CustomFlatButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            Control?.SetAllCaps(false);                        
        }
    }
}