using Domain.Common;

namespace Domain.Entities;

public class ServiceExample: BaseEntity
{

    public string Label { get; set; } = string.Empty;
    
    public ServiceType? ServiceType { get; set; }

    
}