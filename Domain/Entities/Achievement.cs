using Domain.Common;

namespace Domain.Entities;

public class Achievement : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateAchieved { get; set; }
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; }
}