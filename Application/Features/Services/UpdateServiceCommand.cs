using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class UpdateServiceCommand: IRequest<Result<bool,ValidationFailed>>
{
    //public UpdateServiceDto Data { get; set; }

    public ServiceDto Data { get; set; }
    
    class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateServiceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var data = request.Data;

            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == data.RefId, cancellationToken);

            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            var tempService = await _context.Services.FirstOrDefaultAsync(u => u.Id == data.Id, cancellationToken);

            if (tempService is not null)
            {
                if (!string.IsNullOrEmpty(data.Title))
                {
                    tempService.Title = data.Title;
                }
            
                if (!string.IsNullOrEmpty(data.Description))
                {
                    tempService.Description = data.Description;
                }
                if (!string.IsNullOrEmpty(data.Duration))
                {
                    tempService.Duration = data.Duration;
                }
                if (data.Price is > 0)
                {
                    tempService.Price = data.Price;
                }
                
                if (data.ServiceTypeId is > 0)
                {
                    tempService.ServiceTypeId = data.ServiceTypeId;
                }
                
                tempService.ModifiedDate = DateTime.UtcNow;
                _context.Services.Update(tempService);
            }

                // switch (service.Operation)
                // {
                //     case OperationCode.ADDED:
                //     {
                //         if (!string.IsNullOrEmpty(service.Label))
                //         {
                //             var newService = new Service()
                //             {
                //                 AppUser = user,
                //                 CreatedTime = DateTime.UtcNow,
                //                 CustomService = service.Label,
                //                 IsActive = true,
                //                 IsDeleted = false,
                //                 IsValid = true
                //             };
                //             await _context.Services.AddAsync(newService);
                //         }
                //
                //         break;
                //     }
                //     case OperationCode.UPDATED:
                //     {
                //         
                //
                //         break;
                //     }
                //     case OperationCode.DELETED:
                //     {
                //         var tempService = await _context.Services.FirstOrDefaultAsync(u => u.Id == service.Id, cancellationToken);
                //         if (tempService is not null)
                //         {
                //             _context.Services.Remove(tempService);
                //         }
                //
                //         break;
                //     }
                //     case OperationCode.UNCHANGED:
                //         break;
                //     default:
                //         throw new ArgumentOutOfRangeException();
                // }
            
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}