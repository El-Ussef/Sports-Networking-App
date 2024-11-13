using Application.Contracts;
using Application.Features.Profile;
using Application.Features.Services;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Achievements;

public class GetAchievementByUserIdQuery: IRequest<Result<Achievement,ValidationFailed>>
{
    public Guid UserId { get; set; }
    
    public int Id { get; set; }
    
    public class GetAchievementByUserIdQueryHandler : IRequestHandler<GetAchievementByUserIdQuery,
        Result<Achievement, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        
        public GetAchievementByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Achievement, ValidationFailed>> Handle(GetAchievementByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Where(u => u.RefId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (user is null || user.UserType is not UserType.Athlete)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<Achievement, ValidationFailed>(new ValidationFailed(failure));
            }

            var achievement = await _context.Achievements
                .FirstOrDefaultAsync(a => a.AthleteId == user.Id && a.Id == request.Id, cancellationToken);

            return achievement;
        }
    }
}