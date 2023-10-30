using Domain.Enums;

namespace Domain.Entities;

public class Athlete : User
{
    public int Ranking { get; set; }
    public Sport Sport { get; set; }
    public List<Achievement> Achievements { get; set; }
    public List<Sponsorship> Sponsorships { get; set; }
    public List<Offer> Offers { get; set; }
    public List<AthleteDocument> Documents { get; set; }
}