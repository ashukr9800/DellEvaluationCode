using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using static PokemanDetailsDell.Models.PokemonDetailsModel;

namespace PokemanDetailsDell.Models
{
    public class Pokemon : INotifyPropertyChanged
    {
        public class Result : INotifyPropertyChanged
        {
            public string name { get; set; }
            public string url { get; set; }

            [JsonIgnore]
            private string _pokemonID;
            [JsonIgnore]
            public string PokemonID
            {
                get
                {
                    return _pokemonID;
                }
                set
                {
                    _pokemonID = value;
                    OnPropertyChanged(nameof(PokemonID));
                }
            }

            [JsonIgnore]
            private string _pokemonURL;
            [JsonIgnore]
            public string PokemonURL
            {
                get
                {
                    return _pokemonURL;
                }
                set
                {
                    _pokemonURL = value;
                    OnPropertyChanged(nameof(PokemonURL));
                }
            }

            [JsonIgnore]
            private string _pokemonType;
            [JsonIgnore]
            public string PokemonType
            {
                get
                {
                    return _pokemonType;
                }
                set
                {
                    _pokemonType = value;
                    OnPropertyChanged(nameof(PokemonType));
                }
            }

            [JsonIgnore]
            private PokemonDetails _pokemonItemDetail;
            [JsonIgnore]
            public PokemonDetails PokemonItemDetail
            {
                get
                {
                    return _pokemonItemDetail;
                }
                set
                {
                    _pokemonItemDetail = value;
                    OnPropertyChanged(nameof(this.PokemonItemDetail));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            }
        }

        public class PokemonModel
        {
            public int count { get; set; }
            public string next { get; set; }
            public string previous { get; set; }
            public List<Result> results { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
