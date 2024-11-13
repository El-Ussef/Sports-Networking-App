namespace Domain.Entities;

public class Country
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;
    
    public string CountryCode { get; set; } = string.Empty;

    public List<Region> Regions { get; set; }
    
    public List<City> Cities { get; set; }
}