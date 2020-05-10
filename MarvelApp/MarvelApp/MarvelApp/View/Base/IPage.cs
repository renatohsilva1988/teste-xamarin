using Xamarin.Forms;

namespace MarvelApp.View.Base
{
    public interface IPage 
    {
        string Title { get; set; }

        string AutomationId { get; set; }

        ImageSource IconImageSource { get; set; }
    }
}
