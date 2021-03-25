using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PokemanDetailsDell.ServiceCalls
{
    public class PokemonListCall
    {
        HttpClient client;
        public PokemonListCall()
        {
            client = new HttpClient();
        }

        
    }
}
