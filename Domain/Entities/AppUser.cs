using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class AppUser : BaseEntity
{
    public Guid RefId { get; set; }
    
    #region MyRegion
    
    
    #endregion
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    // 
    public string? Height { get; set; } = string.Empty;
    
    public string? Weight { get; set; } = string.Empty;
    
    public string? CareerStart { get; set; } = string.Empty;
    
    public string? ThirdName { get; set; } = string.Empty;

    [NotMapped] public int Age => this.Birthdate.HasValue ? DateTime.UtcNow.Year - this.Birthdate.Value.Year : 0;

    public DateTime? Birthdate { get; set; }

    //this for the the small presentation of the what the user does
    public string? Presentation { get; set; } = string.Empty;

    //this for clubs, and other like that 
    public string? OrganisationName { get; set; }

    public string? FacebookLink { get; set; } = string.Empty;

    public string? InstagramLink { get; set; } = string.Empty;

    public string? LinkedInLink { get; set; } = string.Empty;
    
    public string? YoutubeLink { get; set; } = string.Empty;
    
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public int? Ranking { get; set; }
    
    //public string? ProfilePicturePath { get; set; }

    public string? CoverImagePath { get; set; }

    public int? NationalityId { get; set; }

    public string Email { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public City City { get; set; } 
    public int CityId { get; set; }
    public Sport? Sport { get; set; } 
    public int? SportId { get; set; }
    
    public Speciality? Speciality { get; set; } //Athletes,Medical and Health,Coach
    public int? SpecialityId { get; set; }
    public string? JobTitle { get; set; } = string.Empty; // this for when we have medicalstaff so that he can specify the job

    public string? Address { get; set; } = string.Empty; 

    public int? Experience { get; set; } 

    public List<Service> Services { get; set; }
    
    
    // other members 
    #region Public info for sponsorship and collabes
    
    public CoreEnums.Gender Gender { get; set; }
    /// <summary>
    /// Asian,Latin,Mestizo,white,Hindu,Black,Arab
    /// </summary>
    public string? Ethnicity { get; set; } = string.Empty;

    /// <summary>
    /// over 10k
    /// </summary>
    public bool? HasMoreFollowers { get; set; } = false;
    public string? Notes { get; set; } = string.Empty;
    
    public string? TopSize { get; set; } = string.Empty;
    public string? BottomSize { get; set; } = string.Empty;
    public string? FootWear { get; set; } = string.Empty;
    public string? DrivingLicense { get; set; } = string.Empty;
    
    #endregion

    #region Admin info
    /// <summary>
    /// DNI,NIE
    /// </summary>
    public string? PhotoDocsFace { get; set; } = string.Empty;
    public string? PhotoDocsBack { get; set; } = string.Empty;
    
    /// <summary>
    /// DNI,NIE,Passport
    /// </summary>
    public string? IdentityPaper { get; set; } = string.Empty;
    
    public string? Iban { get; set; } = string.Empty;
    
    public string? SocialSecurityNumber { get; set; } = string.Empty;

    #endregion

    #region Photos

    public string? ProfilePicturePath { get; set; } = string.Empty;
    public string? PhotoOne { get; set; } = string.Empty;
    public string? PhotoTwo { get; set; } = string.Empty;
    

    #endregion
    
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}

