using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System.Text.Json;

namespace StarWarsPlanetStats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? json = null;
            const string BaseAddress = "https://swapi.dev/";
            const string RequestUrl = "api/planets/";
            // We will use this if the API is down.
            //IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
            try
            {
                IApiDataReader apiDataReader = new StarWarsApiDataReader();
                var jsonTask = apiDataReader.Read(BaseAddress, RequestUrl);
                json = jsonTask.Result;
            }
            catch(HttpRequestException ex) 
            {
                Console.WriteLine("API request was unsuccessful switching to mock API data.\n"+
                                   $"Exception message is: {ex.Message}");
                IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
                var jsonTask = apiDataReader.Read(BaseAddress, RequestUrl);
                json = jsonTask.Result;
            }
            Root? root = JsonSerializer.Deserialize<Root>(json);
            Console.ReadKey();
        }
    }
}