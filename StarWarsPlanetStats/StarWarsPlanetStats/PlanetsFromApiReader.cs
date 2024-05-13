using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StarWarsPlanetStats
{
    internal class PlanetsFromApiReader : IPlanetsReader
    {
        //public async Task<IEnumerable<Program.Planet>> Read()
        //{
        //    string? json = null;

        //    try
        //    {
        //        var jsonTask = _apiDataReader.Read(baseAddress, requestUri);
        //        json = jsonTask.Result;
        //    }
        //    // We will use this if the API is down.
        //    //IApiDataReader apiDataReader = new MockStarWarsApiDataReader();
        //    catch (HttpRequestException ex)
        //    {
        //        Console.WriteLine("API request was unsuccessful switching to mock API data.\n" +
        //                           $"Exception message is: {ex.Message}");

        //        const string ExpectedBaseAddress = "https://swapi.dev/";
        //        const string ExpectedRequestUrl = "api/planets/";
        //        var jsonTask = _mockApiDataReader.Read(ExpectedBaseAddress, ExpectedRequestUrl);
        //        json = jsonTask.Result;
        //    }
        //    Root? root = JsonSerializer.Deserialize<Root>(json);


        //    return ToPlanets(root);
        //}
        public Task<IEnumerable<Program.Planet>> Read()
        {
            throw new NotImplementedException();
        }
    }
}
