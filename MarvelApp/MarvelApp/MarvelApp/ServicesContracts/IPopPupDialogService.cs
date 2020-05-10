using System;
using System.Threading.Tasks;

namespace MarvelApp.ServicesContracts
{
    public interface IPopPupDialogService
    {
        Task AlertAsync(string title, string message, string textoBotao = "OK");

        Task AlertAsync(string title, string message, Action callbackFunction, string textoBotao = "OK");
        
        Task AlertModalAsync(string title, string message, string textoBotao = "OK");

        Task AlertModalAsync(string title, string message, Action callbackFunction, string textoBotao = "OK");
    }
}
