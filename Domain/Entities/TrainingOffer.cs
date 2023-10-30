using Domain.Common;

namespace Domain.Entities;

public class TrainingOffer : BaseEntity
{
    public int CoachId { get; set; }
    public Coach Coach { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime OfferStartDate { get; set; }
    public DateTime OfferEndDate { get; set; }
    public List<string> TermsAndConditions { get; set; }
}