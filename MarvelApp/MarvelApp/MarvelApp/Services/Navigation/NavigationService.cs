using MarvelApp.Helpers;
using MarvelApp.Model;
using MarvelApp.Model.Navigation;
using MarvelApp.View.Base;
using MarvelApp.ViewModel.Base;
using MarvelApp.ServicesContracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public async Task InitializeAsync<TViewModel>(NavigationParameters parameters = null, bool navigationPage = false, NavigationPage customNavigationPage = null) where TViewModel : BaseViewModel
        {
            await InternalInitializeAsync(typeof(TViewModel), parameters, navigationPage, customNavigationPage);
        }

        public async Task InsertBeforeNavigationAsync<TViewModel, TViewModelBefore>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
                                                                                                                     where TViewModelBefore : BaseViewModel
        {
            await InternalInsertBeforeNavigationAsync(typeof(TViewModel), typeof(TViewModelBefore), parameters);
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public async Task NavigateToAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameters);
        }

        public async Task NavigateToAsync(Type viewModelType)
        {
            await InternalNavigateToAsync(viewModelType, null);
        }

        public async Task NavigateToAsync(Type viewModelType, NavigationParameters parameters)
        {
            await InternalNavigateToAsync(viewModelType, parameters);
        }

        public async Task NavigateMenuItemAsync<TViewModelMasterDetail>(Model.MasterDetail.MenuItem selectedMenuItem, NavigationParameters parameters = null) where TViewModelMasterDetail : BaseViewModel
        {
            await InternalNavigateMenuItemAsync(typeof(TViewModelMasterDetail), selectedMenuItem, parameters);
        }

        public async Task InitializeForMasterDetailAsync<TViewModelMasterDetail, TViewModelDetail>(NavigationParameters parameters = null) where TViewModelMasterDetail : BaseViewModel
                                                                                                                                           where TViewModelDetail : BaseViewModel
        {
            await InternalInitializeMasterDetailPage(parameters, typeof(TViewModelMasterDetail), typeof(TViewModelDetail));
        }

        public async Task NavigatePopAsync(bool modal = false)
        {
            await InternalNavigatePopAsync(modal);
        }

        public async Task NavigateTabbedPageToAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            await InternalNavigateTabbedPageToAsync(typeof(TViewModel), parameters);
        }

        public async Task ReplaceNewChildTabbedPageToAsync<TViewModel>(int indexPage = 0, NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            await InternalReplaceNewChildTabbedPageToAsync(indexPage, typeof(TViewModel), parameters);
        }

        public async Task NavigateTabbedPagePopAsync()
        {
            await InternalNavigatetabbedPagePopAsync();
        }

        public async Task NavigateAndClearBackStackAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            try
            {
                Page page = await CreateAndBindPage(typeof(TViewModel), parameters);
                var navigationPage = CurrentApplication.MainPage as NavigationPage;

                await navigationPage.PushAsync(page);
                await ClearBackStackAsync(page, navigationPage);
            }
            catch (Exception ex)
            {
                ex.LogException();
                throw new Exception($"NavigateAndClearBackStackAsync: {ex.Message}");
            }
        }

        public async Task NavigatePopPopupAsync()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAsync();
            }

            await Task.CompletedTask;
        }

        public async Task RemovePopUpPageAsync(VisualElement view)
        {
            var popup = view as PopupPage;

            if (PopupNavigation.Instance.PopupStack.Contains(popup))
            {
                await PopupNavigation.Instance.RemovePageAsync(popup);
            }
        }

        public async Task NavigatePushPopupAsync<TViewModel>(NavigationParameters parameters) where TViewModel : BaseViewModel
        {
            await InternalNavigatePushPopupAsync(typeof(TViewModel), parameters);
        }

        private async Task ClearBackStackAsync(Page page, NavigationPage navigationPage)
        {
            var tasks = new List<Task>();
            if (navigationPage != null && navigationPage.Navigation.NavigationStack.Count > 0)
            {
                var existingPages = navigationPage.Navigation.NavigationStack.Where(existingPage => existingPage != page).ToList();
                foreach (var existingPage in existingPages)
                {
                    tasks.Add(RemovePage(existingPage, navigationPage));
                }
                await Task.WhenAll(tasks);
            }
        }

        private Task RemovePage(Page page, NavigationPage navigationPage)
        {
            navigationPage.Navigation.RemovePage(page);
            return Task.CompletedTask;
        }

        private async Task InternalNavigatePopAsync(bool modal = false)
        {
            if (CurrentApplication.MainPage is NavigationPage currentNavigationPage)
            {
                if (modal)
                {
                    await CurrentApplication.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await currentNavigationPage.PopAsync();
                }
            }
        }

        private async Task InternalNavigateTabbedPageToAsync(Type viewModelType, NavigationParameters parameters = null)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);
                if (CurrentApplication.MainPage is NavigationPage navigationPage)
                {
                    Page current = navigationPage.CurrentPage;
                    if (current is TabbedPage tabbedPage)
                    {
                        await tabbedPage.CurrentPage.Navigation.PushAsync(page);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalReplaceNewChildTabbedPageToAsync(int indexPage, Type viewModelType, NavigationParameters parameters = null)
        {
            try
            {
                await BeginInvokeOnMainThreadHelper.BeginInvokeOnMainThreadAsync(async () =>
                {
                    if (CurrentApplication.MainPage is NavigationPage navigationPage)
                    {
                        if (navigationPage.CurrentPage is TabbedPage currentTabbedPagePage)
                        {
                            Type actualPageType = currentTabbedPagePage.Children[indexPage].GetType();
                            Type newPageType = GetPageTypeForViewModel(viewModelType);
                            if (!(actualPageType).Equals(newPageType))
                            {
                                Page newPage = await CreateAndBindPage(viewModelType, parameters);
                                BasePage oldPage = currentTabbedPagePage.Children[indexPage] as BasePage;
                                await Task.Delay(300);
                                currentTabbedPagePage.Children[indexPage] = newPage;
                            }
                        }
                    }
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public Task SendToNotificationsPage()
        {
            if ((CurrentApplication.MainPage as NavigationPage).CurrentPage is TabbedPage paginaPrincipalTabbedView)
            {
                ((CurrentApplication.MainPage as NavigationPage).CurrentPage as TabbedPage).CurrentPage = new Page();
                ((CurrentApplication.MainPage as NavigationPage).CurrentPage as TabbedPage).CurrentPage = paginaPrincipalTabbedView.Children[2];
            }
            return Task.CompletedTask;
        }

        private async Task InternalNavigatetabbedPagePopAsync()
        {
            try
            {
                if (CurrentApplication.MainPage is NavigationPage navigationPage)
                {
                    Page current = navigationPage.CurrentPage;
                    if (current is TabbedPage tabbedPage)
                    {
                        await tabbedPage.CurrentPage.Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalNavigateToAsync(Type viewModelType, NavigationParameters parameters, bool modal = false)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);

                if (CurrentApplication.MainPage is NavigationPage currentNavigationPage)
                {
                    if (modal)
                    {
                        await CurrentApplication.MainPage.Navigation.PushModalAsync(page);
                    }
                    else
                    {
                        await currentNavigationPage.PushAsync(page);
                    }
                }
                else
                {
                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalNavigatePushPopupAsync(Type viewModelType, NavigationParameters parameters)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);
                if (page is PopupPage popupPage)
                {
                    await PopupNavigation.Instance.PushAsync(popupPage);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalInsertBeforeNavigationAsync(Type viewModelType, Type viewModelTypeBefore, NavigationParameters parameters)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);
                Page pageBefore = await CreateAndBindPage(viewModelTypeBefore, parameters);

                if (CurrentApplication.MainPage is NavigationPage currentNavigationPage)
                {
                    if (currentNavigationPage.Navigation.NavigationStack.All(p => p != page))
                    {
                        currentNavigationPage.Navigation.InsertPageBefore(page, pageBefore);
                        await currentNavigationPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalInitializeAsync(Type viewModelType, NavigationParameters parameters, bool navigationPage = false, NavigationPage customNavigationPage = null)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);

                if (navigationPage)
                {
                    if (customNavigationPage != null)
                    {
                        CurrentApplication.MainPage = (NavigationPage)Activator.CreateInstance(customNavigationPage.GetType(), page);
                    }
                    else
                    {
                        CurrentApplication.MainPage = new NavigationPage(page);
                    }
                }
                else
                {
                    CurrentApplication.MainPage = page;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalInitializeMasterDetailPage(NavigationParameters parameters, Type typePage, Type typeDetailPage)
        {
            try
            {
                CurrentApplication.MainPage = await CreateAndBindMasterDetailPage(typePage, typeDetailPage, parameters);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private async Task InternalNavigateMenuItemAsync(Type viewModelMasterDetailType, Model.MasterDetail.MenuItem selectedMenuItem, NavigationParameters parameters = null)
        {
            try
            {
                Page page = await CreateAndBindPage(selectedMenuItem.ViewModelType, parameters);

                if (CurrentApplication.MainPage is MasterDetailPage currentPage)
                {
                    currentPage.Detail = new NavigationPage(page);
                    currentPage.IsPresented = false;
                }
                else
                {
                    MasterDetailPage masterDetailPage = await CreateAndBindMasterDetailPage(viewModelMasterDetailType, selectedMenuItem.ViewModelType, parameters);
                    CurrentApplication.MainPage = masterDetailPage;
                    if (CurrentApplication.MainPage is MasterDetailPage masterPage)
                    {
                        masterPage.IsPresented = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!ViewModelLocator.Current.mappings.ContainsKey(viewModelType))
            {
                KeyNotFoundException ex = new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
                ex.LogException();
                throw ex;
            }

            return ViewModelLocator.Current.mappings[viewModelType];
        }

        private async Task<Page> CreateAndBindPage(Type viewModelType, NavigationParameters parameters)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                {
                    Exception ex = new Exception($"Mapping type for {viewModelType} is not a page");
                    ex.LogException();
                    throw ex;
                }

                Page page = null;

                page = ViewModelLocator.Current.Resolve(pageType) as Page;
                BaseViewModel viewModel = ViewModelLocator.Current.Resolve(viewModelType) as BaseViewModel;
                await viewModel.SetParameters(parameters);
                await viewModel.OnInitialize();
                page.BindingContext = viewModel;

                return page;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<MasterDetailPage> CreateAndBindMasterDetailPage(Type masterDetailViewModelType, Type detailViewModelType, NavigationParameters parameters)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(masterDetailViewModelType);

                if (pageType == null)
                {
                    Exception ex = new Exception($"Mapping type for {masterDetailViewModelType} is not a page");
                    ex.LogException();
                    throw ex;
                }

                MasterDetailPage page = ViewModelLocator.Current.Resolve(pageType) as MasterDetailPage;

                BaseViewModel viewModel = ViewModelLocator.Current.Resolve(masterDetailViewModelType) as BaseViewModel;
                page.BindingContext = viewModel;
                page.Master.BindingContext = viewModel;

                var newPage = await CreateAndBindPage(detailViewModelType, parameters);
                page.Detail = new NavigationPage(newPage);

                return page;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
