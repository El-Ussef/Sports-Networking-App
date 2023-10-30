using Domain.Common;

namespace Domain.Entities;


public class Event : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public List<Athlete> Participants { get; set; }
}