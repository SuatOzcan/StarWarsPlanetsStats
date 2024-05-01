using StarWarsPlanetStats.ApiDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StarWarsPlanetStats.ApiDataAccess.StarWarsApiDataReader;

namespace StarWarsPlanetStats.ApiDataAccess
{
    internal class StarWarsApiDataReader : IApiDataReader
    { 
            public async Task<string> Read(string baseAddress, string requestUri)
            {
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);
                HttpResponseMessage response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                response.Dispose();
                return json;
            } 
    }
}
