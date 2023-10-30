using Domain.Common;

namespace Domain.Entities;

public class Rating : BaseEntity
{
    public int UserId { get; set; }
    //public User User { get; set; }
    public int RatedById { get; set; }
    public string RatedBy { get; set; }
    public int? OfferId { get; set; }
    public double Score { get; set; }
    public string Comment { get; set; }
    public DateTime DateRated { get; set; }
}