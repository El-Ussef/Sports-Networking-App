using Application.Contracts;
using Application.Features.Profile;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Achievements;

public class GetAchievementsByUserIdQuery: IRequest<Result<List<AchievementDto>,ValidationFailed>>
{
    public Guid UserId { get; set; }
    
    public class GetAchievementsByUserIdQueryHandler : IRequestHandler<GetAchievementsByUserIdQuery,
        Result<List<AchievementDto>, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        
        public GetAchievementsByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<AchievementDto>, ValidationFailed>> Handle(GetAchievementsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Where(u => u.RefId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (user is null || user.UserType is not UserType.Athlete)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user or user is not an athlete");
                return new Result<List<AchievementDto>, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var achievements = await _context.Achievements
                .Where(a => a.AthleteId == user.Id)
                .Select(a=> new AchievementDto()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    DateAchieved = a.DateAchieved.ToUniversalTime(),
                    Location = a.Location,
                    
                })
                .ToListAsync(cancellationToken);
            
            return achievements;
        }
    }
}