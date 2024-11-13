using Application.Features.Profile;

namespace Application.Features.Services;

public class ServiceDto
{
    public Guid RefId { get; set; }
    
    public int Id { get; set; } = default!;
    
    public int? ServiceTypeId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;

    public double? Price { get; set; }
    
    public string? Duration { get; set; } = string.Empty;

    //not used
    public OperationCode Operation { get; set; }
}

public enum OperationCode
{
    UNCHANGED=0,
    ADDED = 1,
    UPDATED =2,
    DELETED=3,
    
}