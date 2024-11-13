using Domain.Enums;

namespace CoreApi.Models;

public class UserRegistrationDto
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    // public string? ThirdName { get; set; } = string.Empty;
    
    public string? OrganisationName { get; set; }
    
    public DateTime? Birthdate { get; set; }

    public string? PhoneNumber { get; set; } = string.Empty;
    
    // public string? Height { get; set; } = string.Empty;
    //
    // public string? Weight { get; set; } = string.Empty;
    
    public CoreEnums.Gender? Gender { get; set; }

    public int? Experience { get; set; } //Athletes,Medical and Health,Coach

    public string? CareerStart { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    //public UserType UserType { get; set; }
    public int CityId { get; set; }
    public int? SportId { get; set; }
    public int? SpecialityId { get; set; } //Athletes,Medical and Health,Coach
    public string? JobTitle { get; set; } = string.Empty;
    
    public string? Address { get; set; } = string.Empty;
    
    public IFormFile? ProfilePhoto { get; set; }
    
    public IFormFile? PhotoOne { get; set; } 
    
    public IFormFile? PhotoTwo { get; set; } 
}