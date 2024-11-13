using Application.Contracts;
using Application.Features.Achievements;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class CreateServiceCommand: IRequest<Result<bool,ValidationFailed>>
{
    public ServiceDto CustomService { get; set; } = default!;
    
    class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public CreateServiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var data = request.CustomService;
            //TODO: validate data 
            if (data.RefId == Guid.Empty)
            {
                return false;
            }
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == data.RefId, cancellationToken);

            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }
            
            var service = new Service()
            {
                AppUser = user,
                CreatedTime = DateTime.UtcNow,
                Title = data.Title,
                Description = data.Description,
                Duration = data.Duration,
                ServiceTypeId = data.ServiceTypeId,
                IsActive = true,
                IsValid = true
            };

            await _context.Services.AddAsync(service, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}