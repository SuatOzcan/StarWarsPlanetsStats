using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System.Text.Json;

namespace StarWarsPlanetStats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ExpectedBaseAddress = "https://swapi.dev/";
            const string ExpectedRequestUri = "api/planets/";
            // We will use this if the API is down.
            //IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
            IApiDataReader apiDataReader = new StarWarsApiDataReader();
            var jsonTask = apiDataReader.Read(ExpectedBaseAddress, ExpectedRequestUri);
            var json = jsonTask.Result;
            Root root = JsonSerializer.Deserialize<Root>(json);
            Console.ReadKey();
        }
    }
}