using Application.Features.Profile;

namespace Application.Features.Services;

public class UpdateServiceDto
{
    public Guid RefId { get; set; }

    public List<ServiceDto> Services { get; set; }
    // public int ServiceId { get; set; }
    //
    // public string? Label { get; set; }
}
