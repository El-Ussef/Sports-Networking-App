using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.UpdateProfile;

public class UpdateSocialMediaCommand: IRequest<Result<bool,ValidationFailed>>
{
    public string RefId { get; set; }

    public SocialMediaDto Data { get; set; }

    class UpdateSocialMediaCommandHandler : IRequestHandler<UpdateSocialMediaCommand, Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        
        public UpdateSocialMediaCommandHandler(
            IApplicationDbContext context
        )
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(UpdateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId.ToString() == request.RefId,cancellationToken);
            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }
            
            var data = request.Data;

            if (!string.IsNullOrEmpty(data.Facebook))
            {
                user.FacebookLink = data.Facebook;
            }
            
            if (!string.IsNullOrEmpty(data.Instagram))
            {
                user.InstagramLink = data.Instagram;
            }
            
            if (!string.IsNullOrEmpty(data.LinkedIn))
            {
                user.LinkedInLink = data.LinkedIn;
            }
            
            if (!string.IsNullOrEmpty(data.Youtube))
            {
                user.YoutubeLink = data.Youtube;
            }
            
            _context.AppUsers.Update(user);
                
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }

}