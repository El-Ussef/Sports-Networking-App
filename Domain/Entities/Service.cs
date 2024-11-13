using Domain.Common;

namespace Domain.Entities;

public class Service : BaseEntity
{
    public int? ServiceTypeId { get; set; }
    public ServiceType? ServiceType { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;

    public double? Price { get; set; }
    
    public string? Duration { get; set; } = string.Empty;

    //public string? CustomService  { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}