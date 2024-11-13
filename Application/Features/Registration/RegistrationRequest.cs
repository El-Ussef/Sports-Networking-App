using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Registration;

public class RegistrationRequest
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    // public string? ThirdName { get; set; } = string.Empty;
    //
    // public string? Height { get; set; } = string.Empty;
    //
    // public string? Weight { get; set; } = string.Empty;
    
    public string? CareerStart { get; set; } = string.Empty;
    
    public DateTime? Birthdate { get; set; }

    public string? OrganisationName { get; set; }
    
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    //public string? ProfilePicturePath { get; set; }

    //public UserType UserType { get; set; }
    public int CityId { get; set; }
    public int? SportId { get; set; }
    public int? SpecialityId { get; set; } //Athletes,Medical and Health,Coach
    public string? JobTitle { get; set; } = string.Empty;
    
    public string? Address { get; set; } = string.Empty;
    
    public CoreEnums.Gender? Gender { get; set; }

    public int? Experience { get; set; } 
    public FileUpload? ProfilePhoto { get; set; }

    public FileUpload? PhotoOne { get; set; } 
    
    public FileUpload? PhotoTwo { get; set; } 
}