using MarvelApp.ApiContracts;
using MarvelApp.ServicesContracts;
using MarvelApp.Setup;
using System;
using System.Net.Http;

namespace MarvelApp.Business.Api.Base
{
    public abstract class ApiCallerBase
    {
        public ISecureStorageService SecureStorageService { get; }
        public IApiManager ApiManager { get; }

        public ApiCallerBase(ISecureStorageService secureStorageService, IApiManager apiManager)
        {
            SecureStorageService = secureStorageService;
            ApiManager = apiManager;
        }

        protected Uri BaseAddress
        {
            get
            {
                return new Uri(Startup.BaseAddress());
            }
        }
    }
}
