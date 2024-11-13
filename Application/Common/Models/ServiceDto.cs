namespace Application.Features.Athletes.UpdateProfile;

public class ServiceDto
{
    public int Id { get; set; }

    public string Type { get; set; }

    public string Description { get; set; } = string.Empty;
}