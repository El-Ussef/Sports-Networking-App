namespace Domain.Entities;

public class Coach : User
{
    public Sport Sport { get; set; }
    public string Expertise { get; set; }
    public List<string> Certifications { get; set; }
    
    public List<Achievement> Achievements { get; set; }
    public List<TrainingOffer> Offers { get; set; }
    public List<Document> Documents { get; set; }
}