namespace Domain.Entities;

public class Coach : AppUser
{
    public Sport Sport { get; set; }
    public string Expertise { get; set; }
    public List<string> Certifications { get; set; }
    
    public List<Achievement> Achievements { get; set; }
    public List<TrainingOffer> Offers { get; set; }
    public List<AthleteDocument> Documents { get; set; }
}