using Domain.Common;

namespace Domain.Entities;


public class Event : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }
    public DateTime EventDate { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string PhotoPath { get; set; }

    public List<string> EventImages { get; set; } = new List<string>();

    //public List<Athlete> Participants { get; set; }
}