using Application.Contracts;
using Application.Features.Profile;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class GetServicesByUserIdQuery : IRequest<Result<List<ServiceResponse>,ValidationFailed>>
{
    public Guid UserId { get; set; }
    
    public class GetServicesByUserIdQueryHandler : IRequestHandler<GetServicesByUserIdQuery,
            Result<List<ServiceResponse>, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        
        public GetServicesByUserIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ServiceResponse>, ValidationFailed>> Handle(GetServicesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Where(u => u.RefId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (user is null)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<List<ServiceResponse>, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var services = await _context.Services
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
            
            return services;
        }
    }
}