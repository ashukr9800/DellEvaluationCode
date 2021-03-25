using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PokemanDetailsDell.CommonFiles;
using PokemanDetailsDell.Models;
using Xamarin.Forms;
using static PokemanDetailsDell.Models.Pokemon;
using static PokemanDetailsDell.Models.PokemonDetailsModel;

namespace PokemanDetailsDell.ViewModels
{
    public class PokemanListPageViewModel : INotifyPropertyChanged
    {
        HttpClient httpClient;
        public PokemanListPageViewModel()
        {
            httpClient = new HttpClient();
            PokemonDetailsCommand = new Command(PokemonDetailsCommandHandler);
            CancelPopUpCommand = new Command(CancelPopUpCommandHandler);
            ReloadPokemonDetails = new Command(ReloadPokemonDetailsHandler);
        }

        #region Commands
        public ICommand PokemonDetailsCommand { get; set; }
        public ICommand CancelPopUpCommand { get; set; }
        public ICommand ReloadPokemonDetails { get; set; }
        
        #endregion

        #region Private Properties
        private List<Result> _pokemonDetailList;
        public List<Result> PokemonDetailsList
        {
            get
            {
                return _pokemonDetailList;
            }
            set
            {
                _pokemonDetailList = value;
                OnPropertyChanged(nameof(this.PokemonDetailsList));
            }
        }

        private ObservableCollection<Result> _pokemonDetailCollection;
        public ObservableCollection<Result> PokemonDetailsCollection
        {
            get
            {
                return _pokemonDetailCollection;
            }
            set
            {
                _pokemonDetailCollection = value;
                OnPropertyChanged(nameof(this.PokemonDetailsCollection));
            }
        }

        private ObservableCollection<Result> _pokemonDetailCollectionTemp;
        public ObservableCollection<Result> PokemonDetailsCollectionTemp
        {
            get
            {
                return _pokemonDetailCollectionTemp;
            }
            set
            {
                _pokemonDetailCollectionTemp = value;
                OnPropertyChanged(nameof(this.PokemonDetailsCollectionTemp));
            }
        }

        private bool _isPokemonDetailsVisible = false;
        public bool IsPokemonDetailsVisible
        {
            get
            {
                return _isPokemonDetailsVisible;
            }
            set
            {
                _isPokemonDetailsVisible = value;
                OnPropertyChanged(nameof(this.IsPokemonDetailsVisible));
            }
        }

        private bool _isActivityInProgress = false;
        public bool IsActivityInProgress
        {
            get
            {
                return _isActivityInProgress;
            }
            set
            {
                _isActivityInProgress = value;
                OnPropertyChanged(nameof(this.IsActivityInProgress));
            }
        }

        private string _nextURL;
        public string NextURL
        {
            get
            {
                return _nextURL;
            }
            set
            {
                _nextURL = value;
                OnPropertyChanged(nameof(this.NextURL));
            }
        }

        private PokemonDetails _pokemonDetail;
        public PokemonDetails PokemonDetail
        {
            get
            {
                return _pokemonDetail;
            }
            set
            {
                _pokemonDetail = value;
                OnPropertyChanged(nameof(this.PokemonDetail));
            }
        }

        private PokemonTypeModel _pokemonTypes;
        public PokemonTypeModel PokemonTypes
        {
            get
            {
                return _pokemonTypes;
            }
            set
            {
                _pokemonTypes = value;
                OnPropertyChanged(nameof(this.PokemonTypes));
            }
        }

