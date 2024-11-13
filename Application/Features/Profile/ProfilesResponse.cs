namespace Application.Features.Profile;

public record ProfilesResponse
{
    public int Id { get; set; }

    public string Ref { get; set; }

    public string FullName { get; set; }

    public string? Photo { get; set; }

    //public string UserType { get; set; }
    
    public string Speciality { get; set; }
    
    //public string? Sport { get; set; }


    public string City { get; set; }
}