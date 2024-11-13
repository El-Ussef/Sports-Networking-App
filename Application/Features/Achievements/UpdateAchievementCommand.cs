using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Achievements;

public class UpdateAchievementCommand: IRequest<Result<bool,ValidationFailed>>
{
    public int Id { get; set; }
    public AchievementDto Data { get; set; }
    
    class UpdateAchievementCommandHandler : IRequestHandler<UpdateAchievementCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAchievementCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
        {
            var data = request.Data;

            var tempAchievement = await _context.Achievements.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (tempAchievement is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("Achievement", "Failed to find Achievement");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            if (!string.IsNullOrEmpty(data.Title))
            {
                tempAchievement.Title = data.Title;
            }
            
            if (!string.IsNullOrEmpty(data.Description))
            {
                tempAchievement.Description = data.Description;
            }
            
            if (!string.IsNullOrEmpty(data.Location))
            {
                tempAchievement.Location = data.Location;
            }
            
            tempAchievement.DateAchieved = data.DateAchieved.ToUniversalTime();
            
            _context.Achievements.Update(tempAchievement);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}