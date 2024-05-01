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
                foreach(Planet planet in planets)
                {
                    Console.WriteLine(planet);
                }
                Console.WriteLine("\nWhich property would you like to see?");
                Console.WriteLine("population");
                Console.WriteLine("diameter");
                Console.WriteLine("surface water");

                string userChoice = Console.ReadLine();

                if(userChoice == "population")
                {
                    var planetWithMaximumPopulation = planets.MaxBy(p => p.Population);
                    Console.WriteLine($"Maximum population is on {planetWithMaximumPopulation.Name}" +
                        $"with a population of {planetWithMaximumPopulation.Population}.");

                    var planetWithMinimumPopulation = planets.MinBy(p => p.Population);
                    Console.WriteLine($"Minimum population is on {planetWithMinimumPopulation.Name}" +
                        $"with a population of {planetWithMinimumPopulation.Population}.");
                }
                else if (userChoice == "diameter")
                {
                    var planetWithMaximumDiameter = planets.MaxBy(p => p.Diameter);
                    Console.WriteLine($"Maximum diameter is in {planetWithMaximumDiameter.Name}" +
                        $"with a diameter of {planetWithMaximumDiameter.Diameter}.");

                    var planetWithMinimumDiameter = planets.MinBy(p => p.Diameter);
                    Console.WriteLine($"minimum diameter is in {planetWithMinimumDiameter.Name}" +
                        $"with a diameter of {planetWithMinimumDiameter.Diameter}.");
                }
                else if (userChoice == "surface water")
                {
                    var planetWithMaximumSurfaceWater = planets.MaxBy(p => p.SurfaceWater);
                    Console.WriteLine($"Maximum surface water is on {planetWithMaximumSurfaceWater.Name}" +
                        $"with a surface water of {planetWithMaximumSurfaceWater.SurfaceWater}.");

                    var planetWithMinimumSurfaceWater = planets.MinBy(p => p.SurfaceWater);
                    Console.WriteLine($"Mainimum surface water is on {planetWithMinimumSurfaceWater.Name}" +
                        $"with a surface water of {planetWithMinimumSurfaceWater.SurfaceWater}.");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }

            }

            private IEnumerable<Planet> ToPlanets(Root? root)
            {
                if(root == null)
                {
                    return Enumerable.Empty<Planet>();
                }
                List<Planet> planets = new List<Planet>();
                foreach(var planetDto in root.results) 
                {
                    Planet planet = (Planet)planetDto; // convert to planet somehow
                    planets.Add(planet);
                }
                return planets;
            }
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

            public static explicit operator Planet(Result planetDto)
            {
                var name = planetDto.name;

                int? diameter = planetDto.diameter.ToIntOrNull();
                int? population = planetDto.population.ToIntOrNull();
                int? surfaceWater = planetDto.surface_water.ToIntOrNull();

                return new Planet(name, diameter, surfaceWater, population);
            }

        }
    }
}