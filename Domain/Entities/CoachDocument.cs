using Domain.Common;

namespace Domain.Entities;

public class CoachDocument : AbstractDocument
{
    public Coach Coach { get; set; }
}