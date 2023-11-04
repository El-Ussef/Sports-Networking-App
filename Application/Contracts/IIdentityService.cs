using Domain.Entities;

namespace Application.Contracts;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<User> CheckUserCredentials(string username, string password);
    Task<User> GetUserByIdAsync(string userId);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(string userId, User user);
    //Task<Result<bool>> DeleteUserAsync(string userId);
    //Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<string>> GetUserClaimesAsync(string userId, string claimType);
}