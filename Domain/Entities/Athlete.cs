using Domain.Enums;

namespace Domain.Entities;

public class Athlete : AppUser
{
    public Sport Sport { get; set; }
    public List<Achievement> Achievements { get; set; }
    //public List<Sponsorship> Sponsorships { get; set; }
    //public List<Offer> Offers { get; set; } should be in AppUser
    public List<AthleteDocument> Documents { get; set; }
}