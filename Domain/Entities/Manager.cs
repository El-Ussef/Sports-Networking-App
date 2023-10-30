namespace Domain.Entities;

public class Manager : User
{
    public string Organization { get; set; }
    public string Category { get; set; }
    public List<AthleteDocument> Documents { get; set; }
}