        private PokemonTypeModel.Result _selectedPokemonName;
        public PokemonTypeModel.Result SelectedPokemonName
        {
            get
            {
                return _selectedPokemonName;
            }
            set
            {
                _selectedPokemonName = value;
                if (_selectedPokemonName != null)
                {
                    PokemonDetailsCollection = PokemonDetailsCollectionTemp;
                    FilterPokemonDetails(_selectedPokemonName.name);
                }
                OnPropertyChanged(nameof(this.SelectedPokemonName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        #region Public Functions
        public async void GetPokemonTypes()
        {
            var pokemonTypeResponse = httpClient.GetStringAsync(Constants.BaseURL + Constants.PokemonTypes).GetAwaiter().GetResult();
            PokemonTypes = JsonConvert.DeserializeObject<PokemonTypeModel>(pokemonTypeResponse);
        }

        public async void GetPokemonList()
        {
            IsActivityInProgress = true;

            try
            {
                var PokemonBasicDetailsResponse = httpClient.GetStringAsync(Constants.BaseURL + Constants.PokemonBasicDetails).GetAwaiter().GetResult();
                var pokemonBasicDetails = JsonConvert.DeserializeObject<PokemonModel>(PokemonBasicDetailsResponse);

                NextURL = pokemonBasicDetails.next;

                await AddDetailsToPokemon(pokemonBasicDetails);

                PokemonDetailsList = new List<Result>(pokemonBasicDetails.results);
                PokemonDetailsCollectionTemp = PokemonDetailsCollection = new ObservableCollection<Result>(PokemonDetailsList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.StackTrace);
                IsActivityInProgress = false;
            }

            IsActivityInProgress = false;
        }

        private async Task AddDetailsToPokemon(PokemonModel pokemonBasicDetails)
        {
            if (pokemonBasicDetails != null && pokemonBasicDetails.results != null && pokemonBasicDetails.results.Count > 0)
            {
                foreach (var pokemonitem in pokemonBasicDetails.results)
                {
                    var pokemonID = pokemonitem.url.TrimEnd(pokemonitem.url[pokemonitem.url.Length - 1]);
                    pokemonitem.PokemonID = pokemonID.Substring(pokemonID.LastIndexOf('/') + 1);
                    pokemonitem.url = Constants.PokemonFrontImage + pokemonitem.PokemonID + ".png";
                    await AssignType(pokemonitem);
                }
            }
        }

        public async Task AssignType(Result pokemonitem)
        {
            var PokemonDetailsResponse = httpClient.GetStringAsync(Constants.BaseURL + Constants.PokemonBasicDetails + "/" + pokemonitem.PokemonID).GetAwaiter().GetResult();
            pokemonitem.PokemonItemDetail = JsonConvert.DeserializeObject<PokemonDetails>(PokemonDetailsResponse);
            pokemonitem.PokemonType = pokemonitem.PokemonItemDetail.types.FirstOrDefault().type.name;
        }

        public async void LoadData()
        {
            IsActivityInProgress = true;

            var PokemonNextResponse = httpClient.GetStringAsync(NextURL).GetAwaiter().GetResult();
            var pokemonNextDetails = JsonConvert.DeserializeObject<PokemonModel>(PokemonNextResponse);
            await AddDetailsToPokemon(pokemonNextDetails);

            NextURL = pokemonNextDetails.next;
            PokemonDetailsList.AddRange(pokemonNextDetails.results);
            PokemonDetailsCollectionTemp = PokemonDetailsCollection = new ObservableCollection<Result>(PokemonDetailsList);

            IsActivityInProgress = false;
        }

        #endregion

        #region Private Functions
        private void PokemonDetailsCommandHandler(object obj)
        {
            IsActivityInProgress = true;
            var pokedetails = obj as Result;

            PokemonDetail = PokemonDetailsCollection.FirstOrDefault(x => x.PokemonID == pokedetails.PokemonID).PokemonItemDetail;
            PokemonDetail.PokemonType = PokemonDetail.types.FirstOrDefault().type.name;

            IsActivityInProgress = false;
            IsPokemonDetailsVisible = true;

        }

        private void CancelPopUpCommandHandler(object obj)
        {
            if (IsPokemonDetailsVisible)
                IsPokemonDetailsVisible = false;
        }

        private void FilterPokemonDetails(string name)
        {
            if (!string.IsNullOrEmpty(name) && PokemonDetailsCollection != null && PokemonDetailsCollection.Count > 0)
            {
                PokemonDetailsCollection = new ObservableCollection<Result>(PokemonDetailsList.Where(x => x.PokemonType == name));
            }
        }

        private void ReloadPokemonDetailsHandler(object obj)
        {
            PokemonDetailsCollection = PokemonDetailsCollectionTemp;
            SelectedPokemonName = null;
        }
        #endregion

    }
}
