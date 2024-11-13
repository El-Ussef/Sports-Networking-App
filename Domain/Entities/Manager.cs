namespace Domain.Entities;

public class Manager : AppUser
{
    public string Organization { get; set; }
    public string Category { get; set; }
    public List<AthleteDocument> Documents { get; set; }
}