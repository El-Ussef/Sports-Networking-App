using Domain.Enums;

namespace Domain.Entities;

public class Offer
{
    //TODO: i should have a user class instead of Athlete
    public int Id { get; set; }
    public Athlete Athlete { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime OfferStartDate { get; set; }
    public OfferType OfferType { get; set; }
    public DateTime OfferEndDate { get; set; }
    public List<string> TermsAndConditions { get; set; }
}