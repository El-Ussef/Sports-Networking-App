namespace Domain.Entities;

public class Manager : User
{
    public string Organization { get; set; }
    public List<Document> Documents { get; set; }

}