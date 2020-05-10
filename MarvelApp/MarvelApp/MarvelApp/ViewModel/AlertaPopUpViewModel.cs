using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.Model.Navigation;
using MarvelApp.ServicesContracts;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MarvelApp.ViewModel.Base;

namespace MarvelApp.ViewModel
{
    public class AlertaPopUpViewModel : BaseViewModel
    {
        private string titulo;
        private string mensagem;
        private bool? closeWhenBackgroundIsClicked;
        private Action callbackFunction;
        private string textoBotao;
        private Command okCommand;

        public string Titulo
        {
            get => titulo;
            set => SetProperty(ref titulo, value);
        }

        public string Mensagem
        {
            get => mensagem;
            set => SetProperty(ref mensagem, value);
        }

        public bool? CloseWhenBackgroundIsClicked
        {
            get => closeWhenBackgroundIsClicked;
            set => SetProperty(ref closeWhenBackgroundIsClicked, value);
        }

        public string TextoBotao
        {
            get => textoBotao;
            set => SetProperty(ref textoBotao, value);
        }

        public Command OkCommand
        {
            get => okCommand;
            set => SetProperty(ref okCommand, value);
        }

        public Action CallbackFunction
        {
            get => callbackFunction;
            set => SetProperty(ref callbackFunction, value);
        }

        public AlertaPopUpViewModel(INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService) : base(navigationService, popPupDialogService, userDialogs, liteDbService, apiManager, secureStorageService)
        {
            OkCommand = new Command(async () =>
            {
                using (UserDialogService.Loading("Executando"))
                {
                    if (IsBusy)
                    {
                        return;
                    }

                    IsBusy = true;
                    CallbackFunction?.Invoke();
                    await NavigationService.NavigatePopPopupAsync();
                    IsBusy = false;
                }
            }, CanExecute());
        }

        public override async Task OnViewAppearingAsync(VisualElement view)
        {
            Titulo = Parameters?.GetValue<string>(NavigationParameterHandle.HandleTituloPopUp);
            Mensagem = Parameters?.GetValue<string>(NavigationParameterHandle.HandleMensagemPopUp);
            TextoBotao = Parameters?.GetValue<string>(NavigationParameterHandle.HandleButtonTextPopUp);
            CallbackFunction = Parameters?.GetValue<Action>(NavigationParameterHandle.HandleCallbackFunctionPopUp);
            CloseWhenBackgroundIsClicked = Parameters?.GetValue<bool>(NavigationParameterHandle.HandleCloseWhenBackgroundIsClicked);
            
            await base.OnViewAppearingAsync(view);
        }
    }
}
