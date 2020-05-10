using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.Model.Navigation;
using MarvelApp.ServicesContracts;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MarvelApp.ViewModel.Base;
using MarvelApp.Model;
using System.Net.Http;
using MarvelApp.Exceptions;
using MarvelApp.Services.Armazenamento.SecureData;
using MarvelApp.Helpers;

namespace MarvelApp.ViewModel
{
    public class InitialViewModel : BaseViewModel
    {
        private string usuario;
        private Command entrarCommand;
        private Command modoClaroCommand;
        private Command modoEscuroCommand;
        private bool lembreMe;

        public string Usuario
        {
            get => usuario;
            set => SetProperty(ref usuario, value);
        }

        public Command EntrarCommand
        {
            get => entrarCommand;
            set => SetProperty(ref entrarCommand, value);
        }

        public Command ModoClaroCommand
        {
            get => modoClaroCommand;
            set => SetProperty(ref modoClaroCommand, value);
        }

        public Command ModoEscuroCommand
        {
            get => modoEscuroCommand;
            set => SetProperty(ref modoEscuroCommand, value);
        }

        public bool LembreMe
        {
            get => lembreMe;
            set => SetProperty(ref lembreMe, value);
        }

        public InitialViewModel(INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService) : base(navigationService, popPupDialogService, userDialogs, liteDbService, apiManager, secureStorageService)
        {
            EntrarCommand = new Command(async () =>
            {
                using (UserDialogService.Loading("Executando"))
                {
                    IsBusy = true;
                    try
                    {
                        if (await Initialize())
                        {
                            await NavigationService.NavigateToAsync<HeroesViewModel>();
                        }
                    }
                    catch (SemConexaoException sex)
                    {
                        await PopPupDialogService.AlertModalAsync("Atenção", sex.Message);
                    }
                    catch (NotFoundException nex)
                    {
                        await PopPupDialogService.AlertModalAsync("Atenção", nex.Message);
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        await PopPupDialogService.AlertModalAsync("Atenção", ex.Message);
                    }
                    finally
                    {
                        IsBusy = false;
                    }

                }
            }, CanExecute());

            ModoClaroCommand = new Command(() =>
            {
                IsBusy = true;
                ResourcesHelper.SetLightMode();
                IsBusy = false;
            }, CanExecute());

            ModoEscuroCommand = new Command(() =>
            {
                IsBusy = true;
                ResourcesHelper.SetDarkMode();
                IsBusy = false;
            }, CanExecute());
        }

        private async Task<bool> Initialize()
        {
            try
            {
                if (Validate())
                {
                    await SecureStorageService.SetValue(SecureKey.LembrarMe, LembreMe.ToString());
                    SalvarUsuario();
                    return true;
                }
                return false;
            }
            catch (SemConexaoException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Problemas ao iniciar a aplicação.", ex);
            }
        }

        private void SalvarUsuario()
        {
            LimparUsuarios();
            IncluirUsuario();
        }

        private void IncluirUsuario()
        {
            var authenticatedUser = new CurrentUser(Usuario);
            LiteDbService.UpsertItem(authenticatedUser);
        }

        private void LimparUsuarios()
        {
            LiteDbService.DeleteAll<CurrentUser>();
        }

        private bool Validate()
        {
            if (!HasConnection())
            {
                throw new SemConexaoException();
            }

            if (string.IsNullOrWhiteSpace(Usuario))
            {
                throw new NotFoundException("O campo usuário é obrigatório");
            }

            return true;
        }
    }
}
