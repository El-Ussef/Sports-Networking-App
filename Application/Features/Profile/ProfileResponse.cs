namespace Application.Features.Profile;

public class ProfileResponse
{
    public int Id { get; set; }

    public Guid Ref { get; set; }

    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string ThirdName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Photo { get; set; } = string.Empty;

    public string Speciality { get; set; } = string.Empty;
    
    public DateTime? Birthdate { get; set; }

    public string Presentation { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string Facebook { get; set; } = string.Empty;

    public string Instagram { get; set; } = string.Empty;

    public string LinkedIn { get; set; } = string.Empty;

    public string Youtube { get; set; } = string.Empty;
    
    public int? SportId { get; set; }
    
    public int? SpecialityId { get; set; }
    
    public int CityId { get; set; }

    //public List<s>? ServiceTypesIds { get; set; }

    public List<ServiceResponse>? Services { get; set; }
    
    public List<AchievementResponse> Achievements { get; set; } = new ();
}

public class ServiceResponse
{
    public int Id { get; set; }

    public int? ServiceTypeId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;

    public double? Price { get; set; }
    
    public string? Duration { get; set; } = string.Empty;
    //public string Description { get; set; } = string.Empty;
    
}

public class AchievementResponse
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public DateTime DateAchieved { get; set; }
}

public class ImageGalleryResponse
{
    public string? ProfilePhoto { get; set; }

    public string? CoverImage { get; set; }
    
    public string? PhotoOne { get; set; } 
    
    public string? PhotoTwo { get; set; }
}