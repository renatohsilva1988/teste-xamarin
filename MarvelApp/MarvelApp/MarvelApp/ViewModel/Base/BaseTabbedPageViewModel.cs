using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.ServicesContracts;
using Xamarin.Forms;

namespace MarvelApp.ViewModel.Base
{
    public class BaseTabbedPageViewModel : BaseViewModel
    {
        public BaseTabbedPageViewModel(INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService) : base(navigationService, popPupDialogService, userDialogs, liteDbService, apiManager, secureStorageService)
        {
            BackCommand = new Command(async () =>
            {
                await NavigationService.NavigateTabbedPagePopAsync();
            });
        }
    }
}
