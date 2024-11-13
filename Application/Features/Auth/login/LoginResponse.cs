using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Features.Auth.login;

public class LoginResponse
{
    public bool Success { get; set; }
    
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }

    public User User { get; set; }
    public LoginResponse()
    {
        Success = AccessToken != string.Empty;
    }
}

public class User
{
    public Guid Id { get; set; }

    public int Seq { get; set; } // the real id 
    public string Type { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public string Photo { get; set; }
}