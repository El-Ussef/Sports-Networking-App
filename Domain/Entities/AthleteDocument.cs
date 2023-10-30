using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;


public class AthleteDocument : AbstractDocument
{
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; }
}