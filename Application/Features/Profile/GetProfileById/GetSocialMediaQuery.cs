using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.GetProfileById;

public class GetSocialMediaQuery: IRequest<Result<SocialMediaDto,ValidationFailed>>
{
    public string Type { get; set; }
    public string RefId { get; set; }

    class GetSocialMediaQueryHandler : IRequestHandler<GetSocialMediaQuery,Result<SocialMediaDto,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetSocialMediaQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<SocialMediaDto, ValidationFailed>> Handle(GetSocialMediaQuery request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<UserType>(request.Type, true, out var userTypeEnum))
            {
                var failure = new ValidationFailure("GetSocialMediaQuery", "Failed to Find user type");
                return new Result<SocialMediaDto, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var user = await _context.AppUsers
                .Where(u => u.UserType == userTypeEnum && u.RefId.ToString() == request.RefId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                var failure = new ValidationFailure("GetSocialMediaQuery", "Failed to Find user");
                return new Result<SocialMediaDto, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var result = new SocialMediaDto
            {
                Facebook = user.FacebookLink,
                Instagram = user.InstagramLink,
                Youtube = user.YoutubeLink,
                LinkedIn = user.LinkedInLink,
            };
            return result;

        }
    }
}