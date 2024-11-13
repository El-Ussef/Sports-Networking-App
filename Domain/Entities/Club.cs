using Domain.Common;

namespace Domain.Entities;

public class Club : AppUser
{
    public string ClubName { get; set; } = string.Empty;
}