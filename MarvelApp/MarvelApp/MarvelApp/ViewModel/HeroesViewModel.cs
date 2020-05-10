using Acr.UserDialogs;
using MarvelApp.ApiContracts;
using MarvelApp.Model.Navigation;
using MarvelApp.ServicesContracts;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using MarvelApp.ViewModel.Base;
using MarvelApp.Model;
using System.Collections.ObjectModel;
using MarvelApp.Exceptions;
using System.Linq;
using Xamarin.Forms.Internals;
using System.Text;

namespace MarvelApp.ViewModel
{
    public class HeroesViewModel : BaseViewModel
    {
        public string HeaderTitle => GetHeaderPage();
        public string HeaderTitleComplement => "Busca Marvel";        

        public bool ShowBackIcon => false;

        private const int PageSize = 6;

        private PageData currentPage;
        private Command<ItemTappedEventArgs> tapCommand;
        private Command loadMoreCommand;
        private Command searchCommand;
        private ObservableCollection<Character> characters;
        private string searchText;
        private bool existemNovosRegistros;
        private bool isLoadingMore;
        private readonly IMavelApiCaller mavelApiCaller;

        public PageData CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        public Command<ItemTappedEventArgs> TapCommand
        {
            get => tapCommand;
            set => SetProperty(ref tapCommand, value);
        }

        public Command LoadMoreCommand
        {
            get => loadMoreCommand;
            set => SetProperty(ref loadMoreCommand, value);
        }

        public Command SearchCommand
        {
            get => searchCommand;
            set => SetProperty(ref searchCommand, value);
        }

        public ObservableCollection<Character> Characters
        {
            get => characters;
            set => SetProperty(ref characters, value);
        }

        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        public bool IsLoadingMore
        {
            get => isLoadingMore;
            set => SetProperty(ref isLoadingMore, value);
        }

        private Func<bool> CanLoadMore()
        {
            return new Func<bool>(() => existemNovosRegistros);
        }

        public HeroesViewModel(IMavelApiCaller mavelApiCaller, INavigationService navigationService, IPopPupDialogService popPupDialogService, IUserDialogs userDialogs, ILiteDbService liteDbService, IApiManager apiManager, ISecureStorageService secureStorageService) : base(navigationService, popPupDialogService, userDialogs, liteDbService, apiManager, secureStorageService)
        {
            Characters = new ObservableCollection<Character>();

            this.mavelApiCaller = mavelApiCaller;

            LoadMoreCommand = new Command(async () =>
            {
                try
                {
                    IsLoadingMore = true;

                    CurrentPage = CurrentPage.ToNextPage();
                    await GetCharacteres(CurrentPage, SearchText);
                }
                catch (SemConexaoException sex)
                {
                    await PopPupDialogService.AlertModalAsync("Atenção", sex.Message);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    await PopPupDialogService.AlertModalAsync("Atenção", ex.Message);
                }
                finally
                {
                    IsLoadingMore = false;
                }
            }, CanLoadMore());

            SearchCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    CurrentPage = DefaultPage();
                    Characters = await FiltrarPersonagens(CurrentPage, SearchText);
                }
                catch (SemConexaoException sex)
                {
                    await PopPupDialogService.AlertModalAsync("Atenção", sex.Message);
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
            });

            TapCommand = new Command<ItemTappedEventArgs>(async (ItemTappedEventArgs eventArgs) =>
            {
                try
                {
                    if (eventArgs.Item is Character character)
                    {
                        var parameters = new NavigationParameters(NavigationParameterHandle.HandleSelectedCharacter, character);
                        await NavigationService.NavigateToAsync<HeroViewModel>(parameters);
                        return;
                    }
                    throw new Exception("Item selecionado não é um herói");
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    await PopPupDialogService.AlertModalAsync("Atenção", "Não foi possível selecionar o personagem.");
                }
            });
        }

        private async Task GetCharacteres(PageData currentPage, string filtro = null)
        {
            try
            {
                if (HasConnection() && existemNovosRegistros)
                {
                    var result = await FiltrarPersonagens(currentPage, filtro);
                    if (result != null && result.Any())
                    {
                        result.ForEach(character => Characters.Add(character));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível carregar os personagens", ex);
            }
        }

        private async Task<ObservableCollection<Character>> FiltrarPersonagens(PageData currentPage, string filtro = null)
        {
            var heroes = new ObservableCollection<Character>();
            try
            {
                var result = await mavelApiCaller.GetPersonagens(currentPage, filtro);
                if (result.Results.Any())
                {
                    existemNovosRegistros = true;
                    heroes = new ObservableCollection<Character>(result.Results);
                }
                else
                {
                    CurrentPage = DefaultPage();
                    existemNovosRegistros = false;
                }
            }
            catch (SemConexaoException) { }
            catch (Exception ex)
            {
                Exception newException = new Exception("Problemas ao buscar dados dos personagens.", ex);
                newException.LogException();
            }
            return heroes;
        }

        public override async Task OnInitialize()
        {
            try
            {
                await OnLoadPageData();
            }
            catch (SemConexaoException) { }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public override async Task OnLoadPageData()
        {
            CurrentPage = DefaultPage();
            Characters = await FiltrarPersonagens(CurrentPage);
        }

        private PageData DefaultPage() => new PageData(1, PageSize);

        private string GetHeaderPage()
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append("Olá ");

            var currentUser = LiteDbService.FirstOrDefaut<CurrentUser>();
            if (currentUser != null)
            {
                sbMessage.Append($"{currentUser.Name} ");
            }
            return sbMessage.ToString();
        }
    }
}
