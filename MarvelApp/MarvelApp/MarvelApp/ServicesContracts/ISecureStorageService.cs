using System.Threading.Tasks;

namespace MarvelApp.ServicesContracts
{
    public interface ISecureStorageService
    {
        Task SetValue(string key, string value);

        Task<string> GetValue(string key);

        void RemoveValue(string key);

        Task DeleteDataAsync();
    }
}
