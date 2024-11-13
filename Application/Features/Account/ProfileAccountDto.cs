namespace Application.Features.Account;

public class ProfileAccountDto
{
    public int Id { get; set; }

    public Guid Ref { get; set; }

    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string ThirdName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? ProfilePicturePath { get; set; }

    public string? CoverImagePath { get; set; }

    public string? Height { get; set; } = string.Empty;
    
    public string? Weight { get; set; } = string.Empty;
    
    public string? CareerStart { get; set; } = string.Empty;

    public DateTime? Birthdate { get; set; }

    public string Presentation { get; set; } = string.Empty;

    public string? JobTitle { get; set; } = string.Empty;

    public string? Facebook { get; set; } = string.Empty;

    public string? Instagram { get; set; } = string.Empty;

    public string? LinkedIn { get; set; } = string.Empty;

    public string? Youtube { get; set; } = string.Empty;
    
    public int? SportId { get; set; }
    
    public int? SpecialityId { get; set; }
    
    public int? NationalityId { get; set; }
    
    public int? CityId { get; set; }
}