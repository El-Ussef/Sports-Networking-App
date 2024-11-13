using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class DeleteServiceCommand: IRequest<Result<bool,ValidationFailed>>
{
    //int id,Guid userId
    public int Id { get; set; }

    public Guid RefId { get; set; }
    
    class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteServiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {

            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == request.RefId, cancellationToken);

            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            var tempService = await _context.Services.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (tempService is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("tempService", "Failed to find Service");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            _context.Services.Remove(tempService);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}