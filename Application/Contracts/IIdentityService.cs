using Application.Features.Registration;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;

namespace Application.Contracts;

public interface IIdentityService
{
    //Task<Result<bool>> DeleteUserAsync(string userId);
    //Task<Result<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    //Task<AppUser> CheckUserCredentials(string username, string password);

    Task<AppUser> GetByUserName(string userName);
    Task<AppUser> GetUserByIdAsync(int userId);
    public Task<string?> CheckUserCredentials(string username, string password);

    //Task<AppUser>? CreateApplicationUserUserAsync(AppUser appuser, string password);
    Task<AppUser> UpdateUserAsync(string userId, AppUser updateduser);
    Task<AppUser> GetUserByEmailAsync(string email);
    Task<List<string>> GetUserClaimesAsync(int userId, string claimType);

    Task<Result<AppUser, ValidationFailed>> RegisterAppUserAsync(RegistrationRequest data, string password);

    Task UpdateAsync(AppUser user);
    Task<string> GeneratePasswordResetTokenAsync(AppUser user);
}