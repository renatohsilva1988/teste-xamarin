using MarvelApp.ApiContracts;
using MarvelApp.Exceptions;
using MarvelApp.Model;
using MarvelApp.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MarvelApp.Business.Api
{
    public class ApiManager : IApiManager
    {
        private readonly INetworkService networkService;

        public bool IsConnected { get; set; }
        private Dictionary<int, CancellationTokenSource> RunningTasks { get; set; }

        public ApiManager(INetworkService networkService)
        {
            RunningTasks = new Dictionary<int, CancellationTokenSource>();
            IsConnected = HasIntenet(Connectivity.NetworkAccess);
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            this.networkService = networkService;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = HasIntenet(e.NetworkAccess);
            if (!IsConnected)
            {
                KillApiTasks();
            }
        }

        private Task KillApiTasks()
        {
            var items = RunningTasks.ToList();
            foreach (var item in items)
            {
                item.Value.Cancel();
                RunningTasks.Remove(item.Key);
            }
            return Task.CompletedTask;
        }

        public async Task<TData> MakeRequest<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            AdicionarRequisicao(task.Id, new CancellationTokenSource());
            return await RemoteRequestAsync(task);
        }

        private async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = null;
            try
            {
                if (!HasIntenet())
                {
                    throw new SemConexaoException();
                }
                
                var func = new Func<Task<TData>>(() => task);
                data = await networkService.Retry(func, 3, OnRetry);

                if (data.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await KillApiTasks();
                    throw new UnauthorizedException();
                }
            }
            catch (SemConexaoException)
            {
                throw;
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
                        
            return data;
        }

        private Task OnRetry(Exception ex, int retryCount)
        {
            return Task.Factory.StartNew(() => {
                System.Diagnostics.Debug.WriteLine($"Ocorreu um erro ao executar a requisição: {ex.Message}, tentando novamente...");
            });
        }

        private void AdicionarRequisicao(int taskId, CancellationTokenSource cts)
        {
            RunningTasks.Add(taskId, cts);
        }

        private bool HasIntenet()
        {
            return HasIntenet(Connectivity.NetworkAccess);
        }

        private bool HasIntenet(NetworkAccess networkAccess)
        {
            return networkAccess == NetworkAccess.Internet;
        }
    }
}
