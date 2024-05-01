namespace StarWarsPlanetStats.DataTransferObjects
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);


    public record Root(
                         int count,
                         string next,
                         object previous,
                         IReadOnlyList<Result> results
                            );
}
