namespace Domain.Entities;

public class Sponsor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime DateCreated { get; set; }
    
    public string Name { get; set; }
    public string CompanyName { get; set; }
}