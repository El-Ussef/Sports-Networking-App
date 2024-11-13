using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.UpdateProfile;

/// <summary>
/// check if we don't need this 
/// </summary>
public class UpdatePhysicalInformationCommand : IRequest<Result<bool,ValidationFailed>>
{
    public string RefId { get; set; }

    public PhysicalInformationDto Data { get; set; }

    class UpdatePhysicalInformationCommandHandler : IRequestHandler<UpdatePhysicalInformationCommand, Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        
        public UpdatePhysicalInformationCommandHandler(
            IApplicationDbContext context
        )
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(UpdatePhysicalInformationCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId.ToString() == request.RefId,cancellationToken);
            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }
            
            var data = request.Data;

            if (!string.IsNullOrEmpty(data.Height))
            {
                user.Height = data.Height;
            }
            
            if (!string.IsNullOrEmpty(data.Weight))
            {
                user.Weight = data.Weight;
            }
            
            if (!string.IsNullOrEmpty(data.TopSize))
            {
                user.TopSize = data.TopSize;
            }
            
            if (!string.IsNullOrEmpty(data.BottomSize))
            {
                user.BottomSize = data.BottomSize;
            }
            
            if (!string.IsNullOrEmpty(data.FootWear))
            {
                user.FootWear = data.FootWear;
            }
            
            _context.AppUsers.Update(user);
                
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            return result;
        }
    }

}