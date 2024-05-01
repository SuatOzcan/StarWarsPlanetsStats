using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace StarWarsPlanetStats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ExpectedBaseAddress = "https://swapi.dev/";
            const string ExpectedRequestUrl = "api/planets/";
            try 
            { 
                StarWarsPlanetStatsApp app = new StarWarsPlanetStatsApp(new StarWarsApiDataReader(),
                                                                        new MockStarWarsApiDataReader(),
                                                                    ExpectedBaseAddress, ExpectedRequestUrl);
                app.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine( $"An error occured. Exception message is: {ex.Message}.");
            }
            Console.ReadKey();
        }

        public class StarWarsPlanetStatsApp
        {
            string? json = null;
            const string ExpectedBaseAddress = "https://swapi.dev/";
            const string ExpectedRequestUrl = "api/planets/";
            private readonly IApiDataReader _apiDataReader;
            private readonly IApiDataReader _mockApiDataReader;
            private readonly string baseAddress;
            private readonly string requestUri;

            public StarWarsPlanetStatsApp(IApiDataReader _apiDataReader, IApiDataReader _secondApiDataReader,
                string baseAddress, string requestUri)
            {
                this._apiDataReader = _apiDataReader;
                this.baseAddress = baseAddress;
                this.requestUri = requestUri;
                this._mockApiDataReader = _secondApiDataReader;

            }
            public void Run()
            {
                // We will use this if the API is down.
                //IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
                try
                {
                    var jsonTask = _apiDataReader.Read(baseAddress, requestUri);
                    json = jsonTask.Result;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("API request was unsuccessful switching to mock API data.\n" +
                                       $"Exception message is: {ex.Message}");

                    var jsonTask = _mockApiDataReader.Read(ExpectedBaseAddress, ExpectedRequestUrl);
                    json = jsonTask.Result;
                }
                Root? root = JsonSerializer.Deserialize<Root>(json);
            }
        }
    }
}