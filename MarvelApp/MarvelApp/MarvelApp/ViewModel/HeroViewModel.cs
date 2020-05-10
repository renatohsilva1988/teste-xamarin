using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.Model.Navigation;
using MarvelApp.ServicesContracts;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MarvelApp.ViewModel.Base;
using MarvelApp.Model;
using Xamarin.Essentials;

namespace MarvelApp.ViewModel
{
    public class HeroViewModel : BaseViewModel
    {
        public string HeaderTitle => GetHeaderPage();

        private readonly IOpenWebService openWebService;
        private bool isOpenWebInfoCommandVisibleEnable;
        private Character character;
        private Command openWebInfoCommand;

        public bool IsOpenWebInfoCommandEnable
        {
            get => isOpenWebInfoCommandVisibleEnable;
            private set => SetProperty(ref isOpenWebInfoCommandVisibleEnable, value);
        }

        public Character Character
        {
            get => character;
            set => SetProperty(ref character, value);
        }

        public Command OpenWebInfoCommand
        {
            get => openWebInfoCommand;
            set => SetProperty(ref openWebInfoCommand, value);
        }

        public HeroViewModel(INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService) : base(navigationService, popPupDialogService, userDialogs, liteDbService, apiManager, secureStorageService)
        {
            openWebService = DependencyService.Get<IOpenWebService>();
            OpenWebInfoCommand = new Command(() =>
            {
                openWebService.OpenUrl(Character.Thumbnail.ToString());
            });
        }

        public override async Task OnInitialize()
        {
            HasInternet();
            await base.OnInitialize();
        }

        public override async Task SetParameters(NavigationParameters parameters)
        {
            try
            {
                await base.SetParameters(parameters);
                Character = Parameters?.GetValue<Character>(NavigationParameterHandle.HandleSelectedCharacter);
            }
            catch (Exception ex)
            {
                var newException = new Exception("Não foi possível encontrar um personagem selecionado.", ex);
                newException.LogException();
                await PopPupDialogService.AlertModalAsync("Atenção", newException.Message);
            }
        }

        protected override void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            HasInternet(e.NetworkAccess);
            base.OnConnectivityChanged(sender, e);
        }

        private void HasInternet()
        {
            SetConectivityVisible(HasConnection());
        }

        private void HasInternet(NetworkAccess networkAccess)
        {
            SetConectivityVisible(HasConnection(networkAccess));
        }

        private void SetConectivityVisible(bool isConnected)
        {
            IsOpenWebInfoCommandEnable = isConnected;
        }

        private string GetHeaderPage()
        {
            return character?.Name;
        }
    }
}
