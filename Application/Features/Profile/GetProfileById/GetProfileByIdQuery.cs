using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.GetProfileById;

public class GetProfileByIdQuery : IRequest<Result<ProfileResponse,ValidationFailed>>
{
    public string Type { get; set; }

    public Guid RefId { get; set; }
    
    public class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery,Result<ProfileResponse,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetProfileByIdQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<ProfileResponse, ValidationFailed>> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<UserType>(request.Type, true, out var userTypeEnum))
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user type");
                return new Result<ProfileResponse, ValidationFailed>(new ValidationFailed(failure));
            }

            var user = await _context.AppUsers
                .Include(a=>a.Speciality)
                .Where(u => u.UserType == userTypeEnum && u.RefId == request.RefId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<ProfileResponse, ValidationFailed>(new ValidationFailed(failure));
            }

            ProfileResponse result = new ProfileResponse()
            {
                CityId = user.CityId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ThirdName = user.ThirdName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Speciality = user.Speciality.Label,
                SpecialityId = user.SpecialityId,
                Photo = user.ProfilePicturePath,
                Ref = user.RefId,
                SportId = user.SportId,
                JobTitle = user.JobTitle,
                Birthdate = user.Birthdate,
                Presentation = user.Presentation,
                Instagram = user.InstagramLink,
                Facebook = user.FacebookLink,
                LinkedIn = user.LinkedInLink,
                Youtube = user.YoutubeLink
                
            };

            result.Achievements = await _context.Achievements
                .Where(a => a.AthleteId == user.Id)
                .Select(a=> new AchievementResponse()
                {
                    Title = a.Title,
                    DateAchieved = a.DateAchieved,
                    Description = a.Description
                })
                .ToListAsync(cancellationToken);
            
            result.Services = await _context.Services
                .Where(a => a.AppUserId == user.Id)
                .Select(a=> new ServiceResponse()
                {
                    Id = a.Id,
                    Title = a.Title,
                    ServiceTypeId = a.ServiceType != null? a.ServiceType.Id : null,
                    Duration = a.Duration,
                    Description = a.Description
                })
                .ToListAsync(cancellationToken);
            return result;
        }
    }
}