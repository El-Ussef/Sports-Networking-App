using Application.Contracts;
using Application.Features.Athletes;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Achievements;

public class CreateAchievementCommand : IRequest<Result<bool,ValidationFailed>>
{
    public AchievementDto Data { get; set; }
    
    class CreateAchievementCommandHandler : IRequestHandler<CreateAchievementCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public CreateAchievementCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
        {
            var data = request.Data;
            //TODO: validate data 
            if (data.RefId == Guid.Empty)
            {
                return false;
            }
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == data.RefId, cancellationToken);

            if (user is null || user.UserType is not UserType.Athlete)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            var achievement = new Achievement()
            {
                Title = data.Title,
                Description = data.Description,
                DateAchieved = data.DateAchieved.ToUniversalTime(),
                AthleteId = user.Id,
                Location = data.Location,
                
            };

            await _context.Achievements.AddAsync(achievement, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}