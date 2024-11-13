using Application.Common.Options;
using Application.Contracts;
using Application.Features.Athletes.UpdateProfile;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Features.Athletes.GetAhtlete;

public class GetAthleteQuery : IRequest<Result<AthleteRespons,ValidationFailed>>
{
    public int Id { get; set; }
    
    public class GetAthleteQueryHandler : IRequestHandler<GetAthleteQuery,Result<AthleteRespons,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IOptions<StorageOption> _storageOptions;

        public GetAthleteQueryHandler(IApplicationDbContext context,IOptions<StorageOption> storageOptions)
        {
            _context = context;
            _storageOptions = storageOptions;
        }
        public async Task<Result<AthleteRespons,ValidationFailed>> Handle(GetAthleteQuery request, CancellationToken cancellationToken)
        {
            int id = request.Id;

            var appUser = await _context.AppUsers
                .Where(c => c.UserType == UserType.Athlete && c.Id == id)
                .Include(c=>c.City)
                .Include(c=>c.Sport)
                .FirstOrDefaultAsync(cancellationToken);

            if (appUser is null)
            {
                var failure = new ValidationFailure("GetAthleteQuery", "Failed to Find user");
                return new Result<AthleteRespons, ValidationFailed>(new ValidationFailed(failure));
            }

            var achievements = await _context.Achievements
                .Where(a => a.AthleteId == appUser.Id)
                .Select(a => new AchievementAthleteDto
                {
                    Title = a.Title,
                    Description = a.Description,
                    DateAchieved = a.DateAchieved
                })
                .ToListAsync(cancellationToken);
            var services = await _context.Services
                .Where(s => s.AppUserId == appUser.Id)
                .Select(c=>new ServiceDto
                {
                    Id = c.Id,
                    Type = c.ServiceType.Label,
                    // Description = to add in model
                })
                .ToListAsync(cancellationToken);
            
            var athlete = new AthleteRespons()
            {
                Id=appUser.Id,
                FirstName = appUser.FirstName,
                PhoneNumber = appUser.PhoneNumber,
                OrganisationName = appUser.OrganisationName,
                City = appUser.City.Label,
                Presentation = appUser.Presentation,
                Sport = appUser.Sport.Name,
                Email = appUser.Email,
                JobTitle = appUser.JobTitle,
                UserType = appUser.UserType.ToString(),
                ProfileImageUrl = appUser.ProfilePicturePath,
                Achievements = achievements,
                Services = services
                //Speciality = appUser.Speciality.
            };
            
            return athlete;
        }
    }
}