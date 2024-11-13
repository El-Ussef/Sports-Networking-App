using Application.Features.Athletes;

namespace CoreApi.Models;

public class UserUpdateDto
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

    public IFormFile? CoverImage { get; set; }
    
    public IFormFile? ProfilePicture { get; set; }

    
}