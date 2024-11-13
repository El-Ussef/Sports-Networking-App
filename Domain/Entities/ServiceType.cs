using Domain.Common;

namespace Domain.Entities;

public class ServiceType: BaseEntity
{
    //here should hold all the servcies type that we can have Trainning, ads, 
    public string Label { get; set; } = string.Empty;
    
}