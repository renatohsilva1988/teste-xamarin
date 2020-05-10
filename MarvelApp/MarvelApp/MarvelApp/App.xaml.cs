using MarvelApp.Helpers;
using MarvelApp.Model;
using MarvelApp.Services.Armazenamento.SecureData;
using MarvelApp.ServicesContracts;
using MarvelApp.Setup;
using MarvelApp.View;
using MarvelApp.View.Components;
using MarvelApp.ViewModel;
using MarvelApp.ViewModel.Base;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MarvelApp
{
    public partial class App : Application
    {
        public static readonly PerfilDeExecucao PerfilDeExecucao = PerfilDeExecucao.Renato;

        public CurrentUser User { get; set; }

        private ISecureStorageService secureStorageService;
        private ILiteDbService liteDbService;
        private INavigationService navigationService;

        public App()
        {
            InitializeComponent();
            RegisterViewModels();
            StartSettings();
            InitializeApp();
        }

        private static void RegisterViewModels()
        {
            ViewModelLocator.Current.RegisterForNavigation<AlertaPopUpView, AlertaPopUpViewModel>();
            ViewModelLocator.Current.RegisterForNavigation<InitialView, InitialViewModel>();
            ViewModelLocator.Current.RegisterForNavigation<HeroesView, HeroesViewModel>();
            ViewModelLocator.Current.RegisterForNavigation<HeroView, HeroViewModel>();
        }

        private void InitializeApp()
        {
            ThreadHelper.Init(SynchronizationContext.Current);
            ThreadHelper.RunOnUIThread(async () => { await InitializeNavigation(); });
        }

        private void StartSettings()
        {
            secureStorageService = ViewModelLocator.Current.Resolve<ISecureStorageService>();
            liteDbService = ViewModelLocator.Current.Resolve<ILiteDbService>();
            navigationService = ViewModelLocator.Current.Resolve<INavigationService>();
        }

        private async Task InitializeNavigation()
        {
            ResourcesHelper.SetLightMode();
            bool lembrarMe;
            bool.TryParse(await secureStorageService.GetValue(SecureKey.LembrarMe), out lembrarMe);

            var currentUser = liteDbService.FirstOrDefaut<CurrentUser>();
            if (lembrarMe && currentUser != null)
            {
                await navigationService.InitializeAsync<HeroesViewModel>(null, true);
            }
            else
            {
                await navigationService.InitializeAsync<InitialViewModel>(null, true);
            }
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=3a658d11-a983-41af-bfc8-ced386a4a2a6;" +
               "ios=2f2943fb-8ad7-42c1-9123-990164eea6ef",
               typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
