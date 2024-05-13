using static StarWarsPlanetStats.Program;

namespace StarWarsPlanetStats
{
    internal interface IPlanetsReader
    {
        public Task<IEnumerable<Planet>> Read();
    }
}