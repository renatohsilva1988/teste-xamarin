using System.Net.Http;
using System.Threading.Tasks;

namespace MarvelApp.ApiContracts
{
    public interface IApiManager
    {
        Task<TData> MakeRequest<TData>(Task<TData> task) where TData : HttpResponseMessage, new();
    }
}
