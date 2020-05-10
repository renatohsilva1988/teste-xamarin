using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.ViewModel.Base
{
    public interface IHandleViewAppearing
    {
       Task OnViewAppearingAsync(VisualElement view);
    }
}
