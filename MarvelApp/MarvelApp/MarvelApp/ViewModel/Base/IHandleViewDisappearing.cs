using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.ViewModel.Base
{
    public interface IHandleViewDisappearing
    {
        Task OnViewDisappearingAsync(VisualElement view);
    }
}
