using Domain.Common;

namespace Domain.Entities;

public class Location : BaseEntity
{
    public string LocationName { get; set; } = string.Empty;

    public string? Address { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public OpeningHours OpeningHours { get; set; }

    public List<string>? LocationImages { get; set; } = new();
    
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

public class OpeningHours
{
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }

    public override string ToString()
    {
        return $"{Start:hh\\:mm tt} - {End:hh\\:mm tt}";
    }
}