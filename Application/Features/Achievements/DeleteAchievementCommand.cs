using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Achievements;

public class DeleteAchievementCommand: IRequest<Result<bool,ValidationFailed>>
{
    public int Id { get; set; }
    
    public Guid RefId { get; set; }
    
    class DeleteAchievementCommandHandler : IRequestHandler<DeleteAchievementCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAchievementCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(DeleteAchievementCommand request, CancellationToken cancellationToken)
        {

            var tempAchievement = await _context.Achievements.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (tempAchievement is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("Achievement", "Failed to find Achievement");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == request.RefId, cancellationToken);

            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            _context.Achievements.Remove(tempAchievement);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}