using System;
using System.Collections.Generic;

namespace PokemanDetailsDell.Models
{
    public class PokemonTypeModel
    {
        public class Result
        {
            public string name { get; set; }
            public string url { get; set; }
        }

        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Result> results { get; set; }
        
    }
}
