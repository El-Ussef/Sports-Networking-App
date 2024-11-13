using Application.Common.Models;
using Application.Features.Athletes;

namespace Application.Features.Profile;

public class ProfileRequestDto
{
    public string? FirstName { get; set; } = string.Empty;
    
    public string? LastName { get; set; } = string.Empty;
    
    public string? ThirdName { get; set; } = string.Empty;

    public string? OrganisationName { get; set; } = string.Empty;
    

    //this for the the small presentation of the what the user does
    public string? Presentation { get; set; } = string.Empty;

    public string? CareerStart { get; set; } = string.Empty;
    
    public DateTime? Birthdate { get; set; }

    public string? PhoneNumber { get; set; } = string.Empty;
    
    //public int Ranking { get; set; }

    public string Email { get; set; } = string.Empty;
    
    //public UserType UserType { get; set; }

    public int? CityId { get; set; }
    
    public int? SportId { get; set; }
    
    public int? SpecialityId { get; set; }
    
    public int? NationalityId { get; set; }

    public string? JobTitle { get; set; } = string.Empty;

    public FileUpload? ProfilePicture { get; set; }

    public FileUpload? CoverImage { get; set; }
    
    public FileUpload? PhotoOne { get; set; } 
    
    public FileUpload? PhotoTwo { get; set; } 
    
}

public class PhysicalInformationDto
{
    public string? Height { get; set; } = string.Empty;
    
    public string? Weight { get; set; } = string.Empty;
    
    public string? TopSize { get; set; } = string.Empty;
    
    public string? BottomSize { get; set; } = string.Empty;
    
    public string? FootWear { get; set; } = string.Empty;

}

public class ImageGalleryDto
{
    public FileUpload? ProfilePicture { get; set; }

    public FileUpload? CoverImage { get; set; }
    
    public FileUpload? PhotoOne { get; set; } 
    
    public FileUpload? PhotoTwo { get; set; }

}

public class SocialMediaDto
{
    public string? Facebook { get; set; } = string.Empty;

    public string? Instagram { get; set; } = string.Empty;

    public string? LinkedIn { get; set; } = string.Empty;

    public string? Youtube { get; set; } = string.Empty;
}