using MarvelApp.Model.Navigation;
using MarvelApp.ViewModel;
using MarvelApp.ServicesContracts;
using System;
using System.Threading.Tasks;

namespace MarvelApp.Services.Dialog
{
    public class PopPupDialogService : IPopPupDialogService
    {
        public PopPupDialogService(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private INavigationService NavigationService { get; }

        public async Task AlertAsync(string title, string message, string textoBotao = "OK")
        {
            await AlertAsync(title, message, null, textoBotao);
        }

        public async Task AlertAsync(string title, string message, Action callbackFunction, string textoBotao = "OK")
        {
            NavigationParameters parameter = new NavigationParameters(NavigationParameterHandle.HandleTituloPopUp, title)
            {
                { NavigationParameterHandle.HandleMensagemPopUp, message },
                { NavigationParameterHandle.HandleCallbackFunctionPopUp, callbackFunction },
                { NavigationParameterHandle.HandleButtonTextPopUp, textoBotao },
                { NavigationParameterHandle.HandleCloseWhenBackgroundIsClicked, true }
            };
            await NavigationService.NavigatePushPopupAsync<AlertaPopUpViewModel>(parameter);
        }

        public async Task AlertModalAsync(string title, string message, string textoBotao = "OK")
        {
            await AlertModalAsync(title, message, null, textoBotao);
        }

        public async Task AlertModalAsync(string title, string message, Action callbackFunction, string textoBotao = "OK")
        {
            NavigationParameters parameter = new NavigationParameters(NavigationParameterHandle.HandleTituloPopUp, title)
            {
                { NavigationParameterHandle.HandleMensagemPopUp, message },
                { NavigationParameterHandle.HandleCallbackFunctionPopUp, callbackFunction },
                { NavigationParameterHandle.HandleButtonTextPopUp, textoBotao },
                { NavigationParameterHandle.HandleCloseWhenBackgroundIsClicked, false }
            };
            await NavigationService.NavigatePushPopupAsync<AlertaPopUpViewModel>(parameter);
        }
    }
}
