using MarvelApp.View.Base;
using MarvelApp.ViewModel.Base;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MarvelApp.View.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertaPopUpView : PopupPage, IPage
    {
        private BaseViewModel ViewModel => BindingContext as BaseViewModel;

        public AlertaPopUpView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel is IHandleViewAppearing viewAware)
            {
                viewAware.OnViewAppearingAsync(this);
            }
        }
    }
}