using Domain.Enums;

namespace Application.Contracts;

public interface ICurrentUserService
{
    public string UserId { get; set; }
    
    public string Id { get; set; }
    //Guid StorageAreaId { get; }
    public string UserName { get; set; }
    public string UserType { get; set; }

    public bool IsAuthenticated { get; set; }
    List<string> RoleNames { get; }

    public bool IsLoggedIn() => string.IsNullOrEmpty(UserId);
}