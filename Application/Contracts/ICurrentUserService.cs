namespace Application.Contracts;

public interface ICurrentUserService
{
    string UserId { get; }
    Guid StorageAreaId { get; }
    public string UserName { get; }
    List<string> RoleNames { get; }

    public bool IsLoggedIn() => string.IsNullOrEmpty(UserId);
}