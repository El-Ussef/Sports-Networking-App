using Application.Common.Models;

namespace Application.Features.Events;

public class CreateEventDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CityId { get; set; }
    public DateTime EventDate { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public FileUpload? ProfilePicture { get; set; }
    
    public List<FileUpload>? EventImages { get; set; }

}

public class EventDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string City { get; set; }

    public DateTime EventDate { get; set; }
    
    public string ProfilePicture { get; set; }
    
}

public class EventDetailDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime EventDate { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public string ProfilePicture { get; set; }
    
    public List<string>? EventImages { get; set; }
}