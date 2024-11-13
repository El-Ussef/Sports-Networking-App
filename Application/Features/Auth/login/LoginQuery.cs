 using Application.Common.Options;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.login;

public class LoginQuery : IRequest< Result<LoginResponse,ValidationFailed>>
{
    public string Login { get; set; }

    public string Password { get; set; }
    
    class LoginQueryHandler : IRequestHandler<LoginQuery,Result<LoginResponse,ValidationFailed>>
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenGenerator;
        private readonly IOptions<StorageOption> _storageOptions;

        public LoginQueryHandler(IIdentityService identityService,
            IApplicationDbContext context,
            IJwtTokenService jwtTokenGenerator,
            IOptions<StorageOption> storageOptions)
        {
            _identityService = identityService;
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _storageOptions = storageOptions;
        }
        public async Task<Result<LoginResponse,ValidationFailed>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.CheckUserCredentials(request.Login,request.Password);

            if (userId is not null)
            {
                var user = await _context.AppUsers.Where(u => u.RefId.ToString().Equals(userId)).FirstOrDefaultAsync(cancellationToken);

                if (user is not null)
                {
                    var token = _jwtTokenGenerator.GenerateJwtToken(user);
                    var refreshToken = _jwtTokenGenerator.GenerateJwtRefreshToken(user);
                    
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    _context.AppUsers.Update(user);
                    await _context.SaveChangesAsync(cancellationToken);
                    
                    return new Result<LoginResponse, ValidationFailed>(new LoginResponse()
                    {
                        AccessToken = token,
                        RefreshToken = refreshToken,
                        User = new User
                        {
                            Id = user.RefId,
                            Seq = user.Id,
                            Type = user.UserType.ToString(),
                            Name = user.FirstName,
                            Email = user.Email,
                            Photo = user.ProfilePicturePath
                        },
                        
                    });
                }
            }

            var failure = new ValidationFailure("LoginQuery", "Email or password are not valid");

            return new Result<LoginResponse, ValidationFailed>(new ValidationFailed(failure));
            
        }
    }
}