using System.Security.Claims;
using Domain.Entities;

namespace Application.Contracts;

public interface IJwtTokenService
{
    string GenerateJwtToken(AppUser user);
    string GenerateJwtRefreshToken(AppUser user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}