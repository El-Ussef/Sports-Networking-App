using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.GetProfileById;

public class GetPhysicalInformationQuery : IRequest<Result<PhysicalInformationDto,ValidationFailed>>
{
    public string Type { get; set; }
    public string RefId { get; set; }

    class GetPhysicalInformationQueryHandler : IRequestHandler<GetPhysicalInformationQuery,Result<PhysicalInformationDto,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetPhysicalInformationQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<PhysicalInformationDto, ValidationFailed>> Handle(GetPhysicalInformationQuery request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<UserType>(request.Type, true, out var userTypeEnum))
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user type");
                return new Result<PhysicalInformationDto, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var user = await _context.AppUsers
                .Where(u => u.UserType == userTypeEnum && u.RefId.ToString() == request.RefId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<PhysicalInformationDto, ValidationFailed>(new ValidationFailed(failure));
            }
            
            var result = new PhysicalInformationDto
            {
                Height = user.Height,
                Weight = user.Weight,
                BottomSize = user.BottomSize,
                TopSize = user.TopSize,
                FootWear = user.FootWear
                
            };
            return result;

        }
    }
}