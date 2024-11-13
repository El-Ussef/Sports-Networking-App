using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Athletes.UpdateProfile;

public class ProfileDto
{
    public string FullName { get; set; } = string.Empty;

    //this for the the small presentation of the what the user does
    public string Presentation { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    
    //public int Ranking { get; set; }

    public string Email { get; set; } = string.Empty;
    
    //public UserType UserType { get; set; }
    
    public int CityId { get; set; }
    
    public int SportId { get; set; }
    
    //public int SpecialityId { get; set; }
    
    public string JobTitle { get; set; } = string.Empty;

    public FileUpload? CoverImage { get; set; }

    public List<int>? ServiceTypesIds { get; set; }

    public List<string>? CustomService { get; set; }
    
    public List<AchievementAthleteDto>? Achievements { get; set; }

}