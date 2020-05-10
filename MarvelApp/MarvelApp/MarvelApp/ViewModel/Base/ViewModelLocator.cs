using Acr.UserDialogs;
using DryIoc;
using MarvelApp.Business.Api;
using MarvelApp.ApiContracts;
using MarvelApp.Services.Armazenamento.SecureData;
using MarvelApp.Services.Database;
using MarvelApp.Services.Dialog;
using MarvelApp.Services.Navigation;
using MarvelApp.Services.Network;
using MarvelApp.ServicesContracts;
using MarvelApp.View.Base;
using System;
using System.Collections.Concurrent;
using static DryIoc.Made;

namespace MarvelApp.ViewModel.Base
{
    public class ViewModelLocator
    {
        public Container containerBuilder;

        internal ConcurrentDictionary<Type, Type> mappings;

        private static readonly Lazy<ViewModelLocator> lazyViewModel = new Lazy<ViewModelLocator>(() => new ViewModelLocator());

        public static ViewModelLocator Current => lazyViewModel.Value;

        public ViewModelLocator()
        {
            containerBuilder = new Container(rules => rules.WithoutFastExpressionCompiler());

            containerBuilder.Register<INavigationService, NavigationService>();
            containerBuilder.Register<IPopPupDialogService, PopPupDialogService>();
            containerBuilder.Register<ISecureStorageService, SecureStorageService>();        
            containerBuilder.Register(made: Of(() => UserDialogs.Instance));
            containerBuilder.Register<IMavelApiCaller, MavelApiCaller>();            
            containerBuilder.Register<ILiteDbService, LiteDbService>(reuse:Reuse.Singleton);
            containerBuilder.Register<INetworkService, NetworkService>(reuse: Reuse.Singleton);
            containerBuilder.Register<IApiManager, ApiManager>(reuse: Reuse.Singleton);

            mappings = new ConcurrentDictionary<Type, Type>();
        }

        public T Resolve<T>() => containerBuilder.Resolve<T>();

        public object Resolve(Type type) => containerBuilder.Resolve(type);

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface =>
            containerBuilder.Register<TInterface, TImplementation>();

        public void Register<T>() where T : class => containerBuilder.Register<T>();

        public void RegisterForNavigation<TView, TViewModel>(IReuse reuseItem = null)
        where TView : IPage
        where TViewModel : BaseViewModel
        {
            containerBuilder.Register<TViewModel>(reuse: reuseItem, ifAlreadyRegistered: IfAlreadyRegistered.Keep, setup: DryIoc.Setup.With(trackDisposableTransient: true));
            containerBuilder.Register<TView>(reuse: reuseItem, ifAlreadyRegistered: IfAlreadyRegistered.Keep, setup: DryIoc.Setup.With(trackDisposableTransient: true));
            AddMappings(typeof(TViewModel), typeof(TView));
        }

        public void Dispose()
        {
            containerBuilder.Dispose();
        }

        private void AddMappings(Type typeIn, Type typeOut)
        {
            mappings.AddOrUpdate(typeIn, typeOut, (key, existingVal) =>
            {
                if (typeOut != existingVal)
                {
                    throw new ArgumentException("Duplicate values are not allowed: {0}.", typeOut.Name);
                }
                return existingVal;
            });
        }
    }
}
