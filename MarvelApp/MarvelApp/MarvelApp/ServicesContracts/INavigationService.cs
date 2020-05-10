using MarvelApp.Model.Navigation;
using MarvelApp.ViewModel.Base;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.ServicesContracts
{
    public interface INavigationService
    {
        Task InitializeAsync<TViewModel>(NavigationParameters parametros = null, bool paginaNavegacao = false, NavigationPage paginaNavegacaoCustomizada = null) where TViewModel : BaseViewModel;

        Task InitializeForMasterDetailAsync<TViewModelMasterDetail, TViewModelDetail>(NavigationParameters parameters = null) where TViewModelMasterDetail : BaseViewModel
                                                                                                                               where TViewModelDetail : BaseViewModel;

        Task InsertBeforeNavigationAsync<TViewModel, TViewModelBefore>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
                                                                                                             where TViewModelBefore : BaseViewModel;
        Task NavigateMenuItemAsync<TViewModelMasterDetail>(Model.MasterDetail.MenuItem selectedMenuItem, NavigationParameters parameters = null) where TViewModelMasterDetail : BaseViewModel;

        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(NavigationParameters parametros = null) where TViewModel : BaseViewModel;
        Task NavigateToAsync(Type viewModelType);
        Task NavigateToAsync(Type viewModelType, NavigationParameters parametros = null);
        Task NavigateTabbedPageToAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel;
        Task ReplaceNewChildTabbedPageToAsync<TViewModel>(int indexPage = 0, NavigationParameters parameters = null) where TViewModel : BaseViewModel;
        Task NavigateAndClearBackStackAsync<TViewModel>(NavigationParameters parametros = null) where TViewModel : BaseViewModel;
        Task NavigatePushPopupAsync<TViewModel>(NavigationParameters parametros) where TViewModel : BaseViewModel;
        Task NavigatePopPopupAsync();
        Task NavigateTabbedPagePopAsync();
        Task NavigatePopAsync(bool modal = false);
        Task RemovePopUpPageAsync(VisualElement view);
        Task SendToNotificationsPage();
    }
}
