using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace StarWarsPlanetStats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string BaseAddress = "https://swapi.dev/";
            const string RequestUrl = "api/planets/";
            try 
            { 
                StarWarsPlanetStatsApp app = new StarWarsPlanetStatsApp(new StarWarsApiDataReader(),
                                                                        new MockStarWarsApiDataReader());
                app.Run(BaseAddress, RequestUrl);
            }
            catch(Exception ex)
            {
                Console.WriteLine( $"An error occurred. Exception message is: {ex.Message}.");
            }
            Console.ReadKey();
        }

        public class StarWarsPlanetStatsApp
        {
            private readonly IApiDataReader _apiDataReader;
            private readonly IApiDataReader _mockApiDataReader;

            public StarWarsPlanetStatsApp(IApiDataReader apiDataReader, IApiDataReader _secondApiDataReader)
            {
                this._apiDataReader = apiDataReader;
                this._mockApiDataReader = _secondApiDataReader;

            }
            public void Run(string baseAddress, string requestUri)
            {
                string? json = null;
                
                try
                {
                    var jsonTask = _apiDataReader.Read(baseAddress, requestUri);
                    json = jsonTask.Result;
                }
                // We will use this if the API is down.
                //IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("API request was unsuccessful switching to mock API data.\n" +
                                       $"Exception message is: {ex.Message}");

                    const string ExpectedBaseAddress = "https://swapi.dev/";
                    const string ExpectedRequestUrl = "api/planets/";
                    var jsonTask = _mockApiDataReader.Read(ExpectedBaseAddress, ExpectedRequestUrl);
                    json = jsonTask.Result;
                }
                Root? root = JsonSerializer.Deserialize<Root>(json);

                var planets = ToPlanets(root);
            }

            private IEnumerable<Planet> ToPlanets(Root? root)
            {
                if(root == null)
                {
                    return Enumerable.Empty<Planet>();
                }
                throw new NotImplementedException();
            }

            public readonly record struct Planet
            {
                public string Name { get; }
                public int? Diameter { get; }
                public int? SurfaceWater { get; }
                public int? Population { get; }

                public Planet(string name, int? diameter, int? surfaceWater, int? population)
                {
                    this.Name = name;
                    this.Diameter = diameter;
                    this.SurfaceWater = surfaceWater;
                    this.Population = population;
                }
            }
        }
    }
}