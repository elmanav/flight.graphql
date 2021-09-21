namespace Flight.ImportData.Models
{
    public record Airport(string Id, string Name, string? CityId, string IcaoCode, string? CountryId);

    public record Country(string Id, string Name, string Capital, string Continent);

    public record City(string Id, string Name, string CountryId, string TimeZone)
    {
        public Country Country { get; init; }
    };
}