using MarvelApp.Model;
using System.Threading.Tasks;

namespace MarvelApp.ApiContracts
{
    public interface IMavelApiCaller
    {
        Task<ApiDataContainer<Character>> GetPersonagens(PageData pageData, string filter = null);
    }
}
