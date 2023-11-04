using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } 
    public byte[] PasswordSalt { get; set; }
    public Speciality Speciality { get; set; } //Athletes,Medical and Health,Coach
    public string JobTitle { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public DateTime DateCreated { get; set; }
    public City City { get; set; }
}

