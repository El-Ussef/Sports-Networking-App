using Application.Features.Athletes.UpdateProfile;

namespace Application.Features.Athletes.GetAhtlete;

public class AthleteRespons
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public string ThirdName { get; set; } = string.Empty;

    public string Presentation { get; set; } = string.Empty;
    
    public string? OrganisationName { get; set; }
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string UserType { get; set; }

    public string City { get; set; }
    
    public string Sport { get; set; }
    
    //public string Speciality { get; set; }
    public string JobTitle { get; set; } = string.Empty;

    public string? ProfileImageUrl{ get; set; }

    public List<ServiceDto> Services { get; set; }
    
    public List<AchievementAthleteDto> Achievements { get; set; }

}