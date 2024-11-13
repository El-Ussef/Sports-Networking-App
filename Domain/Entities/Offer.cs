using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// represent the offer that each Type of user can do (not used for now)
/// </summary>
public class Offer : BaseEntity
{
    //TODO: i should have a user class instead of Athlete
    public Athlete Athlete { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime OfferStartDate { get; set; }
    public OfferType OfferType { get; set; }
    public DateTime OfferEndDate { get; set; }
    public List<string> TermsAndConditions { get; set; }
}