using Domain.Common;

namespace Domain.Entities;

public class TrainingSession : BaseEntity
{
    public int TrainingSessionId { get; set; }
    public int? UserId { get; set; } // This can be null for anonymous bookings
    //public User User { get; set; }
    public int CoachId { get; set; }
    public Coach Coach { get; set; }
    public DateTime SessionDate { get; set; }
}