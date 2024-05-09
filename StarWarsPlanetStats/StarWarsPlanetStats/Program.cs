using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System.Collections.Generic;
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

                Dictionary<string, Func<Planet, int?>> propertyNameToSelectorsMapping =
                    new Dictionary<string, Func<Planet, int?>>
                    {
                        ["population"] = planet => planet.Population,
                        ["diameter"] = planet => planet.Diameter,
                        ["surface water"] = planet => planet.SurfaceWater
                    };

                Console.WriteLine("\nWhich property would you like to see?");
                Console.WriteLine(string.Join(Environment.NewLine,propertyNameToSelectorsMapping.Keys));
                string? userChoice = Console.ReadLine();
                if(userChoice is null || !propertyNameToSelectorsMapping.ContainsKey(userChoice))
                {
                    Console.WriteLine("Invalid choice.");
                }
                else
                {
                    ShowStatistics(planets, userChoice, propertyNameToSelectorsMapping[userChoice]);
                }
            }

            private static void ShowStatistics(IEnumerable<Planet> planets, string propertyName, 
                                        Func<Planet, int?> propertySelector)
            {
                var planetWithMaximumProperty = planets.MaxBy(propertySelector);

                DisplayMinMax(planetWithMaximumProperty, propertyName, propertySelector, "Maximum");
                //Console.WriteLine($"Maximum {propertyName} is on {planetWithMaximumProperty.Name}" +
                //    $"with a population of {propertySelector(planetWithMaximumProperty)}.");

                var planetWithMinimumProperty = planets.MinBy(propertySelector);
                DisplayMinMax(planetWithMinimumProperty, propertyName, propertySelector, "Minimum");
                //Console.WriteLine($"Minimum {propertyName} is on {planetWithMinimumProperty.Name}" +
                //    $" with a population of {propertySelector(planetWithMinimumProperty)}.");
            }

            private static void DisplayMinMax(Planet planet, string propertyName,
                                    Func<Planet, int?> propertySelector, string minOrMax)
            {
                Console.WriteLine($"{minOrMax} {propertyName} is on {planet.Name}" +
                    $" with a {propertyName} of {propertySelector(planet)}.");
            }

            private static IEnumerable<Planet> ToPlanets(Root? root)
            {
                if(root == null)
                {
                    return Enumerable.Empty<Planet>();
                }
                //List<Planet> planets = new List<Planet>();
                //foreach(var planetDto in root.results) 
                //{
                //    Planet planet = (Planet)planetDto; // convert to planet somehow
                //    planets.Add(planet);
                //}
                //return planets;

                return root.results.Select(result => (Planet) result);
            }
        }
        public readonly record struct Planet
        {
            public string Name { get; }
            public int? Diameter { get; }
            public int? SurfaceWater { get; }
            public int? Population { get; }
            public string? Gravity { get; }
            public string? Terrain { get;}

            public Planet(string name, int? diameter, int? surfaceWater, int? population, 
                            string? gravity, string? terrain)
            {
                this.Name = name;
                this.Diameter = diameter;
                this.SurfaceWater = surfaceWater;
                this.Population = population;
                this.Gravity = gravity;
                this.Terrain = terrain;
            }

            public static explicit operator Planet(Result planetDto)
            {
                var name = planetDto.name;

                int? diameter = planetDto.diameter.ToIntOrNull();
                int? population = planetDto.population.ToIntOrNull();
                int? surfaceWater = planetDto.surface_water.ToIntOrNull();
                string? gravity = planetDto.gravity;
                string? terrain = planetDto.terrain;

                return new Planet(name, diameter, surfaceWater, population, gravity, terrain);
            }

        }
    }
}