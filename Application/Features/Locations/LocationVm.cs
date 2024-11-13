namespace Application.Features.Locations;

public class LocationVm
{
    public int Id { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string OpeningHoursStart { get; set; } = string.Empty;
    public string OpeningHoursEnd { get; set; } = string.Empty;
    public List<string>? LocationImages { get; set; }

}