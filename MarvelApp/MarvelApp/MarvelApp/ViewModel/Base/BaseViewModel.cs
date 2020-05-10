using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.Helpers;
using MarvelApp.Model;
using MarvelApp.Model.Base;
using MarvelApp.Model.Navigation;
using MarvelApp.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MarvelApp.ViewModel.Base
{
    public abstract class BaseViewModel : ModelBase, IHandleViewAppearing, IHandleViewDisappearing, IDisposable
    {
        protected INavigationService NavigationService { get; }
        protected IPopPupDialogService PopPupDialogService { get; }
        protected IUserDialogs UserDialogService { get; }
        protected ILiteDbService LiteDbService { get; }
        protected IApiManager ApiManager { get; }
        protected ISecureStorageService SecureStorageService { get; }

        protected bool Inscrito { get; set; }

        protected bool Disposed { get; set; }


        public static event EventHandler OnLogout;

        private Command backCommand;
        private Command logoutCommand;
        private bool isBusy;
        private bool isRefreshing;
        private bool isListaVisivel;
        private bool semInternet;
        private NavigationParameters parameters;
        private string title;
        private bool showMessage;
        private string message;
        private ImageSource iconImageSource;

        public Command BackCommand
        {
            get => backCommand;
            protected set => SetProperty(ref backCommand, value);
        }

        public Command LogoutCommand
        {
            get => logoutCommand;
            protected set => SetProperty(ref logoutCommand, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public bool IsListaVisivel
        {
            get => isListaVisivel;
            set => SetProperty(ref isListaVisivel, value);
        }

        public NavigationParameters Parameters
        {
            get => parameters;
            set => SetProperty(ref parameters, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool ShowMessage
        {
            get => showMessage;
            set => SetProperty(ref showMessage, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public ImageSource IconImageSource
        {
            get => iconImageSource;
            set => SetProperty(ref iconImageSource, value);
        }

        public BaseViewModel(INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService)
        {
            Inscrito = false;
            Disposed = false;
            semInternet = false;
            IsListaVisivel = true;

            NavigationService = navigationService;
            PopPupDialogService = popPupDialogService;
            UserDialogService = userDialogs;
            LiteDbService = liteDbService;
            ApiManager = apiManager;
            SecureStorageService = secureStorageService;

            SubscribeEvents();
            BindHeaderCommands();
        }

        ~BaseViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual Task SetParameters(NavigationParameters parameters)
        {
            Parameters = parameters;
            return Task.CompletedTask;
        }

        public virtual Task OnInitialize()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnLoadPageData()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnViewAppearingAsync(VisualElement view)
        {
            ShowNoConnectionMessage();
            return Task.FromResult(view);
        }

        public virtual Task OnViewDisappearingAsync(VisualElement view) => Task.FromResult(view);

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                UnsubscribeEvents();
            }
            Disposed = true;
        }

        protected async virtual void SubscribeEvents()
        {
            if (Inscrito)
            {
                return;
            }

            await BeginInvokeOnMainThreadHelper.BeginInvokeOnMainThreadAsync(() =>
            {
                Connectivity.ConnectivityChanged += OnConnectivityChanged;
                OnLogout += OnLogoutEventHandler;
                Inscrito = true;
            });
        }

        protected virtual async void UnsubscribeEvents()
        {
            await BeginInvokeOnMainThreadHelper.BeginInvokeOnMainThreadAsync(() =>
            {
                Connectivity.ConnectivityChanged -= OnConnectivityChanged;
                OnLogout -= OnLogoutEventHandler;
                Inscrito = false;
            });
        }

        protected virtual void OnLogoutEventHandler(object sender, EventArgs e)
        {
            UnsubscribeEvents();
        }

        protected virtual async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ShowNoConnectionMessage();

            if (semInternet && HasConnection(e.NetworkAccess))
            {
                semInternet = false;
                await OnInitialize();
            }
            else
            {
                semInternet = true;
            }
        }

        private void ShowNoConnectionMessage()
        {
            Message = "Sem conexão com a internet.";
            ShowMessage = !HasConnection();
        }

        protected Func<bool> CanExecute()
        {
            return new Func<bool>(() => !IsBusy);
        }

        protected bool HasConnection()
        {
            return HasConnection(Connectivity.NetworkAccess);
        }

        protected bool HasConnection(NetworkAccess networkAccess)
        {
            return networkAccess == NetworkAccess.Internet;
        }

        protected virtual void InvokeOnLogout(EventArgs e)
        {
            OnLogout?.Invoke(this, e);
        }

        private void BindHeaderCommands()
        {
            BackCommand = new Command(async () =>
            {
                using (UserDialogService.Loading("Executando"))
                {
                    IsBusy = true;
                    await NavigationService.NavigatePopAsync();
                    IsBusy = false;
                }
            });

            LogoutCommand = new Command(async () =>
            {
                using (UserDialogService.Loading("Executando"))
                {
                    if (IsBusy)
                    {
                        return;
                    }

                    IsBusy = true;
                    await PopPupDialogService.AlertAsync("Atenção", "Deseja sair do aplicativo?", async () => await ExecuteLogoutMethod());
                    IsBusy = false;
                }
            }, CanExecute());
        }

        private async Task ExecuteLogoutMethod()
        {
            using (UserDialogService.Loading("Executando"))
            {
                try
                {
                    await RemoveUser();
                    await ParallelLogout();
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                await NavigationService.InitializeAsync<InitialViewModel>(null, true);
            }
        }

        private async Task ParallelLogout()
        {
            var tasks = new List<Task>()
            {
                Task.Run(() => InvokeOnLogout(EventArgs.Empty)),
                SecureStorageService.DeleteDataAsync()
            };
            await Task.WhenAll(tasks);
        }

        private async Task RemoveUser()
        {
            List<Task> tasks = new List<Task>()
            {
                RemoveUserDb()
            };
            await Task.WhenAll(tasks);
        }

        private async Task RemoveUserDb()
        {
            await Task.Run(() => LiteDbService.DeleteAll<CurrentUser>());
        }
    }
}
